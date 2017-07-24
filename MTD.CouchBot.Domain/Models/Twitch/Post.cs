using Newtonsoft.Json;

namespace MTD.CouchBot.Domain.Models.Twitch
{
    public class Post
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
    }
}
