using Newtonsoft.Json;
using System.Collections.Generic;

namespace MTD.CouchBot.Domain.Models.Mixer
{
    public class Social
    {
        [JsonProperty("twitter")]
        public string Twitter { get; set; }
        [JsonProperty("youtube")]
        public string Youtube { get; set; }
        [JsonProperty("player")]
        public string Player { get; set; }
        [JsonProperty("verified")]
        public List<object> Verified { get; set; }
        [JsonProperty("discord")]
        public string Discord { get; set; }
    }
}
