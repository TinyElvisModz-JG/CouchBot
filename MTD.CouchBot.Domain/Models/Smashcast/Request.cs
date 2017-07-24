using Newtonsoft.Json;

namespace MTD.CouchBot.Domain.Models.Smashcast
{
    public class Request
    {
        [JsonProperty("@this")]
        public string This { get; set; }
    }
}
