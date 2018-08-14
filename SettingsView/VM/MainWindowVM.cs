using Communication.Client;
using Prism.Commands;
using SettingsView.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SettingsView.VM
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        #region Members, Properties, Constructor

        public event PropertyChangedEventHandler PropertyChanged;
        private IMainWindowModel model;
        public ICommand WindowCloseCommand { get; set; }
        public bool IsConnected
        {
            get { return model.IsConnected; }
        }

        public MainWindowVM()
        {
            this.model = new MainWindowModel();
            WindowCloseCommand = new DelegateCommand(() => {
                Console.WriteLine("In closeWindow Command");
                this.model.SendCloseCommandToService();
            }
            );

        }
        #endregion

    }

}
