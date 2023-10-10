using System;
using System.Windows;

namespace Dictionary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // Merge the String.xaml resource dictionary into application resources
            this.Resources.MergedDictionaries.Add(new ResourceDictionary());
            this.Resources.MergedDictionaries[0].Source = new Uri("Resource/String.xaml", UriKind.Relative);

            // Set environment variables here
            Environment.SetEnvironmentVariable("SPEECH_KEY", Application.Current.Resources["AzureTextToSpeechKey"].ToString());
            Environment.SetEnvironmentVariable("SPEECH_REGION", Application.Current.Resources["AzureTextToSpeechRegion"].ToString());

            InitializeComponent();
        }
    }
}
