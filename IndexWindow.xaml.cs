using Dictionary.Model;
using System;
using System.Windows;
using System.Windows.Navigation;
using NetSparkleUpdater.SignatureVerifiers;
using NetSparkleUpdater.Enums;
using NetSparkleUpdater;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.IO;

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

            // get public and secret key in README, just in development, hard code in release
            var publicKey = "uyhHEMP+TENQzvw/uRbLfdxnvZoGLLRvhqmEeCvlnRc=";
            var _sparkle = new SparkleUpdater(
    "https://github.com/MinhQuan020903/Dictionary/tree/release/Update/appcast.xml", // link to your app cast file
    new Ed25519Checker(SecurityMode.Strict, publicKey) // your base 64 public key -- generate this with the NetSparkleUpdater.Tools.AppCastGenerator .NET CLI tool on any OS
)
            {
                UIFactory = new NetSparkleUpdater.UI.WPF.UIFactory(), // or null or choose some other UI factory or build your own!
                RelaunchAfterUpdate = false, // default is false; set to true if you want your app to restart after updating (keep as false if your installer will start your app for you)
                CustomInstallerArguments = "", // set if you want your installer to get some command-line args
                ShowsUIOnMainThread = true, // required on Avalonia, preferred on WPF/WinForms
            };
            _sparkle.StartLoop(true); // `true` to run an initial check online -- only call StartLoop once for a given SparkleUpdater instance!
            _sparkle.CheckForUpdatesQuietly();
        }
    }
}
