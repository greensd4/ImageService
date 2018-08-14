using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Infrastructure
{
    public class MessageRecievedEventArgs : EventArgs
    {

        #region Properties 
        public MessageTypeEnum Status { get; set; }
        public string Message { get; set; }
        #endregion
        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="MessageType"></param>
        /// <param name="message"></param>
        public MessageRecievedEventArgs(int MessageType, string message)
        {
            Status = (MessageTypeEnum)MessageType;
            Message = message;
        }
        /// <summary>
        /// Converts object to json string
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            //One string with no new lines.
            return JsonConvert.SerializeObject(this).Replace(Environment.NewLine, " ");
        }
        /// <summary>
        /// Converts json string to object
        /// </summary>
        /// <param name="jStr"></param>
        /// <returns></returns>
        public static MessageRecievedEventArgs FromJson(string jStr)
        {
            try
            {
                JObject jObject = (JObject)JsonConvert.DeserializeObject(jStr);
                int messageType = (int)jObject["Status"];
                string message = (string)jObject["Message"];
                return new MessageRecievedEventArgs(messageType, message);
            }catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
    }
}
