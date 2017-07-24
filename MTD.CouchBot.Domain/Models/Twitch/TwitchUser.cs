using Newtonsoft.Json;
using System.Collections.Generic;

namespace MTD.CouchBot.Domain.Models.Twitch
{
    public class TwitchUser
    {
        public class User
        {
            [JsonProperty("display_name")]
            public string DisplayName { get; set; }
            [JsonProperty("_id")]
            public string Id { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("type")]
            public string Type { get; set; }
            [JsonProperty("bio")]
            public string Bio { get; set; }
            [JsonProperty("created_at")]
            public string CreatedAt { get; set; }
            [JsonProperty("updated_at")]
            public string UpdatedAt { get; set; }
            [JsonProperty("logo")]
            public string Logo { get; set; }
        }

        [JsonProperty("_total")]
        public int Total { get; set; }
        [JsonProperty("users")]
        public List<User> Users { get; set; }
    }
}
