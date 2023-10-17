using Dictionary.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Nodes;
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
        private string _translatedText;
        public string TranslatedText
        {
            get => _translatedText;
            set
            {
                _translatedText = value;
                OnPropertyChanged(nameof(TranslatedText));
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
        public ICommand LostFocusCommand { get; set; }
        public ICommand ButtonCommand { get; set; }
        public ICommand ButtonAudioCommand { get; set; }
        public ICommand ButtonTranslatorCommand { get; set; }
        public ICommand LoadedWindowCommand { get; set; }

        public MainViewModel()
        {
            /*//Set environment variable for Azure Speech API
            Environment.SetEnvironmentVariable("SPEECH_KEY", App.Current.Resources["AzureTextToSpeechKey"].ToString());
            Environment.SetEnvironmentVariable("SPEECH_REGION", "southeastasia");*/

            //Command when loading main windows
            LoadedWindowCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
            LostFocusCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Keyboard.ClearFocus();
            });
            ButtonCommand = new RelayCommand<object>(ButtonCommandCanExecute, ButtonCommandExecute);
            ButtonAudioCommand = new RelayCommand<object>(ButtonCommandAudioCanExecute, ButtonCommandAudioExecute);
            ButtonTranslatorCommand = new RelayCommand<object>(ButtonCommandTranslatorCanExecute, ButtonCommandTranslatorExecute);


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
            }
            else
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

        private bool ButtonCommandTranslatorCanExecute(object obj)
        {
            return true;
        }
        private async void ButtonCommandTranslatorExecute(object obj)
        {
            //Check if text box is empty
            if (Text == null || Text == "")
            {
                MessageBox.Show("Please enter a word");
            }
            else
            {
                //Translate text
                string translatedText = await TranslateAPI.Translate(Text);
                // Parse the JSON array
                JArray jsonArray = JArray.Parse(translatedText);
                // Check if the array contains any items
                if (jsonArray.Count > 0)
                {
                    // Access the first item in the array
                    JObject firstObject = jsonArray[0] as JObject;

                    if (firstObject != null)
                    {
                        // Use SelectToken to get the "text" property within the first object
                        JToken textToken = firstObject.SelectToken("translations[0].text");

                        // Check if the property exists and get its value
                        if (textToken != null)
                        {
                            TranslatedText = textToken.ToString();
                            // 'translatedText' now contains the value of the "text" property
                        }
                    }
                }
            }
        }
    }
}
