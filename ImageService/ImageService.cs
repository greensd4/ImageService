using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using ImageService.Controller;
using ImageService.Modal;
using ImageService.Logging;
using ImageService.Server;
using System.Configuration;

using System.IO;
using Communication.Server;
using Communication.Infrastructure;

namespace ImageService
{
    /// <summary>
    /// partial class for ImageService, derived from ServiceBase.
    /// </summary>
    public partial class ImageService : ServiceBase
    {
        private int eventId = 1;
        private IImageController imageController;
        private ImageServer imageServer;
        private IISServer server;
        private ILoggingService loggingService;
        private IImageServiceModal imageModal;
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="args"> arguments for the service</param>
        public ImageService(string[] args)
        {
            InitializeComponent();
            this.loggingService = new LoggingService();
            this.loggingService.Log("Logger created", MessageTypeEnum.INFO);
            this.loggingService.Log("Test background info", MessageTypeEnum.INFO);
            this.loggingService.Log("Test background fail", MessageTypeEnum.FAIL);
            this.loggingService.Log("Test background warning", MessageTypeEnum.WARNING);
            this.imageModal = new ImageServiceModal(loggingService)
            {
                OutputFolder = ConfigurationManager.AppSettings["OutputDir"],
                ThumbnailSize = int.Parse(ConfigurationManager.AppSettings["ThumbnailSize"])

            };
            this.imageController = new ImageController(this.imageModal, this.loggingService);
            this.imageServer = new ImageServer(loggingService, imageController);
            this.server = new ISMobileServer(imageServer);
            //this.guiServer = new GuiServer(8000, imageServer);
            string eventSourceName = ConfigurationManager.AppSettings["SourceName"];
            string logName = ConfigurationManager.AppSettings["LogName"];
            if (args.Count() > 0)
            {
                eventSourceName = args[0];
            }
            if (args.Count() > 1)
            {
                logName = args[1];
            }

            ImageServiceLog = new EventLog();
            if (!EventLog.SourceExists(eventSourceName))
            {
                EventLog.CreateEventSource(eventSourceName, logName);
            }
            ImageServiceLog.Source = eventSourceName;
            ImageServiceLog.Log = logName;
            
            this.loggingService.MessageRecieved += this.WriteEntryToLog;
        }
        /// <summary>
        /// OnStart - when the service starts running.
        /// </summary>
        /// <param name="args"> arguments for the service </param>
        protected override void OnStart(string[] args)
        {

            this.loggingService.Log("In OnStart", MessageTypeEnum.INFO);
            this.server.Start();
            //this.guiServer.Start();
            // Set up a timer to trigger every minute.  
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 6000000; // 10 mins  
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();
            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }
        /// <summary>
        /// OnStop - when the user decide to stop the service.
        /// </summary>
        protected override void OnStop()
        {
            this.loggingService.Log("In OnStop", MessageTypeEnum.INFO);
            //Closing the server.
            this.imageServer.OnClose();
            this.server.Stop();
            //this.guiServer.Stop();
            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }
        /// <summary>
        /// OnTimer
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="args">elapsed event args</param>
        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here. 
            ImageServiceLog.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
        }
        /// <summary>
        /// ServiceState enums.
        /// </summary>
        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public int dwServiceType;
            public ServiceState dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
        };
        /// <summary>
        /// Gets a message from the Log and translates it to the eventLog.
        /// </summary>
        /// <param name="sender"> sender</param>
        /// <param name="e">eventArgs</param>
        public void WriteEntryToLog(object sender , MessageRecievedEventArgs e)
        {
            string message = sender.ToString() +": " + e.Message;
            EventLogEntryType type;
            switch (e.Status)
            {
                case MessageTypeEnum.WARNING:
                    {
                        type = EventLogEntryType.Warning;
                        break;
                    }
                case MessageTypeEnum.FAIL:
                    {
                        type = EventLogEntryType.Error;
                        break;
                    }
                default:
                    {
                        type = EventLogEntryType.Information;
                        break;
                    }
            }
            this.ImageServiceLog.WriteEntry(message, type);
        }
    }
}