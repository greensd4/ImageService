using Communication.Infrastructure;
using ImageService.Commands;
using ImageService.Logging;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class LogCommand : ICommand
    {
        #region Members, Constructor
        private ILoggingService m_logging;

        /// <summary>
        /// Constructors.
        /// </summary>
        /// <param name="modal">Service Modal</param>
        public LogCommand(ILoggingService logging)
        {
            m_logging = logging;            // Storing the Modal
        }
        #endregion
        #region methods

        public string Execute(string[] args, out bool result)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (MessageRecievedEventArgs item in m_logging.LogList)
                {
                    sb.Append(item.ToJson() + " ; ");
                }
                result = true;
                string[] arguments = new string[1];
                arguments[0] = sb.ToString();
                CommandRecievedEventArgs c = new CommandRecievedEventArgs((int)CommandEnum.LogCommand, arguments, null);
                return c.ToJson();
            }
            catch (Exception e)
            {
                result = false;
                return e.ToString();
            }

        }
        #endregion
    }

}
