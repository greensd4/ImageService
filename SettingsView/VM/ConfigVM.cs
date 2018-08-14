using Communication.Infrastructure;
using SettingsView.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using Prism.Commands;
namespace SettingsView.VM
{
    class ConfigVM : INotifyPropertyChanged
    {
        #region Members, Properties , Events
        private IConfigModel model;
        public event PropertyChangedEventHandler PropertyChanged;
        private string outputDir;
        public string VM_OutputDir
        {
            get { return model.OutputDir; }
            set
            {
                outputDir = value;
            }

        }
        private string sourceName;
        public string VM_SourceName
        {
            get { return model.SourceName; }
            set
            {
                this.sourceName = value;
            }
        }
        private string logName;
        public string VM_LogName
        {
            get { return model.LogName; }
            set
            {
                this.logName = value;
            }
        }
        private int thumbnailSize;
        public int VM_ThumbnailSize
        {
            get { return model.ThumbnailSize; }
            set
            {
                this.thumbnailSize = value;
            }
        }
        private ObservableCollection<string> handlers;
        public ObservableCollection<string> VM_Handlers
        {
            get { return model.Handlers; }
            set
            {
                this.handlers = value;
            }
        }
        #endregion
        #region Remove handler
        public ICommand RemoveButtonCmd { get; set; }
        public bool CanRemove
        {
            get
            {
                return (SelectedHandler != null);
            }
        }
        private string selectedHandler;
        public string SelectedHandler
        {
            get { return selectedHandler; }
            set
            {
                selectedHandler = value;
                var command = this.RemoveButtonCmd as DelegateCommand;
                command.RaiseCanExecuteChanged();
            }
        }
        #endregion
        #region Methods
        public ConfigVM()
        {
            model = new ConfigModel();
            model.PropertyChanged += NotifyPropertyChanged;
            this.RemoveButtonCmd = new DelegateCommand(RemoveHandler, () => { return CanRemove; });
        }

        public void NotifyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string propName = e.PropertyName;
            if (propName.Equals("OutputDir"))
            {
                VM_OutputDir = model.OutputDir;
            }
            if (propName.Equals("ThumbnailSize"))
            {
                VM_ThumbnailSize = model.ThumbnailSize;
            }
            if (propName.Equals("SourceName"))
            {
                VM_SourceName = model.SourceName;
            }
            if (propName.Equals("LogName"))
            {
                VM_LogName = model.LogName;
            }
            if (propName.Equals("Handlers"))
            {
                VM_Handlers = model.Handlers;
            }
            PropertyChangedEventArgs p = new PropertyChangedEventArgs("VM_" + e.PropertyName);
            PropertyChanged?.Invoke(this, p);
        }

        public void RemoveHandler()
        {
            string[] args = new string[1];
            args[0] = SelectedHandler;
            model.SendCommandToService(new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, args, null));
        }
        #endregion
    }
}
