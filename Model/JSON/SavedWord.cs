using Dictionary.Model.Word;

namespace Dictionary.Model.JSON
{
    public class SavedWord
    {
        public string SourceLang { get; set; }
        public string TranslateLang { get; set; }
        public string Text { get; set; }
        public string TranslatedText { get; set; }
        public string PartOfSpeech { get; set; }
        public string Image { get; set; }
        public WordListView WordListView { get; set; }
    }
}
