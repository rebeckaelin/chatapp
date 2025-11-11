namespace ChatApp;

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
        
        Console.WriteLine("╔═══════════════════════════════════╗");
        Console.WriteLine("║         WELCOME TO rCHAT          ║");
        Console.WriteLine("║        available commands         ║");
        Console.WriteLine("╠═══════════════════════════════════╣");
        Console.WriteLine("║ /rules  - How to behave           ║");
        Console.WriteLine("║ /online  - Show online users      ║");
        Console.WriteLine("║ /join room  - **COMING SOON**     ║");
        Console.WriteLine("║ /dm <username>  - **COMING SOON** ║");
        Console.WriteLine("║ /exit  - Leave the chat           ║");
        Console.WriteLine("╚═══════════════════════════════════╝");
        Console.WriteLine($"You are connected as: [{username}]");
        
        var inputBuffer = new System.Text.StringBuilder();
        
        while (true)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(intercept: true);
                
                if (key.Key == ConsoleKey.Enter)
                {
                    var input = inputBuffer.ToString();
                    inputBuffer.Clear();
                    ChatClient.ClearCurrentInput();
                    
                    Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");
                    
                    if (input == "/rules")
                    {
                        ShowRules();
                        continue;
                    }
                    
                    if (input == "/online")
                    {
                        Console.WriteLine($"Online: {chat.GetOnlineUserCount()}");
                        foreach (var user in chat.GetOnlineUsers())
                        {
                            Console.WriteLine($"- {user}");
                        }
                        continue;
                    }
                    
                    if (input == "/exit")
                    {
                        await chat.DisconnectAsync();
                        break;
                    }

                    if (!string.IsNullOrWhiteSpace(input))
                    {
                        await chat.SendMessageAsync(input);
                    }
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (inputBuffer.Length > 0)
                    {
                        inputBuffer.Remove(inputBuffer.Length - 1, 1);
                        ChatClient.UpdateCurrentInput(inputBuffer.ToString());
                        
                        Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");
                        Console.Write(inputBuffer.ToString());
                    }
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    inputBuffer.Append(key.KeyChar);
                    ChatClient.UpdateCurrentInput(inputBuffer.ToString());
                    Console.Write(key.KeyChar);
                }
            }
            
            await Task.Delay(10);
        }
        
        Console.WriteLine("Goodbye!");
    }
    static void ShowRules()
    {
        Console.WriteLine("╔═══════════════════════════════════╗");
        Console.WriteLine("║          RULES IN rCHAT           ║");
        Console.WriteLine("║        aka THE VIBE GUIDE         ║");
        Console.WriteLine("╠═══════════════════════════════════╣");
        Console.WriteLine("║ 1: Respect the vibe.              ║");
        Console.WriteLine("║ 2: Share, dont shout.             ║");
        Console.WriteLine("║ 3: Stay inspired, not expired.    ║");
        Console.WriteLine("║ 4: Help, dont heckle.             ║");
        Console.WriteLine("║ 4: Enjoy the flow.                ║");
        Console.WriteLine("╚═══════════════════════════════════╝");
    }
}