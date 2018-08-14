
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Communication.Infrastructure;

namespace ImageService.Logging
{
    public class LoggingService : ILoggingService
    {
        #region Members, Properties, Events
        private List<MessageRecievedEventArgs> logList;
        public List<MessageRecievedEventArgs> LogList
        {
            get { return logList; }
        }
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        public LoggingService()
        {
            logList = new List<MessageRecievedEventArgs>();
        }
        #endregion
        #region Methods

        public void Log(string message, MessageTypeEnum type)
        {

            if (message == null)
                return;
            MessageRecievedEventArgs eventArgs = new MessageRecievedEventArgs((int)type,message);
            logList.Add(eventArgs);
            MessageRecieved?.Invoke(this, eventArgs);
        }
        #endregion
    }
}
