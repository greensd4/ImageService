using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Logging;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using StreamJsonRpc;
using System.Threading.Tasks;
using Communication.Server;
using Communication.Infrastructure;
using System.Threading;
using System.Diagnostics;

namespace ImageService.Server
{
    /// <summary>
    /// ImageServer . Server for the Service - Creating Directory handlers for each directory.
    /// Connecting between the service and the handlers.
    /// </summary>
    public class ImageServer : IISClientHandler
    {
        #region Members, Constructor
        private IImageController m_controller;
        private ILoggingService m_logging;
        private Dictionary<TcpClient, bool> clientsReadyForNewLogs;
        private List<string> dirPaths;
        private static Mutex writeMutex = new Mutex();
        private static Mutex removeMutex = new Mutex();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggingService">Log</param>
        /// <param name="imageController">Controller</param>
        public ImageServer(ILoggingService loggingService, IImageController imageController)
        {
            this.m_controller = imageController;
            this.m_logging = loggingService;
            this.clientsReadyForNewLogs = new Dictionary<TcpClient, bool>();
           // this.m_logging.MessageRecieved += this.NewLogEntry;
            string[] arr = ConfigurationManager.AppSettings["Handler"].Split(';');
            dirPaths = new List<string>(arr);
            //Creates the direcory handlers for each directory path recieved.
            m_logging.Log("Image server was created, making handlers now", MessageTypeEnum.INFO);
            foreach (string path in dirPaths)
            {
                if (Directory.Exists(path))
                {
                    m_logging.Log("Creating handler for :" + path, MessageTypeEnum.INFO);
                    this.CreateHandler(path);
                }
            }
        }

