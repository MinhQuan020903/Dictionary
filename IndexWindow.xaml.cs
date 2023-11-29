using Dictionary.Model;
using System.Windows.Navigation;

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
