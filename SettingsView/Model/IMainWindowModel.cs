using Communication.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingsView.Model
{
    public interface IMainWindowModel
    {
        #region Methods and Events
        event PropertyChangedEventHandler PropertyChanged;
        void SendCloseCommandToService();
        void NotifyPropertyChanged(string propName);
        #endregion
        #region Properties
        bool IsConnected { get; }
        #endregion
    }
}
