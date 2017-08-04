using System.ComponentModel.DataAnnotations.Schema;

namespace MTD.CouchBot.Domain.Dtos.Bot
{
    [Table("Configuration", Schema = "Bot")]
    public class Configuration
    {
        public int Id { get; set; }
        public string DiscordToken { get; set; }
        public string TwitchClientId { get; set; }
        public string YouTubeApiKey { get; set; }
        public string ApiAiKey { get; set; }
        public string BotId { get; set; }
        public string Prefix { get; set; }
        public int TotalShards { get; set; }
        public string OwnerId { get; set; }
        public bool EnableMixer { get; set; }
        public bool EnablePicarto { get; set; }
        public bool EnableSmashcast { get; set; }
        public bool EnableTwitch { get; set; }
        public bool EnableVidMe { get; set; }
        public bool EnableYouTube { get; set; }
        public int PicartoInterval { get; set; }
        public int SmashcastInterval { get; set; }
        public int TwitchInterval { get; set; }
        public int TwitchFeedInterval { get; set; }
        public int YouTubeLiveInterval { get; set; }
        public int YouTubePublishedInterval { get; set; } 
        public int VidMeInterval { get; set; }
    }
}
