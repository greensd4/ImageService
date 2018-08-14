using Communication.Client;
using Communication.Infrastructure;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SettingsView.Model
{
    class ConfigModel : IConfigModel
    {
        #region Members, Constructor
        private IISClient client;
        public event PropertyChangedEventHandler PropertyChanged;
        public ConfigModel()
        {
            try
            {
                this.client = ISClient.ClientServiceIns;
                this.client.MessageRecieved += GetMessageFromClient;
                SendCommandToService(new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, null, null));
            } catch (Exception e)
            {
                NotConnectedValues();
            }
        }
        #endregion
        #region Write/Read
        public void GetMessageFromClient(object sender, string message)
        {
            CommandRecievedEventArgs command = CommandRecievedEventArgs.FromJson(message);
            if (command.CommandID == (int) CommandEnum.GetConfigCommand)
            {
                string m = command.Args[0];
                JObject json = JObject.Parse(m);
                OutputDir = (string)json["OutputDir"];
                SourceName = (string)json["SourceName"];
                ThumbnailSize = int.Parse((string)json["ThumbnailSize"]);
                LogName = (string)json["LogName"];
                UpdateHandlersFromString((string)json["Handler"]);
            }
            else if (command.CommandID == (int) CommandEnum.CloseCommand) 
            {
                UpdateHandlersFromString(command.Args[0]);
            }
        }
        public void SendCommandToService(CommandRecievedEventArgs command)
        {

            client.Write(command.ToJson());
        }
        #endregion
        #region Setting's Properties 
        private string outputDir;
        public string OutputDir
        {
            get { return this.outputDir; }
            set
            {
                this.outputDir = value;
                NotifyPropertyChanged("OutputDir");
            }
        }
        private string sourceName;
        public string SourceName
        {
            get { return this.sourceName; }
            set
            {
                this.sourceName = value;
                NotifyPropertyChanged("SourceName");
            }
        }
        private string logName;
        public string LogName
        {
            get { return this.logName; }
            set
            {
                this.logName = value;
                NotifyPropertyChanged("LogName");
            }
        }
        private int thumbnailSize;
        public int ThumbnailSize
        {
            get { return this.thumbnailSize; }
            set
            {

                this.thumbnailSize = value;
                NotifyPropertyChanged("ThumbnailSize");
            }
        }
        private ObservableCollection<string> handlers;
        public ObservableCollection<string> Handlers
        {
            get { return this.handlers; }
            set
            {
                this.handlers = value;
                NotifyPropertyChanged("Handlers");
            }
        }
        #endregion
        #region Methods
        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        public void UpdateHandlersFromString(string handlers)
        {
            string[] result = handlers.Split(';');
            if (handlers == "" || handlers == null || handlers == string.Empty || handlers == ";")
            {
                Handlers = new ObservableCollection<string>();

            }
            else
            {
                Handlers = new ObservableCollection<string>(result);
            }
        }
        public void NotConnectedValues()
        {
            OutputDir = "Not connected to server!";
            SourceName = null;
            ThumbnailSize = 0;
            LogName = null;
            Handlers = null;
        }
        #endregion
    }
}
