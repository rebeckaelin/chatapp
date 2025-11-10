namespace ChatApp;
using SocketIOClient;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Write("Enter your name: ");
        var username = Console.ReadLine() ?? "Guest";
        
        // if ()
        
        var chat = new ChatClient(username);
        await chat.ConnectAsync();
        
        Console.Clear();
        Console.WriteLine($"Welcome to rCHAT");
        Console.WriteLine($"Connected as {username}");
        Console.WriteLine("Type your message here and press Enter, type '/exit' to quit.\n");
        
        
        while (true)
        {
            var input = Console.ReadLine();

            if (input == "/exit")
            {
                await chat.DisconnectAsync();
                break;
            }

            if (!string.IsNullOrWhiteSpace(input))
            { 
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop);
                await chat.SendMessageAsync(input);
            }
        }
        Console.WriteLine("Goodbye!");
    }
}