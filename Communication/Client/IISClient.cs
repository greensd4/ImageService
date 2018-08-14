using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Client
{
    public interface IISClient
    {
        #region Properties and Events
        event EventHandler<string> MessageRecieved;
        bool Connection { get; }
        #endregion
        #region Methods
        void Write(string command);
        void Read(); // blocking call
        void Disconnect();
        #endregion
    }
}
