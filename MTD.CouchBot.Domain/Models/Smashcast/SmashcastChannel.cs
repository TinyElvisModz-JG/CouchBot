using Newtonsoft.Json;
using System.Collections.Generic;

namespace MTD.CouchBot.Domain.Models.Smashcast
{
    public class SmashcastChannel
    {
        [JsonProperty("request")]
        public Request Request { get; set; }
        [JsonProperty("media_type")]
        public string MediaType { get; set; }
        [JsonProperty("livestream")]
        public List<Livestream> Livestream { get; set; }
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("error")]
        public bool Error { get; set; }
        [JsonProperty("error_msg")]
        public string ErrorMessage { get; set; }
    }
}
