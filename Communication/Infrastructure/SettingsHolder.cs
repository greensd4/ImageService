using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Infrastructure
{
    class SettingsHolder
    {
        public static string IP { get { return "127.0.0.1"; } }
        private static readonly Dictionary<string, int> ports = new Dictionary<string, int>()
        {
            {"mobile", 8500 },
            {"regular", 8000 }
        };
        public static Dictionary<string , int> PortByKey { get { return ports; } }
    }
}
