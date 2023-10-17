using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Dictionary.ViewModel.UserControlViewModel
{
    public class ControlBarViewModel : BaseViewModel
    {
        #region commands

        public ICommand CloseWindowCommand { get; set; }
        public ICommand MinimizeWindowCommand { get; set; }
        public ICommand MaximizeWindowCommand { get; set; }
        public ICommand MouseMoveWindowCommand { get; set; }
        #endregion

        public ControlBarViewModel()
        {
            CloseWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) => { GetParentWindow(p).Close(); });
            MinimizeWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) =>
            {
                if (GetParentWindow(p) != null)
                {
                    if (GetParentWindow(p).WindowState != System.Windows.WindowState.Minimized)
                    {
                        GetParentWindow(p).WindowState = System.Windows.WindowState.Minimized;
                    }
                    else
                    {
                        GetParentWindow(p).WindowState = System.Windows.WindowState.Normal;
                    }
                }

            });
            MaximizeWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) =>
            {
                if (GetParentWindow(p) != null)
                {
                    if (GetParentWindow(p).WindowState != System.Windows.WindowState.Maximized)
                    {
                        GetParentWindow(p).WindowState = System.Windows.WindowState.Maximized;
                    }
                    else
                    {
                        GetParentWindow(p).WindowState = System.Windows.WindowState.Normal;
                    }
                }
            });

            MouseMoveWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) =>
            {
                if (GetParentWindow(p) != null)
                {
                    GetParentWindow(p).DragMove();
                }
            }); 
        }
        private Window GetParentWindow(UserControl control)
        {
            // Use the Window.GetWindow method to find the parent window of the UserControl.
            return Window.GetWindow(control);
        }
    }
}
