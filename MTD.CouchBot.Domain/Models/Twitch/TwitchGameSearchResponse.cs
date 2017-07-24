using Newtonsoft.Json;
using System.Collections.Generic;

namespace MTD.CouchBot.Domain.Models.Twitch
{
    public class TwitchGameSearchResponse
    {
        [JsonProperty("games")]
        public List<Game> Games { get; set; }

        public class Box
        {
            [JsonProperty("large")]
            public string Large { get; set; }
            [JsonProperty("medium")]
            public string Medium { get; set; }
            [JsonProperty("small")]
            public string Small { get; set; }
            [JsonProperty("template")]
            public string Template { get; set; }
        }

        public class Logo
        {
            [JsonProperty("large")]
            public string Large { get; set; }
            [JsonProperty("medium")]
            public string Medium { get; set; }
            [JsonProperty("small")]
            public string Small { get; set; }
            [JsonProperty("template")]
            public string Template { get; set; }
        }

        public class Game
        {
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("popularity")]
            public int Popularity { get; set; }
            [JsonProperty("_id")]
            public int Id { get; set; }
            [JsonProperty("giantbomb_id")]
            public int GiantbombId { get; set; }
            [JsonProperty("box")]
            public Box Box { get; set; }
            [JsonProperty("logo")]
            public Logo Logo { get; set; }
            [JsonProperty("localized_name")]
            public string LocalizedName { get; set; }
            [JsonProperty("locale")]
            public string Locale { get; set; }
        }
    }
}
