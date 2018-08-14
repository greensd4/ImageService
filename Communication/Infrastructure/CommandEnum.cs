using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Infrastructure
{
    public enum CommandEnum : int
    {
        NewFileCommand,
        GetConfigCommand,
        LogCommand,
        CloseCommand,
        NewLogEntryCommand,
        WindowClosedCommand,
        RemovePhoto,
    }
}
