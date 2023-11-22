using System;
using System.Windows;
using System.Windows.Controls;

namespace Dictionary.View
{
    /// <summary>
    /// Interaction logic for MainPageView.xaml
    /// </summary>
    public partial class MainPageView : Page
    {
        public MainPageView()
        {

            // Merge the String.xaml resource dictionary into application resources
            this.Resources.MergedDictionaries.Add(new ResourceDictionary());
            this.Resources.MergedDictionaries[0].Source = new Uri("Resource/String.xaml", UriKind.Relative);

            // Set environment variables here
            Environment.SetEnvironmentVariable("SPEECH_KEY", Application.Current.Resources["AzureTextToSpeechKey"].ToString());
            Environment.SetEnvironmentVariable("SPEECH_REGION", Application.Current.Resources["AzureTextToSpeechRegion"].ToString());

            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
