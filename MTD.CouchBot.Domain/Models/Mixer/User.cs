using Newtonsoft.Json;
using System.Collections.Generic;

namespace MTD.CouchBot.Domain.Models.Mixer
{
    public class User
    {
        [JsonProperty("level")]
        public int Level { get; set; }
        [JsonProperty("social")]
        public Social Social { get; set; }
        [JsonProperty("id")]
        public int? Id { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("verified")]
        public bool Verified { get; set; }
        [JsonProperty("experience")]
        public int Experience { get; set; }
        [JsonProperty("sparks")]
        public int Sparks { get; set; }
        [JsonProperty("avatarUrl")]
        public string AvatarUrl { get; set; }
        [JsonProperty("bio")]
        public string Bio { get; set; }
        [JsonProperty("primaryTeam")]
        public object PrimaryTeam { get; set; }
        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }
        [JsonProperty("updatedAt")]
        public string UpdatedAt { get; set; }
        [JsonProperty("deletedAt")]
        public object DeletedAt { get; set; }
        [JsonProperty("groups")]
        public List<Group> Groups { get; set; }
        [JsonProperty("channel")]
        public Channel Channel { get; set; }
    }
}
