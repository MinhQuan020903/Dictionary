using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
