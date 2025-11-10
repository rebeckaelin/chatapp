namespace ChatApp;
using SocketIOClient;

public class ChatClient
{
    private readonly SocketIO _client;
    private static readonly string _path = "/sys25d";
    private readonly string _username;

    public ChatClient(string username)
    {
        _username = username;
        _client = new SocketIO("wss://api.leetcode.se", new SocketIOOptions { Path = _path });
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        _client.OnConnected += async (sender, e) =>
        {
            await _client.EmitAsync("join", _username);
        };
        
        _client.OnDisconnected += (sender, e) =>
        {
           Console.WriteLine("Disconnected from server"); 
        };
        
        _client.On("chatmessage", (Action<SocketIOResponse>)(response =>
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
        
        _client.On("join", response =>
        {
            var username = response.GetValue<string>();
            var joinMsg = new SystemMessage 
            { 
                Event = "join", 
                Username = username 
            };
            DisplayMessage(joinMsg);
        });

        _client.On("leave", response =>
        {
            try 
            {
                var username = response.GetValue<string>();
                var leaveMsg = new SystemMessage    
                { 
                    Event = "leave", 
                    Username = username 
                };
                DisplayMessage(leaveMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Debug] Error: {ex.Message}");
            }
        });
    }
        public async Task ConnectAsync()
        {
            await _client.ConnectAsync();
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
            await _client.EmitAsync("chatmessage", chatMessage);
        }

        public async Task DisconnectAsync()
        {
            await _client.EmitAsync("leave", _username);
            await _client.DisconnectAsync();
        }
        private void DisplayMessage(BaseMessage message)
        {
            Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r"); 
            Console.WriteLine(message.FormatDisplay());
        }
}