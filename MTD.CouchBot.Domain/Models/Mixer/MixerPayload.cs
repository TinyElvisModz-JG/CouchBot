using Newtonsoft.Json;

namespace MTD.CouchBot.Domain.Models.Mixer
{
    public class MixerPayload
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("@event")]
        public string Event { get; set; }
        [JsonProperty("data")]
        public Data Data { get; set; }
    }
}
