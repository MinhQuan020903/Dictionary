using Dictionary.Model;
using Dictionary.Model.API;
using Dictionary.Model.JSON;
using Dictionary.Model.Word;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private Word _translatedWord;
        public Word TranslatedWord
        {
            get => _translatedWord;
            set
            {
                _translatedWord = value;
                OnPropertyChanged(nameof(TranslatedWord));
            }
        }
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

        private string _partOfSpeech;
        public string PartOfSpeech
        {
            get => _partOfSpeech;
            set
            {
                _partOfSpeech = value;
                OnPropertyChanged(nameof(PartOfSpeech));
            }
        }

        private WordListView _wordListView;

        public WordListView WordListView
        {
            get => _wordListView;
            set
            {
                _wordListView = value;
                OnPropertyChanged(nameof(WordListView));
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


        //Data binding for visibility of translated grid
        private Visibility _isTranslatedGridVisible = Visibility.Hidden;
        private Visibility _isLoading = Visibility.Hidden;
        public Visibility IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public Visibility IsTranslatedGridVisible
        {
            get => _isTranslatedGridVisible;
            set
            {
                _isTranslatedGridVisible = value;
                OnPropertyChanged(nameof(IsTranslatedGridVisible));
            }
        }
        public ICommand LostFocusCommand { get; set; }
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
            //Command when text box lost focus
            LostFocusCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Keyboard.ClearFocus();
            });
            ButtonAudioCommand = new RelayCommand<object>(ButtonCommandAudioCanExecute, ButtonCommandAudioExecute);
            ButtonTranslatorCommand = new RelayCommand<object>(ButtonCommandTranslatorCanExecute, ButtonCommandTranslatorExecute);


        }


        private bool ButtonCommandAudioCanExecute(object obj)
        {
            return true;
        }

        private async void ButtonCommandAudioExecute(object obj)
        {
            Console.Write(Text);
            await TextToSpeechAPI.TextToSpeech(TranslatedText, "vi", "en");
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
                IsLoading = Visibility.Visible;
                TranslatedWord = new Word();
                //Translate text
                try
                {
                    await TranslateInput("vi", "en");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                //Get synonyms and parts of speech of word
                try
                {
                    await DictionaryLookupInput("vi", "en");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                //Get example of translation
                try
                {
                    await GetAllSynonymExamples("vi", "en");

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                //Binding data to ListView
                WordListView = new WordListView();
                WordListView.PopulateWordListView(TranslatedWord.GetExamplesOfSynonym());
                //Get example image from text
                try
                {
                    string unsplashApiKey = App.Current.Resources["UnsplashApiKey"].ToString();
                    //Connect to Unsplash API with API credential
                    Image = await TextToImageAPI.GetImageFromText(TranslatedText);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                //Set loading to hidden
                IsTranslatedGridVisible = Visibility.Visible;
                IsLoading = Visibility.Hidden;
            }
        }
        private async Task TranslateInput(string from, string to)
        {
            //Translate text
            try
            {
                ApiResponse<string> translated = await TranslateAPI.Translate(Text, from, to);

                if (translated.IsSuccess)
                {
                    // Parse the JSON array
                    JArray jsonArray = JArray.Parse(translated.Data);
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
                                //Check if the translated text is the same as the original text
                                //If it is, show error message (because the API cannot translate the text)
                                if (string.Equals(textToken.ToString(), Text, StringComparison.OrdinalIgnoreCase))
                                {
                                    MessageBox.Show("Vui lòng nhập lại từ.");
                                }
                                else
                                {
                                    // 'translatedText' now contains the value of the "text" property
                                    TranslatedWord.SetTranslatedWord(textToken.ToString());
                                    TranslatedText = textToken.ToString();
                                }

                            }
                        }

                    }
                    else
                    {
                        MessageBox.Show("Vui lòng nhập lại từ.");
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task DictionaryLookupInput(string from, string to)
        {
            //Translate text
            try
            {
                ApiResponse<DictionaryLookup> lookUp = await DictionaryLookupAPI.LookUp(Text, from, to);
                if (lookUp.IsSuccess)
                {
                    //Add data to TranslatedWord
                    TranslatedWord.SetSynonyms(lookUp.Data.translations);
                    TranslatedWord.SetPartsOfSpeech(lookUp.Data.translations.Select(x => x.posTag).ToList());
                    //Get part of speech
                    PartOfSpeech = lookUp.Data.getALlPartOfSpeech();
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập lại từ.");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task GetAllSynonymExamples(string from, string to)
        {
            foreach (WordSynonym synonym in TranslatedWord.GetSynonyms())
            {
                await DictionaryExampleInput(synonym.GetSynonym(), "vi", "en");
            }
        }

        private async Task DictionaryExampleInput(string translation, string from, string to)
        {
            //Get example of translation
            try
            {
                ApiResponse<DictionaryExample> lookUp = await DictionaryExampleAPI.GetExample(Text, translation, from, to);
                if (lookUp.IsSuccess)
                {
                    TranslatedWord.SetExamplesOfSynonym(lookUp.Data.examples);
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập lại từ.");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
