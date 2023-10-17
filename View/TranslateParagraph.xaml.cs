﻿using System;
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
using System.Windows.Shapes;

namespace Dictionary.View
{
    /// <summary>
    /// Interaction logic for TranslateParagraph.xaml
    /// </summary>
    public partial class TranslateParagraph : Window
    {
        public TranslateParagraph()
        {
            this.Resources.MergedDictionaries.Add(new ResourceDictionary());
            this.Resources.MergedDictionaries[0].Source = new Uri("Resource/String.xaml", UriKind.Relative);

            InitializeComponent();
        }
    }
}