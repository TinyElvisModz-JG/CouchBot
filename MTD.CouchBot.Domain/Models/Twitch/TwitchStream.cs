using Newtonsoft.Json;

namespace MTD.CouchBot.Domain.Models.Twitch
{
    public class TwitchStream
    {
        [JsonProperty("stream")]
        public Stream Stream { get; set; }
    }
}
