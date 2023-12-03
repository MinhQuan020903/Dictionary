using Dictionary.Model.JSON;
using System.Collections.Generic;
using System.Linq;

namespace Dictionary.Model.Word
{
    public class Word
    {
        private string TranslatedWord;
        private List<string> PartsOfSpeech;
        private List<WordSynonym> Synonyms;
        private Dictionary<WordSynonym, string> ExamplesOfSynonym;

        public Word()
        {
            TranslatedWord = "";
            PartsOfSpeech = new List<string>();
            Synonyms = new List<WordSynonym>();
            ExamplesOfSynonym = new Dictionary<WordSynonym, string>();
        }

        public Word(string translatedWord, List<string> partsOfSpeech, List<WordSynonym> synonyms, Dictionary<WordSynonym, string> examplesOfSynonym)
        {
            TranslatedWord = translatedWord;
            PartsOfSpeech = partsOfSpeech;
            Synonyms = synonyms;
            ExamplesOfSynonym = examplesOfSynonym;
        }

        public string GetTranslatedWord()
        {
            return TranslatedWord;
        }

        public void SetTranslatedWord(string translatedWord)
        {
            TranslatedWord = translatedWord;
        }

        public List<string> GetPartsOfSpeech()
        {
            return PartsOfSpeech;
        }

        public void SetPartsOfSpeech(List<string> partsOfSpeech)
        {
            List<string> strings = new List<string>();
            foreach (string pos in partsOfSpeech)
            {
                if (!strings.Contains(pos))
                {
                    strings.Add(pos);
                }

            }
            PartsOfSpeech = strings;
        }

        public List<WordSynonym> GetSynonyms()
        {
            return Synonyms;
        }

        public void SetSynonyms(List<WordSynonym> synonyms)
        {
            Synonyms = synonyms;
        }

        public void SetSynonyms(List<WordLookup> words)
        {
            foreach (WordLookup word in words)
            {
                if (!Synonyms.Contains(new WordSynonym(word.normalizedTarget, word.posTag)))
                {
                    Synonyms.Add(new WordSynonym(word.normalizedTarget, word.posTag));
                }
            }
        }

        public Dictionary<WordSynonym, string> GetExamplesOfSynonym()
        {
            return ExamplesOfSynonym;
        }

        public void SetExamplesOfSynonym(Dictionary<WordSynonym, string> examplesOfSynonym)
        {
            ExamplesOfSynonym = examplesOfSynonym;
        }

        public void SetExamplesOfSynonym(List<WordExample> examples)
        {
            foreach (WordExample example in examples)
            {
                foreach (WordSynonym synonym in Synonyms)
                {
                    if (synonym.GetSynonym().Equals(example.targetTerm) && !ExamplesOfSynonym.Keys.Any(key => key.GetSynonym().Equals(synonym.GetSynonym())))
                    {
                        string key = synonym.GetSynonym();
                        string value = example.targetPrefix + example.targetTerm + example.targetSuffix;
                        ExamplesOfSynonym.Add(new WordSynonym(synonym.GetSynonym(), synonym.GetPartOfSpeech()), value);
                    }
                }
            }
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            string ret = "TranslatedWord: " + TranslatedWord + "\n PartsOfSpeech: ";
            foreach (string pos in PartsOfSpeech)
            {
                ret += pos + " ";
            }
            ret += "\n Examples: ";

            foreach (KeyValuePair<WordSynonym, string> example in ExamplesOfSynonym)
            {
                ret += "\n " + example.Key.GetSynonym() + " " + example.Value;
            }
            ret += "\n Synonyms: ";

            return ret;
        }
    }
}
