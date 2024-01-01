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

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            string? version = fvi.FileVersion;

            currentVersion.Text = version;
        }
    }
}
