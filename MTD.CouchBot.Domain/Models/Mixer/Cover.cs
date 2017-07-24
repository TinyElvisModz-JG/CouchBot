using Newtonsoft.Json;

namespace MTD.CouchBot.Domain.Models.Mixer
{
    public class Cover
    {
        [JsonProperty("meta")]
        public Meta Meta { get; set; }
        [JsonProperty("id")]
        public int? Id { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("relid")]
        public object RelId { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("store")]
        public string Store { get; set; }
        [JsonProperty("remotePath")]
        public string RemotePath { get; set; }
        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }
        [JsonProperty("updatedAt")]
        public string UpdatedAt { get; set; }
    }
}
