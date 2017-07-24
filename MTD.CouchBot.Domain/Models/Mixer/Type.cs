using Newtonsoft.Json;

namespace MTD.CouchBot.Domain.Models.Mixer
{
    public class Type
    {
        [JsonProperty("id")]
        public int? Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("parent")]
        public string Parent { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("source")]
        public string Source { get; set; }
        [JsonProperty("viewersCurrent")]
        public int ViewersCurrent { get; set; }
        [JsonProperty("coverUrl")]
        public string CoverUrl { get; set; }
        [JsonProperty("online")]
        public int Online { get; set; }
    }
}
