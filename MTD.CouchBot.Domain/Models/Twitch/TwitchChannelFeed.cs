using Newtonsoft.Json;
using System.Collections.Generic;

namespace MTD.CouchBot.Domain.Models.Twitch
{
    public class TwitchChannelFeed
    {
        [JsonProperty("_disabled")]
        public bool Disabled { get; set; }
        [JsonProperty("_cursor")]
        public string Cursor { get; set; }
        [JsonProperty("_topic")]
        public string Topic { get; set; }
        [JsonProperty("posts")]
        public List<Post> Posts { get; set; }
    }
}
