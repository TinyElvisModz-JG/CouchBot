using Newtonsoft.Json;

namespace MTD.CouchBot.Domain.Models.Twitch
{
    public class Stream
    {
        [JsonProperty("_id")]
        public long Id { get; set; }
        [JsonProperty("game")]
        public string Game { get; set; }
        [JsonProperty("viewers")]
        public int Viewers { get; set; }
        [JsonProperty("video_height")]
        public int VideoGeight { get; set; }
        [JsonProperty("average_fps")]
        public decimal AverageFps { get; set; }
        [JsonProperty("delay")]
        public int Delay { get; set; }
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
        [JsonProperty("is_playlist")]
        public bool IsPlaylist { get; set; }
        [JsonProperty("preview")]
        public Preview Preview { get; set; }
        [JsonProperty("channel")]
        public Channel Channel { get; set; }
    }
}
