using Newtonsoft.Json;

namespace MTD.CouchBot.Domain.Models.Picarto
{
    public class PicartoMultistream
    {
        [JsonProperty("user_id")]
        public int UserId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("online")]
        public bool Online { get; set; }
        [JsonProperty("adult")]
        public bool Adult { get; set; }
    }
}
