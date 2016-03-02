using System;
using System.Net;
using StackExchange.NetGain;
using StackExchange.NetGain.WebSockets;

namespace NetGain
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("172.16.2.114"), 6002);//(IPAddress.Loopback, 6002);
            using (var server = new ChatTcpServer(1))
            {
                server.ProtocolFactory = WebSocketsSelectorProcessor.Default;
                server.ConnectionTimeoutSeconds = 60;
                server.Received += msg =>
                {
                    Console.WriteLine("[server] {0}", msg.Value);
                };

                server.Start("abc", endpoint);
                Console.WriteLine("Server running");

                Console.ReadKey();
            }
            Console.WriteLine("Server dead; press any key");
            Console.ReadKey();
        }
    //}
}