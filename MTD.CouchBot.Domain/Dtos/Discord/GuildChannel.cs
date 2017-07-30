using MTD.CouchBot.Domain.Dtos.Platform;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTD.CouchBot.Domain.Dtos.Discord
{
    [Table("GuildChannel", Schema = "Discord")]
    public class GuildChannel
    {
        public int GuildId { get; set; }
        public Guild Guild { get; set; }

        public int ChannelId { get; set; }
        public Channel Channel { get; set; }
    }
}
