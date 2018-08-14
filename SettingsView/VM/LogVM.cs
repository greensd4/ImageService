using Communication.Infrastructure;
using SettingsView.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingsView.VM
{
    public class LogVM : INotifyPropertyChanged
    {
        #region Members, Properties , Events

        private ILogModel model;
        public event PropertyChangedEventHandler PropertyChanged;
        public LogVM()
        {
            model = new LogModel();
            model.PropertyChanged += this.NotifyPropertyChanged;
        }
        
        ObservableCollection<MessageRecievedEventArgs> logs;
        public ObservableCollection<MessageRecievedEventArgs> VM_Logs 
        {
            get { return model.Logs; } 
            set { logs = value; }
        }
        #endregion
        #region Methods
        public void NotifyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChangedEventArgs p = new PropertyChangedEventArgs("VM_" + e.PropertyName);
            if (e.PropertyName.Equals("Logs"))
            {
                VM_Logs = model.Logs;
            }
            PropertyChanged?.Invoke(this, p);
        }
        #endregion
    }
}
