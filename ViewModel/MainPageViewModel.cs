﻿using Dictionary.Model;
using Dictionary.Model.API;

using Dictionary.Model.JSON;
using Dictionary.Model.Word;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Dictionary.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        private ILoggerFactory loggerFactory;
        private ILogger<MainPageViewModel> logger;

        private ObservableCollection<SavedWord> _savedWords;

        public ObservableCollection<SavedWord> SavedWords
        {
            get => _savedWords;
            set
            {
                _savedWords = value;
                OnPropertyChanged(nameof(SavedWords));
            }
        }
        private int maxSavedWords = 8;


        //List of languages
        private List<LanguageObject> _langList;

        public List<LanguageObject> LangList
        {
            get => _langList;
            set => _langList = value;
        }

        private string _sourceLang = "vi";
        public string SourceLang
        {
            get { return _sourceLang; }
            set
            {
                _sourceLang = value;
                OnPropertyChanged(nameof(SourceLang));
            }
        }

        private string _translateLang = "en";
        public string TranslateLang
        {
            get { return _translateLang; }
            set
            {
                _translateLang = value;
                OnPropertyChanged(nameof(TranslateLang));
            }
        }

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
        private Boolean _isSuccess = false;
        public Boolean IsSuccess
        {
            get => _isSuccess;
            set
            {
                _isSuccess = value;
                OnPropertyChanged(nameof(IsSuccess));
            }
        }
        public ICommand LostFocusCommand { get; set; }
        public ICommand ButtonAudioCommand { get; set; }
        public ICommand ButtonTranslatorCommand { get; set; }
        public ICommand LoadedWindowCommand { get; set; }
        public ICommand SavedWordButtonCommand { get; set; }

        public MainPageViewModel()
        {
            /*//Set environment variable for Azure Speech API
            Environment.SetEnvironmentVariable("SPEECH_KEY", App.Current.Resources["AzureTextToSpeechKey"].ToString());
            Environment.SetEnvironmentVariable("SPEECH_REGION", "southeastasia");*/

            //Load saved words from Log file
            SavedWords = SaveFile.LoadTranslatedItemsFromFile();

            string saveWordJson = JsonConvert.SerializeObject(SavedWords);



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
            SavedWordButtonCommand = new RelayCommand<SavedWord>(SavedWordButtonCanExecute, SavedWordButtonExecute);


            string baseDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));
            //Create logger object
            loggerFactory = LoggerFactory.Create(builder =>
            {
                LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
                .WriteTo.File(baseDirectory + "\\Log\\Error\\Log.txt", rollingInterval: RollingInterval.Day);

                builder.AddSerilog(loggerConfiguration.CreateLogger());
            });
            logger = loggerFactory.CreateLogger<MainPageViewModel>();


            LangList = new List<LanguageObject>();
            LangList.Add(new LanguageObject("Tiếng Việt", "vi"));
            LangList.Add(new LanguageObject("Tiếng Anh", "en"));
        }
        public MainPageViewModel(string randomWord)
        {
            /*//Set environment variable for Azure Speech API
            Environment.SetEnvironmentVariable("SPEECH_KEY", App.Current.Resources["AzureTextToSpeechKey"].ToString());
            Environment.SetEnvironmentVariable("SPEECH_REGION", "southeastasia");*/
            Text = randomWord;

            //Load saved words from Log file
            SavedWords = new ObservableCollection<SavedWord>();

            SavedWords = SaveFile.LoadTranslatedItemsFromFile();
            string saveWordJson = JsonConvert.SerializeObject(SavedWords);


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
            SavedWordButtonCommand = new RelayCommand<SavedWord>(SavedWordButtonCanExecute, SavedWordButtonExecute);

            //Create logger object
            string baseDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));
            //Create logger object
            loggerFactory = LoggerFactory.Create(builder =>
            {
                LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
                .WriteTo.File(baseDirectory + "\\Log\\Error\\Log.txt", rollingInterval: RollingInterval.Day)
                ;

                builder.AddSerilog(loggerConfiguration.CreateLogger());
            });
            logger = loggerFactory.CreateLogger<MainPageViewModel>();

            LangList = new List<LanguageObject>();
            LangList.Add(new LanguageObject("Tiếng Việt", "vi"));
            LangList.Add(new LanguageObject("Tiếng Anh", "en"));
            //Set language for random word (default is Vietnamese)
            SourceLang = "en";
            TranslateLang = "vi";
            //Call for translation
            ButtonCommandTranslatorExecute(null);
        }


        private void SavedWordButtonExecute(SavedWord selectedSavedWord)
        {

            // Set properties in the view model based on the selected SavedWord
            Text = selectedSavedWord.Text;
            TranslatedText = selectedSavedWord.TranslatedText;
            PartOfSpeech = selectedSavedWord.PartOfSpeech;
            Image = selectedSavedWord.Image;
            WordListView = selectedSavedWord.WordListView;

            // Update SourceLang and TranslateLang in the view model
            SourceLang = selectedSavedWord.SourceLang;
            TranslateLang = selectedSavedWord.TranslateLang;
            IsTranslatedGridVisible = Visibility.Visible;
        }


        private bool SavedWordButtonCanExecute(SavedWord selectedSavedWord)
        {
            // Implement logic to determine if the command can be executed
            return selectedSavedWord != null;
        }


        private bool ButtonCommandAudioCanExecute(object obj)
        {
            return true;
        }

        private async void ButtonCommandAudioExecute(object obj)
        {
            Console.Write(Text);
            await TextToSpeechAPI.TextToSpeech(TranslatedText, SourceLang, TranslateLang);
        }

        private bool ButtonCommandTranslatorCanExecute(object obj)
        {
            return true;
        }
        private async void ButtonCommandTranslatorExecute(object obj)
        {
            //Set loading as Hidden by default 
            //(Incase input is invalid)
            IsLoading = Visibility.Hidden;
            IsTranslatedGridVisible = Visibility.Hidden;
            //Check if text box is empty

            if (Text == null || Text == "")
            {
                MessageBox.Show("Vui lòng nhập từ!");
            }
            if (SourceLang == TranslateLang)
            {
                MessageBox.Show("Vui lòng chọn hai ngôn ngữ khác nhau!");
            }
            else
            {
                IsLoading = Visibility.Visible;
                TranslatedWord = new Word();
                //Translate text
                await TranslateInput(SourceLang, TranslateLang);

                //Get synonyms and parts of speech of word
                await Task.WhenAll(
                        //Get synonyms and parts of speech of word
                        DictionaryLookupInput(SourceLang, TranslateLang),
                        //Get example image from text
                        GetImageOfTranslatedText()
                    );

                if (IsSuccess)
                {
                    //Binding data to ListView
                    WordListView = new WordListView();
                    WordListView.PopulateWordListView(TranslatedWord.GetExamplesOfSynonym());
                    //Set loading to hidden
                    IsTranslatedGridVisible = Visibility.Visible;
                    IsLoading = Visibility.Hidden;
                    // Create a new SavedWord object
                    SavedWord translationItem = new SavedWord
                    {
                        SourceLang = SourceLang,
                        TranslateLang = TranslateLang,
                        Text = Text,
                        TranslatedText = TranslatedText,
                        PartOfSpeech = PartOfSpeech,
                        Image = Image,
                        WordListView = WordListView
                    };

                    // Check if there is already an item with the same Text
                    if (!SavedWords.Any(savedWord => savedWord.Text == Text))
                    {

                        // Limit the number of saved words to 10
                        if (SavedWords.Count >= maxSavedWords)
                        {
                            // Remove the oldest item
                            SavedWords.RemoveAt(0);
                        }

                        // Add the item to the ObservableCollection
                        SavedWords.Add(translationItem);

                        // Save the list of translated items to a file
                        SaveFile.SaveTranslatedItemsToFile("..\\Log\\SavedWord.json", SavedWords);
                    }
                }
                else
                {
                    MessageBox.Show("Lỗi khi dịch từ.");
                    IsLoading = Visibility.Hidden;
                    logger.LogError("Error when translating input.");
                }
            }
        }

        private async Task GetImageOfTranslatedText()
        {
            //Get example image from text
            try
            {
                string unsplashApiKey = App.Current.Resources["UnsplashApiKey"].ToString();
                //Connect to Unsplash API with API credential
                if (TranslateLang.Equals("vi"))
                {
                    Image = await TextToImageAPI.GetImageFromText(Text);
                }
                else
                {
                    Image = await TextToImageAPI.GetImageFromText(TranslatedText);
                };
            }
            catch (Exception ex)
            {

                logger.LogError("Error when getting image of translated text.");
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
                                // 'translatedText' now contains the value of the "text" property
                                TranslatedWord.SetTranslatedWord(textToken.ToString());
                                TranslatedText = textToken.ToString();
                            }
                        }

                    }
                    else
                    {
                        MessageBox.Show("Dịch từ thất bại.");
                        IsLoading = Visibility.Hidden;
                        IsTranslatedGridVisible = Visibility.Hidden;
                        logger.LogError("Cannot translate input.");
                    }
                }

            }
            catch (Exception ex)
            {
                logger.LogError("Error when translating input.");
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

                    //Get example of translation
                    try
                    {
                        await GetAllSynonymExamples(SourceLang, TranslateLang);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError("Error when getting all synonym examples.");
                    }

                }
                else
                {
                    MessageBox.Show("Không tìm được các thuộc tính của từ.");
                    IsLoading = Visibility.Hidden;
                    IsTranslatedGridVisible = Visibility.Hidden;
                    logger.LogError("Error when looking up dictionary.");
                }

            }
            catch (Exception ex)
            {
                logger.LogError("Error when looking up dictionary.");
            }
        }

        private async Task GetAllSynonymExamples(string from, string to)
        {
            foreach (WordSynonym synonym in TranslatedWord.GetSynonyms())
            {
                await DictionaryExampleInput(synonym.GetSynonym(), SourceLang, TranslateLang);
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

                    //After getting all examples
                    //(Complete translation)
                    //, set IsSuccess to true
                    IsSuccess = true;
                }
                else
                {
                    MessageBox.Show("Không tìm được từ đồng nghĩa của từ.");
                    IsLoading = Visibility.Hidden;
                    IsTranslatedGridVisible = Visibility.Hidden;
                    logger.LogError("Error when looking up dictionary.");
                }


            }
            catch (Exception ex)
            {
                logger.LogError("Error when looking up dictionary.");
            }
        }
    }
}