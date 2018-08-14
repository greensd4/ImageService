using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Server
{
    public interface IISClientHandler
    {
        /// <summary>
        /// Handle client
        /// </summary>
        /// <param name="client"></param>
        void HandleClient(TcpClient client, int flag);

    }
}
