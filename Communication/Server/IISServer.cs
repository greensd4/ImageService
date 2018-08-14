using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Server
{
    public interface IISServer
    {
        #region Properties
        IISClientHandler ClientHandler { get; set; }
        string IP { get; set; }
        int Port { get; set; }
        TcpListener Listener { get; set; }
        #endregion
        #region Methods
        void Start();
        void Stop();
        #endregion
    }
}