        #endregion
        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        public event EventHandler<DirectoryCloseEventArgs> CloseEvent;
        #endregion
        #region Methods
        /// <summary>
        /// CreateHandler :
        /// creates the handler for a given directory's path.
        /// </summary>
        /// <param name="dirPath"></param>
        public void CreateHandler(string dirPath)
        {
            m_logging.Log("In create handler", MessageTypeEnum.INFO);
            IDirectoryHandler dirHandler = new DirectoyHandler(dirPath, m_logging, m_controller);
            CommandRecieved += dirHandler.OnCommandRecieved;
            CloseEvent += dirHandler.CloseHandler;
            dirHandler.StartHandleDirectory(dirPath);
            this.m_logging.Log("Created handler for: " + dirPath, MessageTypeEnum.INFO);
        }
        /// <summary>
        /// InvokeCommand 
        /// </summary>
        /// <param name="commandArgs"></param>
        public void InvokeCommand(CommandRecievedEventArgs commandArgs)
        {
            CommandRecieved?.Invoke(this, commandArgs);
        }
        /// <summary>
        /// OnClose the server need to close all the handlers.
        /// </summary>
        public void OnClose()
        {
            try
            {
                foreach (string path in dirPaths)
                {
                    CloseDirectoryHandler(path);
                }
            }
            catch (Exception e)
            {
                e.ToString();
                m_logging.Log("Error in closing the handlers", MessageTypeEnum.FAIL);
            }
        }
        /// <summary>
        /// Handle client
        /// </summary>
        /// <param name="client"></param>
        public void HandleClient(TcpClient client, int flag)
        {
            m_logging.Log("Client connected from flag " + flag.ToString(), MessageTypeEnum.INFO);

            Debug.WriteLine("##############################");
            this.clientsReadyForNewLogs.Add(client, false);
            if (flag == 0)
            {
                HandleRegularClient(client);
            }
            if (flag == 1)
            {
                HandleMobileClient(client);
            }

        }
        private void HandleMobileClient(TcpClient client) 
        {
            new Task(() =>
            {
                try
                {
                    while (client.Connected)
                    {
                        NetworkStream stream = client.GetStream();
                        //get the image name
                        byte[] nameInBytes = ReadName(stream);
                        string name = Encoding.UTF8.GetString(nameInBytes);
                        //tell the client we got the name and return 0 if it is already exists
                        Byte[] confirmation = new byte[1];
                        if (CheckIfFileExistsAlready(name) == 1)
                        {
                            confirmation[0] = 0;
                            stream.Write(confirmation, 0, 1);
                            continue;
                        }
                        else
                        {
                            confirmation[0] = 1;
                            stream.Write(confirmation, 0, 1);
                        }
                       
                        
                        //read the image
                        byte[] photoBytes = ReadPhotoBytes(stream);
                        SettingsObject settings = SettingsObject.GetInstance;
                        string handler = (settings.Handlers.Split(';'))[0];
                        File.WriteAllBytes(handler + @"\" + name, photoBytes);
                        stream.Write(confirmation, 0, 1);
                    }
                }
                catch (Exception ex)
                {
                    this.clientsReadyForNewLogs.Remove(client);
                    m_logging.Log(ex.ToString(), MessageTypeEnum.FAIL);
                    client.Close();
                }

            }).Start();

        }
        private byte[] ReadPhotoBytes(NetworkStream stream)
        {
            List<Byte> bytesArr = new List<byte>();
            Byte[] temp;
            Byte[] data = new Byte[6790];
            int size = 0;
            //start reading the bytes in parts to get the whole image
            do
            {
                size = stream.Read(data, 0, data.Length);
                temp = new byte[size];
                for (int n = 0; n < size; n++)
                {
                    temp[n] = data[n];
                    bytesArr.Add(temp[n]);

                }
                System.Threading.Thread.Sleep(300);
            } while (stream.DataAvailable || size  == data.Length);
            return bytesArr.ToArray();
        }
        private byte[] ReadName(NetworkStream stream)
        {
            Byte[] temp = new Byte[1];
            List<Byte> fileName = new List<byte>();
            //read the file name
            do
            {
                stream.Read(temp, 0, 1);
                fileName.Add(temp[0]);
            } while (stream.DataAvailable);
            return fileName.ToArray();
        } 
        private int CheckIfFileExistsAlready(string name)
        {
            SettingsObject s = SettingsObject.GetInstance;
            string path = Path.Combine(s.OutPutDir, name);
            string[] files = Directory.GetFiles(s.OutPutDir, "*", SearchOption.AllDirectories);
            foreach(string f in files)
            {
                if (f.Contains(name))
                    return 1;
            }
            return 0;
        }
        
        private void HandleRegularClient(TcpClient client)
        {
            new Task(() =>
            {
                NetworkStream stream = client.GetStream();
                BinaryReader reader = new BinaryReader(stream);
                while (client.Connected)
                {
                    string commandLine = reader.ReadString();
                    if (commandLine == null)
                        continue;
                    CommandRecievedEventArgs crea = CommandRecievedEventArgs.FromJson(commandLine);
                    if (crea.CommandID == (int)CommandEnum.WindowClosedCommand)
                    {
                        m_logging.Log("in close window command", MessageTypeEnum.WARNING);
                        client.GetStream().Close();
                        client.Close();
                        break;
                    }
                    bool result;
                    string res = this.m_controller.ExecuteCommand(crea.CommandID, crea.Args, out result);
                    PublishResult(res);
                    res = string.Empty;
                    if (crea.CommandID == (int)CommandEnum.LogCommand)
                    {
                        //Ready to get new logs entries
                        clientsReadyForNewLogs[client] = true;
                    }
                    if (crea.CommandID == (int)CommandEnum.CloseCommand)
                    {
                        CloseDirectoryHandler(crea.Args[0]);
                    }
                }
            }).Start();
        }
        /// <summary>
        /// Publish result of the controller to all clients
        /// </summary>
        /// <param name="result"></param>
        public void PublishResult(string result)
        {
            new Task(() =>
            {
                foreach (TcpClient client in clientsReadyForNewLogs.Keys)
                {

                    if (client.Connected)
                    {
                        MutexedWriter(client, result);
                    }
                    else
                    {
                        clientsReadyForNewLogs.Remove(client);
                    }

                }
            }).Start();
        }
        /// <summary>
        /// Mutexed writer
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        private void MutexedWriter(TcpClient client, string message)
        {
            NetworkStream stream = client.GetStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writeMutex.WaitOne();
            writer.Write(message);
            writeMutex.ReleaseMutex();
        }
        /// <summary>
        /// Notify for new log entry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="m"></param>
        public void NewLogEntry(object sender, MessageRecievedEventArgs m)
        {
            if (clientsReadyForNewLogs.Keys.Count == 0)
                return;
            string[] args = new string[1];
            args[0] = m.ToJson();
            CommandRecievedEventArgs c = new CommandRecievedEventArgs((int)CommandEnum.NewLogEntryCommand, args, null);
            new Task(() =>
            {
                foreach (TcpClient client in clientsReadyForNewLogs.Keys)
                {

                    if (client.Connected)
                    {
                        if (clientsReadyForNewLogs[client])
                        {
                            MutexedWriter(client, c.ToJson());
                        }
                    }
                    else
                    {
                        clientsReadyForNewLogs.Remove(client);
                    }

                }
            }).Start();
        }
        private void CloseDirectoryHandler(string path)
        {
            m_logging.Log("Server closing the handler: " + path, MessageTypeEnum.INFO);
            CloseEvent?.Invoke(this, new DirectoryCloseEventArgs(path, null));
            this.dirPaths.Remove(path);
        }
        #endregion
    }


}