using Dictionary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Unsplasharp;
using static System.Net.Mime.MediaTypeNames;

namespace Dictionary.ViewModel
{
    public class MainViewModel : BaseViewModel

    {

        //Data binding for text box and image
        private string _text;
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        }
        private string _image;
        public string Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged(nameof(Image));
            }
        }
        public ICommand ButtonCommand { get; set; }
        public ICommand ButtonAudioCommand { get; set;}

        public MainViewModel()
        {
            /*//Set environment variable for Azure Speech API
            Environment.SetEnvironmentVariable("SPEECH_KEY", App.Current.Resources["AzureTextToSpeechKey"].ToString());
            Environment.SetEnvironmentVariable("SPEECH_REGION", "southeastasia");*/

            //Init button command
            ButtonCommand = new RelayCommand<object>(ButtonCommandCanExecute, ButtonCommandExecute);
            ButtonAudioCommand = new RelayCommand<object>(ButtonCommandAudioCanExecute, ButtonCommandAudioExecute);

        }

        private bool ButtonCommandCanExecute(object obj)
        {
            return true;
        }

        private async void ButtonCommandExecute(object obj)
        {
            //Check if text box is empty
            if (Text == null || Text == "")
            {
                MessageBox.Show("Please enter a word");
            } else
            {
                string unsplashApiKey = App.Current.Resources["UnsplashApiKey"].ToString();
                //Connect to Unsplash API with API credential
                Image = await TextToImageAPI.GetImageFromText(Text);
            }
        }

        private bool ButtonCommandAudioCanExecute(object obj)
        {
            return true;
        }

        private async void ButtonCommandAudioExecute(object obj)
        {
            Console.Write(Text);
            await TextToSpeechAPI.TextToSpeech(Text);
        }
    }
}
