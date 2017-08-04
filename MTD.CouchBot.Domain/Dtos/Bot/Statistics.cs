using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTD.CouchBot.Domain.Dtos.Bot
{
    [Table("Statistics", Schema = "Bot")]
    public class Statistics
    {
        public int Id { get; set; }
        public int YouTubeAlertCount { get; set; }
        public int TwitchAlertCount { get; set; }
        public int MixerAlertCount { get; set; }
        public int SmashcastAlertCount { get; set; }
        public int PicacrtoAlertCount { get; set; }
        public int VidMeAlertCount { get; set; }
        public int UptimeMinutes { get; set; }
        public DateTime LoggingStartDate { get; set; }
        public DateTime LastRestartDate { get; set; }
        public int HaiBaiCount { get; set; }
        public int FlipCount { get; set; }
        public int UnflipCount { get; set; }
    }
}
