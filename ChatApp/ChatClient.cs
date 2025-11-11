namespace ChatApp;
using SocketIOClient;

public class ChatClient
{
    private readonly SocketIO _client;
    private static readonly string _path = "/sys25d";
    private readonly string _username;
    private List<string> _usersOnline = new List<string>();

    public ChatClient(string username)
    {
        _username = username;
        _client = new SocketIO("wss://api.leetcode.se", new SocketIOOptions { Path = _path });
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        
        _client.On("onlineUsers", response =>
        {
            var users = response.GetValue<string[]>();
            _usersOnline = new List<string>(users);
        });
        _client.OnConnected += async (sender, e) =>
        {
            await _client.EmitAsync("rJoin", _username);
        };
        
        _client.OnDisconnected += (sender, e) =>
        {
            var statusMessage = new StatusMessage
            {
                Status = "Disconnected from server."
            };
            DisplayMessage(statusMessage);
        };
        
        _client.On("rChat", (Action<SocketIOResponse>)(response =>
        {
            try
            {
                var chatMsg = response.GetValue<ChatMessage>(0);
                if (chatMsg != null && chatMsg.UserName !=_username)
                {
                    DisplayMessage(chatMsg);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error {ex.Message}");
            }
        }));

        
        _client.On("rStatus", response =>
        {
            var statusText = response.GetValue<string>();
            var statusMsg = new StatusMessage { Status = statusText };
            DisplayMessage(statusMsg);
        });
        
        _client.On("rJoin", response =>
        {
            try
            {
                var username = response.GetValue<string>(); 
                _usersOnline.Add(username);
                var joinMsg = new SystemMessage 
                { 
                    Event = "join", 
                    Username = username 
                };
                DisplayMessage(joinMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Join error: {ex.Message}");
            }
    
        });

        _client.On("rLeave", response =>
        {
            try 
            {
                var username = response.GetValue<string>();
                _usersOnline.Remove(username);
                var leaveMsg = new SystemMessage    
                { 
                    Event = "leave", 
                    Username = username 
                };
                DisplayMessage(leaveMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Leave error: {ex.Message}");
            }
        });
        
    }
        public async Task ConnectAsync()
        {
            await _client.ConnectAsync();
            _usersOnline.Add(_username);
            await Task.Delay(1000);
        }

        public async Task SendMessageAsync(string message)
        {
            var chatMessage = new ChatMessage
            {
                UserName = _username,
                Message = message,
            };
            
            Console.WriteLine(chatMessage.FormatDisplay());
            await _client.EmitAsync("rChat", chatMessage);
        }

        public async Task DisconnectAsync()
        {
            await _client.EmitAsync("rLeave", _username);
            _usersOnline.Remove(_username);
            await _client.DisconnectAsync();
        }
        private static void DisplayMessage(BaseMessage message)
        {
            Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r"); 
            Console.WriteLine(message.FormatDisplay());
        }

        public int GetOnlineUserCount()
        {
            return _usersOnline.Count;
        }

        public List<string> GetOnlineUsers()
        {
            return new List<string>(_usersOnline);
        }
        public async Task RequestOnlineUsersAsync()
        {
            await _client.EmitAsync("getOnlineUsers");
        }
}