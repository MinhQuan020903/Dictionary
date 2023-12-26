using NetSparkleUpdater;
using NetSparkleUpdater.Enums;
using NetSparkleUpdater.SignatureVerifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Dictionary.ViewModel.UserControlViewModel
{
    public class ControlBarViewModel
    {

        private SparkleUpdater _sparkle;

        #region commands

        public ICommand CloseWindowCommand { get; set; }
        public ICommand MinimizeWindowCommand { get; set; }
        public ICommand MaximizeWindowCommand { get; set; }
        public ICommand MouseMoveWindowCommand { get; set; }
        public ICommand GetUpdateCommand { get; set; }
        #endregion

        public ControlBarViewModel()
        {
            string publicKey = "uyhHEMP+TENQzvw/uRbLfdxnvZoGLLRvhqmEeCvlnRc=";
            // get public and secret key in README, just in development, hard code in release
            _sparkle = new SparkleUpdater(
    "https://raw.githubusercontent.com/MinhQuan020903/Dictionary/release/Update/appcast.xml", // link to your app cast file
        new Ed25519Checker(SecurityMode.Unsafe, publicKey) // your base 64 public key -- generate this with the NetSparkleUpdater.Tools.AppCastGenerator .NET CLI tool on any OS
)
            {
                UIFactory = new NetSparkleUpdater.UI.WPF.UIFactory(), // or null or choose some other UI factory or build your own!
                RelaunchAfterUpdate = true, // default is false; set to true if you want your app to restart after updating (keep as false if your installer will start your app for you)
                CustomInstallerArguments = "", // set if you want your installer to get some command-line args
                ShowsUIOnMainThread = true, // required on Avalonia, preferred on WPF/WinForms
            };
            _sparkle.StartLoop(true, true); // `true` to run an initial check online -- only call StartLoop once for a given SparkleUpdater instance!

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

            GetUpdateCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, async (p) =>
            {
               await _sparkle.CheckForUpdatesAtUserRequest();
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
