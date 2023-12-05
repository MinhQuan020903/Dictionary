using Dictionary.Model;
using System;
using System.Windows;
using System.Windows.Navigation;

namespace Dictionary
{
    /// <summary>
    /// Interaction logic for IndexWindow.xaml
    /// </summary>
    public partial class IndexWindow : Window
    {
        public IndexWindow()
        {
            InitializeComponent();
            IndexFrame.Navigate(new Uri("/View/MainPageView.xaml", UriKind.Relative));
        }
    }
}
