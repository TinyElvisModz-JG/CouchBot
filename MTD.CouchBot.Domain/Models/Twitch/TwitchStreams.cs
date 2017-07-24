using Newtonsoft.Json;
using System.Collections.Generic;

namespace MTD.CouchBot.Domain.Models.Twitch
{
    public class TwitchStreams
    {
        public int _total { get; set; }
        [JsonProperty("streams")]
        public List<Stream> Streams { get; set; }
    }
}
