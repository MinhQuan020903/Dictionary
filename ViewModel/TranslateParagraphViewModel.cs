using Dictionary.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    public class TranslateParagraphViewModel : BaseViewModel
    {
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
        public ICommand SelectSourceLanguageCommand { get; set; }
        public ICommand SelectTranslateLanguageCommand { get; set; }

        public TranslateParagraphViewModel()
        {
            LangList = new List<LanguageObject>();
            LangList.Add(new LanguageObject("Tiếng Việt", "vi"));
            LangList.Add(new LanguageObject("Tiếng Anh", "en"));

            TranslateCommand = new RelayCommand<object>(TranslateCommandCanExecute, TranslateCommandExecute);
        }

        private bool TranslateCommandCanExecute(object obj)
        {
            return true;
        }

        private async void TranslateCommandExecute(object obj)
        {
            if(SourceParagraph == "")
            {
                MessageBox.Show("Please enter source paragraph");
                return;
            }

            string translatedText = await TranslateAPI.Translate(SourceParagraph, _sourceLang, _translateLang);

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
                        // 'translatedText' now contains the value of the "text" property
                        TranslatedParagraph = textToken.ToString();
                    }
                }
            }
        }
    }
}
