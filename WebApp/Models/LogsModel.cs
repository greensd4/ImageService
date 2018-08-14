using Communication.Client;
using Communication.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Web;

namespace WebApp.Models
{
    public class LogsModel
    {
        #region Properties
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Logs")]
        public ObservableCollection<LogType> Logs { get; set; }
        #endregion
        #region Members & Events
        private static IISClient client;
        public delegate void Refresh();
        public event Refresh NotifyRefresh;
        #endregion
        public LogsModel()
        {
            Logs = new ObservableCollection<LogType>();
            try
            {
                client = ISClient.ClientServiceIns;
                client.MessageRecieved += GetMessageFromClient;
                SendCommandToService(new CommandRecievedEventArgs((int)CommandEnum.LogCommand, null, null));
                Thread.Sleep(1000);
            } catch(Exception e) {}
        }
        public void GetMessageFromClient(object sender, string message)
        {
            //If message if log - handle and notify, else ignore.
            CommandRecievedEventArgs command = CommandRecievedEventArgs.FromJson(message);
            if (command.CommandID == (int)CommandEnum.LogCommand)
            {
                ObservableCollection<LogType> list = new ObservableCollection<LogType>();
                string[] logsStrings = command.Args[0].Split(';');
                foreach (string s in logsStrings)
                {
                    if (s.Contains("Status") && s.Contains("Message"))
                    {
                        try
                        {
                            MessageRecievedEventArgs m = MessageRecievedEventArgs.FromJson(s);
                            list.Add(LogType.LogTypeFromMessageRecieved(m));
                        }
                        catch (Exception e)
                        {
                            continue;
                        }

                    }
                }
                Logs = list;
                NotifyRefresh?.Invoke();
            }
            else if (command.CommandID == (int)CommandEnum.NewLogEntryCommand)
            {
                try
                {
                    MessageRecievedEventArgs m = MessageRecievedEventArgs.FromJson(command.Args[0]);
                    ObservableCollection<LogType> tempList = new ObservableCollection<LogType>(Logs);
                    tempList.Add(LogType.LogTypeFromMessageRecieved(m));
                    this.Logs = tempList;
                    NotifyRefresh?.Invoke();
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
    }
    public class LogType
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Status")]
        public StatusType Status { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Message")]
        public string Message { get; set; }
        public static LogType LogTypeFromMessageRecieved(MessageRecievedEventArgs m)
        {
            return new LogType { Status = (StatusType)((int)m.Status), Message = m.Message };
        }
        public enum StatusType
        {
            INFO,
            WARNING,
            FAIL
        }
    }
}