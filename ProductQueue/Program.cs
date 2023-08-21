using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ProductQueueApp
{
    internal sealed class Program
    {
        public static void Main(string[] args)
        {
            const int maxQueueSize = 5;
            var server = new TcpListener(IPAddress.Any, 8888);
            server.Start();

            Console.WriteLine("Ожидание подключения...");

            var client = server.AcceptTcpClient();
            Console.WriteLine("Подключено!");

            var stream = client.GetStream();
            
            try
            {
                var productQueue = new Queue<string>();
                while (true)
                {
                    var data = new byte[256];
                    var bytesRead = stream.Read(data, 0, data.Length);
                    var message = Encoding.ASCII.GetString(data, 0, bytesRead);

                    if (message.StartsWith("CAMERA:"))
                    {
                        var parts = message.Split(':');
                        if (productQueue.Count < maxQueueSize)
                        {
                            var quality = parts[1] == "1" ? "[Г]" : "[Б]";
                            productQueue.Enqueue(quality);
                        }
                    }
                    else if (message == "PUSHER")
                    {
                        if (productQueue.Count > 0)
                        {
                            productQueue.Dequeue();
                        }
                    }

                    Console.Clear();
                    foreach (var product in productQueue.Reverse())
                    {
                        Console.Write(product);
                    }
                }

            }
            finally
            {
                stream.Close();
                client.Close();
                server.Stop();
            }
            
        }
    }
}