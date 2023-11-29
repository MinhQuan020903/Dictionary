using Dictionary.ViewModel.UserControlViewModel;
using System.Windows.Controls;

namespace Dictionary.View.UserControlView
{
    /// <summary>
    /// Interaction logic for SideBarView.xaml
    /// </summary>
    public partial class SideBarView : UserControl
    {
        public SidelBarViewModel ViewModel { get; set; }
        public SideBarView()
        {
            InitializeComponent();
            this.DataContext = ViewModel = new SidelBarViewModel();
        }
    }
}
