using System.ComponentModel.DataAnnotations.Schema;

namespace MTD.CouchBot.Domain.Dtos.Discord
{
    [Table("Server", Schema = "Discord")]
    public class Server
    {
        public ulong Id { get; set; }
        public ulong OwnerId { get; set; }
        public ulong AnnouncementsChannel { get; set; }
        public ulong LiveChannel { get; set; }
        public ulong PublishedChannel { get; set; }
        public ulong OwnerLiveChannel { get; set; }
        public ulong OwnerPublishedChannel { get; set; }
        public ulong GreetingsChannel { get; set; }
        public ulong OwnerTwitchFeedChannel { get; set; }
        public ulong TwitchFeedChannel { get; set; }
        public bool AllowEveryone { get; set; }
        public bool AllowThumbnails { get; set; }
        public bool AllowGreetings { get; set; }
        public bool AllowGoodbyes { get; set; }
        public bool AllowPublished { get; set; }
        public bool AllowLive { get; set; }
        public bool AllowOwnerChannelFeed { get; set; }
        public bool AllowChannelFeed { get; set; }
        public float TimeZoneOffset { get; set; }
        public bool YtgDomainPublished { get; set; }
        public bool DeleteWhenOffline { get; set; }
        public ulong MentionRole { get; set; }
        public bool UseTextAnnouncements { get; set; }
        public string GreetingMessage { get; set; }
        public string GoodbyeMessage { get; set; }
        public string LiveMessage { get; set; }
        public string PublishedMessage { get; set; }
        public string StreamOfflineMessage { get; set; }
    }
}
