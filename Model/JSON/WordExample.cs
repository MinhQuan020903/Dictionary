using Newtonsoft.Json;

namespace Dictionary.Model.JSON
{
    public class WordExample
    {
        [JsonProperty("sourcePrefix")]
        public string sourcePrefix { get; set; }
        [JsonProperty("sourceTerm")]
        public string sourceTerm { get; set; }
        [JsonProperty("sourceSuffix")]
        public string sourceSuffix { get; set; }
        [JsonProperty("targetPrefix")]
        public string targetPrefix { get; set; }
        [JsonProperty("targetTerm")]
        public string targetTerm { get; set; }
        [JsonProperty("targetSuffix")]
        public string targetSuffix { get; set; }

        public WordExample(string sourcePrefix, string sourceTerm, string sourceSuffix, string targetPrefix, string targetTerm, string targetSuffix)
        {
            this.sourcePrefix = sourcePrefix;
            this.sourceTerm = sourceTerm;
            this.sourceSuffix = sourceSuffix;
            this.targetPrefix = targetPrefix;
            this.targetTerm = targetTerm;
            this.targetSuffix = targetSuffix;
        }

        public string ToString()
        {
            return "sourcePrefix: " + sourcePrefix + "\n" +
                "sourceTerm: " + sourceTerm + "\n" +
                "sourceSuffix: " + sourceSuffix + "\n" +
                "targetPrefix: " + targetPrefix + "\n" +
                "targetTerm: " + targetTerm + "\n" +
                "targetSuffix: " + targetSuffix + "\n";
        }
    }
}
