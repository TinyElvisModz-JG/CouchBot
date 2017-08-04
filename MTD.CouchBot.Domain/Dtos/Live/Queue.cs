using System;

namespace MTD.CouchBot.Domain.Dtos.Live
{
    public class Queue
    {
        public int Id { get; set; }
        public int ChannelId { get; set; }
        public int PlatformId { get; set; }
        public int GuildId { get; set; }
        public string MessageId { get; set; }
        public bool DeleteOffline { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
