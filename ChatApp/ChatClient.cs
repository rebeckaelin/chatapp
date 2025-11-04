namespace ChatApp;
using SocketIOClient;

public class ChatClient
{
    private static readonly SocketIO _client;
    private static readonly string Path = "/sys25d";
    private readonly string _username;

    public static async Task Connect()
    {
        _client = new SocketIO(uri: "wss://api.leetcode.se", new SocketIOOptions
        {
            Path = Path
        });
        
        _client.On("chatMessage" , response =>
        {
            var receivedMessage = response.GetValue<string>();
            Console.WriteLine(receivedMessage);
        });
        
        _client.OnConnected += async (sender, eventArgs) =>
        {
            Console.WriteLine("Connected");
        };
        
        _client.OnDisconnected += async (sender, eventArgs) =>
        {
            Console.WriteLine("Disconnected");
        };

        await _client.ConnectAsync();
        await Task.Delay(1000);
    }
    
    public      static async Task SendMessage(string message)
    {
      
        // Console.WriteLine($"[Debug] Sending message: {message}");
        await _client.EmitAsync("chatMessage", message);
        Console.WriteLine("[Debug] Message sent");
        
    }
}