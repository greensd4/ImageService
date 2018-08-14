﻿using SettingsView.VM;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for LogView.xaml
    /// </summary>
    public partial class LogView : UserControl
    {
        #region Members, Constructor
        private LogVM vm;
        public LogView()
        {
            InitializeComponent();
            this.vm = new LogVM();
            this.DataContext = vm;
        }
        #endregion

    }
}
