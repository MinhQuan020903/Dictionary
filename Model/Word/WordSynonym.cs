namespace Dictionary.Model.Word
{
    public class WordSynonym
    {
        private string Synonym;
        private string PartOfSpeech;

        public WordSynonym() {; }
        public WordSynonym(string synonym, string partOfSpeech)
        {
            Synonym = synonym;
            PartOfSpeech = partOfSpeech;
        }

        public string GetSynonym()
        {
            return Synonym;
        }

        public void SetSynonym(string synonym)
        {
            Synonym = synonym;
        }

        public string GetPartOfSpeech()
        {
            return PartOfSpeech;
        }

        public void SetPartOfSpeech(string partOfSpeech)
        {
            PartOfSpeech = partOfSpeech;
        }

        public override bool Equals(object obj)
        {
            return obj is WordSynonym synonym &&
                   Synonym == synonym.Synonym &&
                   PartOfSpeech == synonym.PartOfSpeech;
        }


    }
}
