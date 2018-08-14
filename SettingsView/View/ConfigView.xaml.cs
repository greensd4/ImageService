using SettingsView.VM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SettingsView.View
{
    /// <summary>
    /// Interaction logic for ConfigView.xaml
    /// </summary>
    public partial class ConfigView : UserControl
    {
        #region Members, Constructor
        private ConfigVM vm;
        public ConfigView()
        {
            InitializeComponent();
            this.vm = new ConfigVM();
            this.DataContext = vm;
        }
        #endregion
    }
}
