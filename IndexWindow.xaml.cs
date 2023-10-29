using Dictionary.View;
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
using Dictionary.Model;

namespace Dictionary
{
    /// <summary>
    /// Interaction logic for IndexWindow.xaml
    /// </summary>
    public partial class IndexWindow : NavigationWindow
    {
        public IndexWindow()
        {
            InitializeComponent();
            NavigationServiceModel.SetNavigationService(this.NavigationService);
        }
    }
}
