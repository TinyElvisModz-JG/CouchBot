using MTD.CouchBot.Domain.Dtos.Discord;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTD.CouchBot.Domain.Dtos.Platform
{
    [Table("Channel", Schema = "Platform")]
    public class Channel
    {
        public int Id { get; set; }
        public int PlatformId { get; set; }
        public string ChannelId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Platform Platform { get; set; }

        public List<GuildChannel> GuildChannels { get; set; }
    }
}
