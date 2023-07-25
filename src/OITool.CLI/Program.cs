using System.Drawing;
using System.Text;
using System.IO.Pipes;
using System.Security.Cryptography;
using Nefe.ColorConsole;
using System.Text.Json;

namespace OITool.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
            await using var client = new Nefe.Pipe.NamedPipe.NamedPipeClient("OITool.Server");
            
            await client.ConnectAsync();
            if (client.IsConnected)
            {
                var command = Console.ReadLine() ?? "";
                await client.SendBytesAsync(RandomNumberGenerator.GetBytes(114514));
                await client.SendBytesAsync(Encoding.UTF8.GetBytes(command));
                
                var result = Encoding.UTF8.GetString(await client.ReceiveBytesAsync());
                Console.WriteLine($"Result: {result}");
                client.Close();
            }
            else Console.WriteLine("Failed to connect");
            */
        }
    }
}
