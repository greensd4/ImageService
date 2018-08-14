using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    public class SettingsObject
    {
        #region Constructor, Singelton method
        private static SettingsObject instance = null;
        private SettingsObject()
        {
            OutPutDir = ConfigurationManager.AppSettings["OutPutDir"];
            SourceName = ConfigurationManager.AppSettings["SourceName"];
            LogName = ConfigurationManager.AppSettings["LogName"];
            ThumbnailSize = int.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]);
            Handlers = ConfigurationManager.AppSettings["Handler"];
            Students = ConfigurationManager.AppSettings["Students"];
            ServiceName = ConfigurationManager.AppSettings["ServiceName"];
            
        }
        public static SettingsObject GetInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SettingsObject();
                }
                return instance;
            }
        }
        #endregion
        #region Members, Properties
        public string ServiceName { get; set; }
        public string Students { get; set; }
        private string outputDir;
        public string OutPutDir
        {
            get { return outputDir; }
            set { outputDir = value; }
        }
        private string sourceName;
        public string SourceName
        {
            get { return sourceName; }
            set { sourceName = value; }
        }
        private string logName;
        public string LogName
        {
            get { return logName; }
            set { logName = value; }
        }
        private int thumbnailSize;
        public int ThumbnailSize
        {
            get { return thumbnailSize; }
            set { thumbnailSize = value; }
        }
        private string handlers;
        public string Handlers
        {
            get { return handlers; }
            set
            {
                handlers = value;
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Converts this object to json string
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            JObject j = new JObject();
            j["OutputDir"] = OutPutDir;
            j["SourceName"] = SourceName;
            j["ThumbnailSize"] = ThumbnailSize.ToString();
            j["LogName"] = LogName;
            j["Handler"] = Handlers;
            j["Students"] = Students;
            return j.ToString();
        }
        Mutex mutex = new Mutex();
        /// <summary>
        /// Removes handler from list
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool RemoveHandler(string path)
        {
            mutex.WaitOne();
            bool result = false;
            string[] s = Handlers.Split(';');
            List<string> hand = new List<string>(s);
            if (hand.Contains(path))
            {
                hand.Remove(path);
                Handlers = string.Join(";", hand.ToArray());
                result =  true;
            }
            mutex.ReleaseMutex();
            return result;
        }
        #endregion

    }
}
