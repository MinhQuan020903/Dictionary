using Dictionary.Model;
using Dictionary.Model.API;
using Dictionary.Model.JSON;
using Dictionary.Model.Word;
using Microsoft.Identity.Client.NativeInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Unsplasharp;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Logging;

namespace Dictionary.ViewModel
{
    public class LanguageObject
    {
        private string _language = "";
        private string _langCode = "";

        public string Language
        {
            get { return _language; }
            set { _language = value; }
        }

        public string LangCode
        {
            get { return _langCode; }
            set { _langCode = value; }
        }

        public LanguageObject(string languageValue, string langCodeValue)
        {
            _language = languageValue;
            _langCode = langCodeValue;
        }
    }

    public class TranslateParagraphPageViewModel : BaseViewModel
    {
        private ILogger<TranslateParagraphPageViewModel> logger;
        private const int maxSavedParagraphs = 20;

        private string _filePath = "SavedParagraph.json";
        private string _baseDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));

        private bool _isTranslating = false;
        public bool IsTranslating
        {
            get => _isTranslating;
            set
            {
                _isTranslating = value;
                OnPropertyChanged(nameof(IsTranslating));
            }
        }
        private bool _isSpeechListening = false;
        public bool IsSpeechListening
        {
            get => _isSpeechListening;
            set
            {
                _isSpeechListening = value;
                OnPropertyChanged(nameof(IsSpeechListening));
            }
        }

        private ObservableCollection<SavedParagraph> _savedParagraphs;

        public ObservableCollection<SavedParagraph> SavedParagraphs
        {
            get => _savedParagraphs;
            set
            {
                _savedParagraphs = value;
                OnPropertyChanged(nameof(SavedParagraphs));
            }
        }

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
                if (_sourceLang != value)
                {
                    _sourceLang = value;
                    OnPropertyChanged(nameof(SourceLang));
                    TranslateLang = value == "vi" ? "en" : "vi";
                }
            }
        }

        private string _translateLang = "en";
        public string TranslateLang
        {
            get { return _translateLang; }
            set
            {
                if (_translateLang != value)
                {
                    _translateLang = value;
                    OnPropertyChanged(nameof(TranslateLang));
                    SourceLang = value == "vi" ? "en" : "vi";
                }
            }
        }

        private string _sourceParagraph = "";
        public string SourceParagraph
        {
            get => _sourceParagraph;
            set
            {
                _sourceParagraph = value;
                OnPropertyChanged(nameof(SourceParagraph));
            }
        }
        private string _translatedParagraph = "";
        public string TranslatedParagraph
        {
            get => _translatedParagraph;
            set
            {
                _translatedParagraph = value;
                OnPropertyChanged(nameof(TranslatedParagraph));
            }
        }
        public ICommand TranslateCommand { get; set; }
        public ICommand SavedParagraphsSelectionChangedCommand { get; set; }
        public ICommand UploadParagraphCommand { get; set; }
        public ICommand SpeakTranslateParagraphCommand { get; set; }
        public ICommand SpeakSourceParagraphCommand { get; set; }
        public ICommand CopySourceParagraphCommand { get; set; }
        public ICommand CopyTranslateParagraphCommand { get; set; }
        public ICommand SpeechToSourceTextCommand { get; set; }

        public TranslateParagraphPageViewModel()
        {
            LangList = new List<LanguageObject>();
            LangList.Add(new LanguageObject("Tiếng Việt", "vi"));
            LangList.Add(new LanguageObject("Tiếng Anh", "en"));

            SavedParagraphs = new ObservableCollection<SavedParagraph>();
            SavedParagraphs = SaveFile.LoadSavedParagraphs();

            TranslateCommand = new RelayCommand<object>(TranslateCommandCanExecute, TranslateCommandExecute);
            SavedParagraphsSelectionChangedCommand = new RelayCommand<SavedParagraph>((SavedParagraph obj) => true, SavedParagraphsSelectionChangedCommandExecute);
            UploadParagraphCommand = new RelayCommand<object>(_ => true, UploadParagraphCommandExecute);
            SpeakTranslateParagraphCommand = new RelayCommand<object>(_ => true, SpeakTranslateParagraphCommandExecute);
            SpeakSourceParagraphCommand = new RelayCommand<object>(_ => true, SpeakSourceParagraphCommandExecute);
            CopySourceParagraphCommand = new RelayCommand<object>(_ => true, _ => System.Windows.Clipboard.SetText(SourceParagraph));
            CopyTranslateParagraphCommand = new RelayCommand<object>(_ => true, _ => System.Windows.Clipboard.SetText(TranslatedParagraph));
            SpeechToSourceTextCommand = new RelayCommand<object>(_ => true, SpeechToSourceTextCommandExecute);

            //Create logger object
            logger = LoggerProvider.CreateLogger<TranslateParagraphPageViewModel>();
        }

        private bool TranslateCommandCanExecute(object obj)
        {
            return true;
        }

        private async void TranslateCommandExecute(object obj)
        {
            IsTranslating = true;
            if (SourceParagraph == "")
            {
                System.Windows.MessageBox.Show("Please enter source paragraph");
                return;
            }

            ApiResponse<string> response = await TranslateAPI.Translate(SourceParagraph, SourceLang, TranslateLang, logger);

            // Parse the JSON array
            JArray jsonArray = JArray.Parse(response.Data);
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
                        TranslatedParagraph = textToken.ToString();

                        SavedParagraph newParagraph = new SavedParagraph()
                        { SourceLangCode = SourceLang, SourceParagraph = SourceParagraph, TranslatedLangCode = TranslateLang };

                        if (SavedParagraphs.Any(item => item.SourceParagraph == newParagraph.SourceParagraph && item.SourceLangCode == newParagraph.SourceLangCode && item.TranslatedLangCode == newParagraph.TranslatedLangCode))
                        {
                            IsTranslating = false;
                            return;
                        }

                        if (SavedParagraphs.Count >= maxSavedParagraphs) { SavedParagraphs.RemoveAt(0); }
                        SavedParagraphs.Add(newParagraph);

                        SaveFile.SaveTranslatedParagraphToFile(SavedParagraphs);
                    }
                }
            }
            IsTranslating = false;
        }

        private void SavedParagraphsSelectionChangedCommandExecute(SavedParagraph selectedItem)
        {
            TranslateLang = selectedItem.TranslatedLangCode;

            SourceLang = selectedItem.SourceLangCode;
            SourceParagraph = selectedItem.SourceParagraph;

            TranslateCommandExecute(new object());
        }

        private void UploadParagraphCommandExecute(object obj)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.InitialDirectory = "c:\\";

            dlg.Filter = "Text file | *.txt";

            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SourceParagraph = File.ReadAllText(dlg.FileName);
                TranslateCommandExecute(obj);
            }
        }

        private async void SpeakTranslateParagraphCommandExecute(object obj)
        {
            TextToSpeechAPI textToSpeech = new TextToSpeechAPI();
            await textToSpeech.TextToSpeech(TranslatedParagraph, SourceLang, TranslateLang, logger);
        }

        private async void SpeakSourceParagraphCommandExecute(object obj)
        {
            TextToSpeechAPI textToSpeech = new TextToSpeechAPI();
            await textToSpeech.TextToSpeech(SourceParagraph, TranslateLang, SourceLang, logger);
        }

        private async void SpeechToSourceTextCommandExecute(object obj)
        {
            IsSpeechListening = true;
            Task<string> speechToTextTask = SpeechToTextAPI.SpeechToText(SourceLang, logger);

            // Wait for either the SpeechToText operation to complete or a timeout of 5 seconds
            Task completedTask = await Task.WhenAny(speechToTextTask, Task.Delay(5000));

            if (completedTask == speechToTextTask)
            {
                // SpeechToText operation completed within the timeout
                SourceParagraph = await speechToTextTask;

                if (string.IsNullOrEmpty(SourceParagraph))
                {
                    TranslatedParagraph = "";
                    System.Windows.MessageBox.Show("Không nhận được giọng nói.");
                }
                else
                {
                    TranslateCommandExecute(obj);
                }
            }
            IsSpeechListening = false;
        }
    }
}
