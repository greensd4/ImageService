using Communication.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Communication.Client
{
    public class ISClient : IISClient 
    {

        #region Properties, Members and Events
        private static Mutex readerMutex = new Mutex();
        private static Mutex writerMutex = new Mutex();
        public int Port { get; set; }
        public string IP { get; set; }
        public event EventHandler<string> MessageRecieved;
        private TcpClient client;
        private IPEndPoint ep;
        private int portNumber;

        //Propeties
        public bool Connection { get { return client.Connected; } }
       
        //Singleton!
        private static ISClient clientService;
        
        //instance
        public static ISClient ClientServiceIns
        {
            get
            {
                if (clientService == null)
                {
                    try
                    {
                        clientService = new ISClient();
                    } catch(Exception e)
                    {
                        throw e;
                    }
                }
                return clientService;
            }
        }
        #endregion
        /// <summary>
        /// Configuration of server
        /// </summary>
        public void ServerConfig()
        {
            IP = SettingsHolder.IP;
            Port = SettingsHolder.PortByKey["regular"];
        }
        /// <summary>
        /// private constructor for singleton
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        private ISClient()
        {
            ServerConfig();
            portNumber = Port;
            client = new TcpClient();
            ep = new IPEndPoint(IPAddress.Parse(IP), Port);
            CreateANewConnection();
        }
        /// <summary>
        /// Creates new connection
        /// </summary>
        private void CreateANewConnection()
        {
            try
            { 
                client.Connect(this.ep);//connect to the server
            } catch (Exception e)
            {
                throw e;
            }
            Console.WriteLine("you are connected ");
            Read();
        }

        /// <summary>
        /// writes string to server
        /// </summary>
        /// <param name="command"></param>
        public void Write(string command)
        {
           new Task(() =>
            { 
                if (!Connection && clientService != null)
                {
                    CreateANewConnection();
                }
                NetworkStream stream = client.GetStream();
                BinaryWriter writer = new BinaryWriter(stream);
                writerMutex.WaitOne();
                Debug.WriteLine("Writing : " + command);
                writer.Write(command);
                writer.Flush();
                writerMutex.ReleaseMutex();
            }).Start();
        }
        /// <summary>
        /// Reads from server , runs in different task
        /// </summary>
        public void Read()
        {
            new Task(() =>
            {
                try
                {
                    while (Connection)
                    {
                        NetworkStream stream = client.GetStream();
                        BinaryReader reader = new BinaryReader(stream);
                        string result = reader.ReadString();
                        Console.WriteLine("Recieved " + result);
                        if (result == null)
                        {
                            return;
                        }
                        MessageRecieved?.Invoke(this, result);
                        result = string.Empty;
                    }
                } catch(Exception e)
                {

                }
            }).Start();
        }
        /// <summary>
        /// Disconnect
        /// </summary>
        public void Disconnect()
        {
            if (client != null)
            {

                client.GetStream().Close();
                client.Close();
                client = null;
            }
        }
       
    }
}
