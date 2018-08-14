using Communication.Client;
using Communication.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SettingsView.Model
{
    class LogModel : ILogModel
    {
        #region Members, Properties, Constructor
        private IISClient client;
        public event PropertyChangedEventHandler PropertyChanged;
         
        private ObservableCollection<MessageRecievedEventArgs> logs;
        public ObservableCollection<MessageRecievedEventArgs> Logs
        {
            set
            {
                logs = value;
                NotifyPropertyChanged("Logs");
            }
            get { return logs; }
        }
        public LogModel()
        {
            try
            {
                this.client = ISClient.ClientServiceIns;
                this.client.MessageRecieved += GetMessageFromClient;
                SendCommandToService(new CommandRecievedEventArgs((int)CommandEnum.LogCommand, null, null));
            } catch(Exception e)
            {
                this.Logs = null;
            }
        }
        #endregion
        #region Read/Write
        public void GetMessageFromClient(object sender, string message)
        {
            //If message if log - handle and notify, else ignore.
            CommandRecievedEventArgs command = CommandRecievedEventArgs.FromJson(message);
            if (command.CommandID == (int)CommandEnum.LogCommand)
            {
                ObservableCollection<MessageRecievedEventArgs> list = new ObservableCollection<MessageRecievedEventArgs>();
                string[] logsStrings = command.Args[0].Split(';');
                foreach(string s in logsStrings)
                {
                    if (s.Contains("Status") && s.Contains("Message"))
                    {
                        try
                        {
                            MessageRecievedEventArgs m = MessageRecievedEventArgs.FromJson(s);
                            list.Add(m);
                        } catch (Exception e)
                        {
                            continue;
                        }
                        
                    }
                }
                Logs = list;
            }
            else if (command.CommandID == (int)CommandEnum.NewLogEntryCommand)
            {
                try
                {
                    MessageRecievedEventArgs m = MessageRecievedEventArgs.FromJson(command.Args[0]);
                    ObservableCollection<MessageRecievedEventArgs> tempList = new ObservableCollection<MessageRecievedEventArgs>(Logs);
                    tempList.Add(m);
                    this.Logs = tempList;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
        public void SendCommandToService(CommandRecievedEventArgs command)
        {
            client.Write(command.ToJson());
        }
        #endregion
        #region Methods

        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
    }
}
