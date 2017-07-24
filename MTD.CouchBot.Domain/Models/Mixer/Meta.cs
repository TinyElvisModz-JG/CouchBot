using Newtonsoft.Json;

namespace MTD.CouchBot.Domain.Models.Mixer
{
    public class Meta
    {
        [JsonProperty("small")]
        public string Small { get; set; }
    }
}
