using Newtonsoft.Json;
using System.Collections.Generic;

namespace MTD.CouchBot.Domain.Models.ApiAi
{
    public class Metadata
    {
        [JsonProperty("inputContexts")]
        public List<object> InputContexts { get; set; }
        [JsonProperty("outputContexts")]
        public List<object> OutputContexts { get; set; }
        [JsonProperty("contexts")]
        public List<object> Contexts { get; set; }
    }
}
