using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Infrastructure
{
    public class PhotoDetails
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string ThumbnailPath { get; set; }
        public DateTime Date { get; set; }
        public string ToJson()
        {
            JObject jStr = new JObject();
            jStr["Path"] = Path;
            jStr["ThumbnailPath"] = ThumbnailPath;
            jStr["DateTime"] = Date.ToString();
            jStr["Name"] = Name;
           
            return jStr.ToString().Replace(Environment.NewLine, " ");
        }
        public static PhotoDetails FromJson(string json)
        {
            try
            {
                JObject jObject = (JObject)JsonConvert.DeserializeObject(json);
                string path = (string)jObject["Path"];
                string name = (string)jObject["Name"];
                string tpath = (string)jObject["ThumbnailPath"];
                DateTime d;
                string date = (string)jObject["DateTime"];
                DateTime.TryParse(date, out d);
                return new PhotoDetails {Path= path,Name = name,ThumbnailPath = tpath, Date = d };
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
