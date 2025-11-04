namespace ChatApp;
using SocketIOClient;

class Program
{


    
    static async Task Main(string[] args)
    {

        await ChatClient.Connect();
        
         while (true)
         {
             Console.Write("Type something");
        
             var input = Console.ReadLine();
        
             await ChatClient.SendMessage(input ?? "");
        
         }
    }
}