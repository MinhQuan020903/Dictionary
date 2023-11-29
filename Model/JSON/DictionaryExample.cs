using Newtonsoft.Json;
using System.Collections.Generic;

namespace Dictionary.Model.JSON
{
    public class DictionaryExample
    {

        [JsonProperty("normalizedSource")]
        public string normalizedSource { get; set; }
        [JsonProperty("normalizedTarget")]
        public string normalizedTarget { get; set; }
        [JsonProperty("examples")]
        public List<WordExample> examples { get; set; }

        public DictionaryExample(string normalizedSource, string normalizedTarget, List<WordExample> examples)
        {
            this.normalizedSource = normalizedSource;
            this.normalizedTarget = normalizedTarget;
            this.examples = examples;
        }
    }
}
