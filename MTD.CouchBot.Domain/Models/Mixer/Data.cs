using Newtonsoft.Json;

namespace MTD.CouchBot.Domain.Models.Mixer
{
    public class Data
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }
        [JsonProperty("payload")]
        public Payload Payload { get; set; }
    }
}
