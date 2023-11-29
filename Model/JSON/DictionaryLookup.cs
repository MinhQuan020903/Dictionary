using Newtonsoft.Json;
using System.Collections.Generic;

namespace Dictionary.Model.JSON
{
    public class DictionaryLookup
    {
        [JsonProperty("normalizedSource")]
        public string normalizedSource { get; set; }
        [JsonProperty("displaySource")]
        public string displaySource { get; set; }
        [JsonProperty("translations")]
        public List<WordLookup> translations { get; set; }

        public DictionaryLookup() { }
        public DictionaryLookup(string normalizedSource, string displaySource, List<WordLookup> translations)
        {
            this.normalizedSource = normalizedSource;
            this.displaySource = displaySource;
            this.translations = translations;
            translations.Sort((x, y) => y.confidence.CompareTo(x.confidence));
        }

        public string getALlPartOfSpeech()
        {
            string result = "";
            foreach (WordLookup word in translations)
            {
                if (result.Contains("[" + word.posTag + "] ") == false)
                {
                    result += "[" + word.posTag + "] ";
                }

            }
            return result;
        }

    }
}
