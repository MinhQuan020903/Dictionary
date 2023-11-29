using Newtonsoft.Json;
using System.Collections.Generic;

namespace Dictionary.Model.JSON
{
    public class BackTranslations
    {
        [JsonProperty("normalizedText")]
        public string normalizedText { get; set; }
        [JsonProperty("displayText")]
        public string displayText { get; set; }
        [JsonProperty("numExamples")]
        public int numExamples { get; set; }
        [JsonProperty("frequencyCount")]
        public int frequencyCount { get; set; }

        public BackTranslations(string normalizedText, string displayText, int numExamples, int frequencyCount)
        {
            this.normalizedText = normalizedText;
            this.displayText = displayText;
            this.numExamples = numExamples;
            this.frequencyCount = frequencyCount;
        }
    }
    public class WordLookup
    {
        [JsonProperty("normalizedTarget")]
        public string normalizedTarget { get; set; }
        [JsonProperty("displayTarget")]
        public string displayTarget { get; set; }
        [JsonProperty("posTag")]
        public string posTag { get; set; }
        [JsonProperty("confidence")]
        public double confidence { get; set; }
        [JsonProperty("prefixWord")]
        public string prefixWord { get; set; }
        [JsonProperty("backTranslations")]
        public List<BackTranslations> backTranslations { get; set; }

        public WordLookup(string normalizedTarget, string displayTarget, string posTag, double confidence, string prefixWord, List<BackTranslations> backTranslations)
        {
            this.normalizedTarget = normalizedTarget;
            this.displayTarget = displayTarget;
            this.posTag = posTag;
            this.confidence = confidence;
            this.prefixWord = prefixWord;
            this.backTranslations = backTranslations;

            //Format pos
            this.posTag = GetPartOfSpeech(posTag);
        }

        public string GetPartOfSpeech(string pos)
        {
            switch (pos)
            {
                case "ADJ":
                    {
                        return "adjective";
                    }
                case "ADV":
                    {
                        return "adverb";
                    }
                case "CONJ":
                    {
                        return "conjunction";
                    }
                case "DET":
                    {
                        return "determiner";
                    }
                case "MODAL":
                    {
                        return "modal";
                    }
                case "NOUN":
                    {
                        return "noun";
                    }
                case "PREP":
                    {
                        return "preposition";
                    }
                case "PRON":
                    {
                        return "pronoun";
                    }
                case "VERB":
                    {
                        return "verb";
                    }
                default:
                    {
                        return "unknown";
                    }
            }
        }

        public string ToString()
        {
            string result = "";
            result += "normalizedTarget: " + normalizedTarget + "\n";
            result += "displayTarget: " + displayTarget + "\n";
            result += "posTag: " + GetPartOfSpeech(posTag) + "\n";
            result += "confidence: " + confidence + "\n";
            result += "prefixWord: " + prefixWord + "\n";
            result += "backTranslations: \n";
            foreach (BackTranslations backTranslation in backTranslations)
            {
                result += backTranslation.ToString();
            }
            return result;
        }

    }

}
