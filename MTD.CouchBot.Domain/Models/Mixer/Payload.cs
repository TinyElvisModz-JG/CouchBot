using Newtonsoft.Json;

namespace MTD.CouchBot.Domain.Models.Mixer
{
    public class Payload
    {
        [JsonProperty("online")]
        public bool Online { get; set; }
    }
}
