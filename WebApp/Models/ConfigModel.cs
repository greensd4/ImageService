using Communication.Client;
using Communication.Infrastructure;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class ConfigModel
    {
        #region Properties
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "OutputDir")]
        public string OutputDir { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "SourceName")]
        public string SourceName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "LogName")]
        public string LogName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ThumbnailSize")]
        public int ThumbnailSize { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Handlers")]
        public ObservableCollection<string> Handlers { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "DeleteHandler")]
        public string DeleteHandler { get; set; }
        public string StudentsString { get; set; }
        public static ConfigModel GetInstance
        {
            get
            {
                if(instance == null)
                {
                    instance = new ConfigModel();
                }
                return instance;
            }
        }
        #endregion
        #region Members & Events
        private static IISClient client;
        private static ConfigModel instance;
        public delegate void Refresh();
        public event Refresh NotifyRefresh;
        #endregion
        private ConfigModel()
        {
            DefaultValues();
            try
            {
                
                client = ISClient.ClientServiceIns;
                client.MessageRecieved += GetMessageFromClient;
                Console.WriteLine("Sending Command");
                SendCommandToService(new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, null, null));

            } catch(Exception e) 
            {
            }
        }
        public void GetMessageFromClient(object sender, string crea)
        {
            Debug.WriteLine("recieved" + crea);
            CommandRecievedEventArgs command = CommandRecievedEventArgs.FromJson(crea);
            if (command.CommandID == (int)CommandEnum.GetConfigCommand)
            {
                string m = command.Args[0];
                JObject json = JObject.Parse(m);
                OutputDir = (string)json["OutputDir"];
                SourceName = (string)json["SourceName"];
                ThumbnailSize = int.Parse((string)json["ThumbnailSize"]);
                LogName = (string)json["LogName"];
                StudentsString = (string)json["Students"];
                UpdateHandlersFromString((string)json["Handler"]);
                NotifyRefresh?.Invoke();
            }
            else if (command.CommandID == (int)CommandEnum.CloseCommand)
            {
                UpdateHandlersFromString(command.Args[0]);
                NotifyRefresh?.Invoke();
            }
           
        }
        public void SendCommandToService(CommandRecievedEventArgs command)
        {
            client.Write(command.ToJson());
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
        public void DefaultValues()
        {
            //Default Values.
            OutputDir = "Not connected to server!";
            SourceName = null;
            ThumbnailSize = 0;
            LogName = null;
            Handlers = new ObservableCollection<string>();
        }
    }


}