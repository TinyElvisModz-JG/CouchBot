using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTD.CouchBot.Domain.Dtos.Discord
{
    [Table("Guild", Schema = "Discord")]
    public class Guild
    {
        public int Id { get; set; }
        public string GuildId { get; set; }
        public string OwnerId { get; set; }
        public string AnnouncementsChannel { get; set; }
        public string LiveChannel { get; set; }
        public string PublishedChannel { get; set; }
        public string OwnerLiveChannel { get; set; }
        public string OwnerPublishedChannel { get; set; }
        public string GreetingsChannel { get; set; }
        public string OwnerTwitchFeedChannel { get; set; }
        public string TwitchFeedChannel { get; set; }
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
        public string MentionRole { get; set; }
        public bool UseTextAnnouncements { get; set; }
        public string GreetingMessage { get; set; }
        public string GoodbyeMessage { get; set; }
        public string LiveMessage { get; set; }
        public string PublishedMessage { get; set; }
        public string StreamOfflineMessage { get; set; }

        public List<GuildChannel> GuildChannels { get; set; }
        public List<GuildUser> GuildUsers { get; set; }
    }
}
