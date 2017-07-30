using System;
using System.Collections.Generic;
using System.Text;
using MTD.CouchBot.Domain.Dtos.Platform;
using MTD.CouchBot.Domain.Dtos.Discord;

namespace MTD.CouchBot.Domain.Dtos.Activity
{
    public class LiveNotification
    {
        public int Id { get; set; }
        public int ChannelId { get; set; }
        public int PlatformId { get; set; }
        public int GuildId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Platform.Platform Platform { get; set; }
        public Guild Guild { get; set; }

    }
}
