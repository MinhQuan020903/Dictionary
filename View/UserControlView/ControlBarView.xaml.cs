﻿using Dictionary.ViewModel.UserControlViewModel;
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

namespace Dictionary.View.UserControlView
{
    /// <summary>
    /// Interaction logic for ControlBarView.xaml
    /// </summary>
    public partial class ControlBarView : UserControl
    {
        public ControlBarViewModel ViewModel { get; set; }
        public ControlBarView()
        {
            InitializeComponent();
            this.DataContext = ViewModel = new ControlBarViewModel();
        }
    }
}
