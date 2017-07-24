using Newtonsoft.Json;

namespace MTD.CouchBot.Domain.Models.ApiAi
{
    public class Result
    {
        [JsonProperty("source")]
        public string Source { get; set; }
        [JsonProperty("resolvedQuery")]
        public string ResolvedQuery { get; set; }
        [JsonProperty("speech")]
        public string Speech { get; set; }
        [JsonProperty("action")]
        public string Action { get; set; }
        [JsonProperty("parameters")]
        public Parameters Parameters { get; set; }
        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }
        [JsonProperty("score")]
        public double Score { get; set; }
    }
}
