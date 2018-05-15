using System;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting server");
            var server = new SoupBinTCP.NET.Server(new ServerListener());
            server.Start();
            var command = "";
            while (command != "x")
            {
                command = Console.ReadLine();
            }
            await server.Shutdown();
        }
    }
}