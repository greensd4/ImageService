using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Infrastructure
{
    public class CommandRecievedEventArgs : EventArgs
    {

        #region Properties 
        public int CommandID { get; set; }      // The Command ID
        public string[] Args { get; set; }
        public string RequestDirPath { get; set; }  // The Request Directory
        #endregion
        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="args"></param>
        /// <param name="path"></param>
        public CommandRecievedEventArgs(int id, string[] args, string path)
        {
            CommandID = id;
            Args = args;
            RequestDirPath = path;
        }
        /// <summary>
        /// Converts object to json string
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            JObject jStr = new JObject();
            jStr["CommandID"] = CommandID;
            jStr["RequestDirPath"] = RequestDirPath;
            jStr["Args"] = new JArray(Args);
            return jStr.ToString().Replace(Environment.NewLine, " ");
        }
        /// <summary>
        /// Converts json string to object
        /// </summary>
        /// <param name="jStr"></param>
        /// <returns></returns>
        public static CommandRecievedEventArgs FromJson(string jStr)
        {
            try
            {
                JObject jObject = JObject.Parse(jStr);
                int id = (int)jObject["CommandID"];
                JArray args = (JArray)jObject["Args"];
                string[] argsArr = args.Select(c => (string)c).ToArray();
                string path = (string)jObject["RequestDirPath"];
                return new CommandRecievedEventArgs(id, argsArr, path);
            } catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

    }
}
