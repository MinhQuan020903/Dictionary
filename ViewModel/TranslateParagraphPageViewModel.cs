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
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Unsplasharp;
using static System.Net.Mime.MediaTypeNames;

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
        private string _filePath = "SavedParagraph.json";
        string _baseDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));

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
        public TranslateParagraphPageViewModel()
        {
            LangList = new List<LanguageObject>();
            LangList.Add(new LanguageObject("Tiếng Việt", "vi"));
            LangList.Add(new LanguageObject("Tiếng Anh", "en"));

            SavedParagraphs = new ObservableCollection<SavedParagraph>();
            LoadSavedParagraphs();

            TranslateCommand = new RelayCommand<object>(TranslateCommandCanExecute, TranslateCommandExecute);
            SavedParagraphsSelectionChangedCommand = new RelayCommand<SavedParagraph>((SavedParagraph obj) => true, SavedParagraphsSelectionChangedExecute);
        }

        private bool TranslateCommandCanExecute(object obj)
        {
            return true;
        }

        private async void TranslateCommandExecute(object obj)
        {
            if (SourceParagraph == "")
            {
                MessageBox.Show("Please enter source paragraph");
                return;
            }

            ApiResponse<string> response = await TranslateAPI.Translate(SourceParagraph, _sourceLang, _translateLang);

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
                        SaveTranslatedParagraph();
                    }
                }
            }
        }

        private void SaveTranslatedParagraph()
        {
            SavedParagraphs.Add(new SavedParagraph() { SourceLangCode = SourceLang, SourceParagraph = SourceParagraph, TranslatedLangCode = TranslateLang, TranslateParagraph = TranslatedParagraph });
            try
            {
                // Get the base directory where the application is running

                // Check if the "Log" folder exists, if not, create it
                string logFolderPath = Path.Combine(_baseDirectory, "Log");
                if (!Directory.Exists(logFolderPath))
                {
                    Directory.CreateDirectory(logFolderPath);
                }

                // Construct the full file path
                string fullFilePath = Path.Combine(logFolderPath, _filePath);

                // Serialize and save translated items to a file
                string translatedItemsJson = JsonConvert.SerializeObject(SavedParagraphs);
                File.WriteAllText(fullFilePath, translatedItemsJson);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void LoadSavedParagraphs()
        {
            try
            {
                // Check if the "Log" folder exists, if not, create it
                string logFolderPath = Path.Combine(_baseDirectory, "Log", _filePath);

                if (File.Exists(logFolderPath))
                {
                    string translatedItemsJson = File.ReadAllText(logFolderPath);
                    SavedParagraphs = new ObservableCollection<SavedParagraph>(JsonConvert.DeserializeObject<List<SavedParagraph>>(translatedItemsJson));
                }
                else
                { }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SavedParagraphsSelectionChangedExecute(SavedParagraph selectedItem)
        {
            TranslateLang = selectedItem.TranslatedLangCode;
            TranslatedParagraph = selectedItem.TranslateParagraph;

            SourceLang = selectedItem.SourceLangCode;
            SourceParagraph = selectedItem.SourceParagraph;
        }
    }
}
