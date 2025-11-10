namespace ChatApp;
using SocketIOClient;

class Program
{
    static async Task Main(string[] args)
    {
        string username;
        do
        {
            Console.Write("Enter your name: ");
            username = Console.ReadLine()?.Trim() ?? "Guest";
            
            if (string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine("Username cannot be empty. Please try again.");
            }
        } while (string.IsNullOrWhiteSpace(username));
        
        var chat = new ChatClient(username);
        await chat.ConnectAsync();
        
        Console.Clear();
        Console.WriteLine($"Welcome to rCHAT - Connected as {username}");
        Console.WriteLine("Type your message here and press Enter, type '/help' for commands.\n");
        
        while (true)
        {
            var input = Console.ReadLine();

            if (input == "/help")
            {
                ShowHelp();
                continue;
            }
            
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

    static void ShowHelp()
    {
        Console.WriteLine("\n--- Available Commands ---");
        Console.WriteLine("/help  - Show this menu");
        Console.WriteLine("/exit  - Leave the chat");
        Console.WriteLine("--------------------------\n");
    }
}