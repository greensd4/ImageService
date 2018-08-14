using Communication.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Server
{
    public class ISMobileServer : IISServer
    {
        public IISClientHandler ClientHandler { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        public TcpListener Listener { get; set; }

        public ISMobileServer(IISClientHandler ch)
        {
            this.ClientHandler = ch;
            ServerConfig();
        }
        public void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(IP), Port);
            Listener = new TcpListener(ep);
            Listener.Start();
            Console.WriteLine("Waiting for connections...");
            Task task = new Task(() =>//creating a listening thread that keeps running.
            {
                while (true)
                {
                    try
                    {
                       
                        TcpClient client = Listener.AcceptTcpClient(); //recieve new client
                        Console.WriteLine("Got new connection");

                        ClientHandler.HandleClient(client, 1); //handle the player through the client handler
                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }
            });
            task.Start();
        }
        /// <summary>
        /// Stop
        /// </summary>
        public void Stop()
        {
            Listener.Stop();
        }
        /// <summary>
        /// Configuration of server
        /// </summary>
        private void ServerConfig()
        {
            IP = SettingsHolder.IP;
            Port = SettingsHolder.PortByKey["mobile"];
        }
    }
}
