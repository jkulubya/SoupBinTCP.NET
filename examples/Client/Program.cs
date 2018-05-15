using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SoupBinTCP.NET;
using SoupBinTCP.NET.Messages;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting client");
            var client = new SoupBinTCP.NET.Client(IPAddress.Parse("127.0.0.1"), 5500, new LoginDetails()
            {
                Password = "password",
                Username = "user"
            }, new ClientListener());
            client.Start();
            var command = Console.ReadLine();
            while (command != "x")
            {
                Message message;
                switch (command)
                {
                        case "d":
                            message = new Debug("debug message!!");
                            break;
                        case "s":
                            message = new UnsequencedData(Encoding.ASCII.GetBytes("unsequenced"));
                            break;
                        case "l":
                            message = new LoginRequest("user", "password");
                            break;
                        default:
                            message = new Debug("default");
                            break;
                }
                await client.Send(message);
                command = Console.ReadLine();
            }
            await client.Shutdown();
        }
    }
}