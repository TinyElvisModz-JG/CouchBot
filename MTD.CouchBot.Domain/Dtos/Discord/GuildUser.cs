using MTD.CouchBot.Domain.Dtos.Platform;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTD.CouchBot.Domain.Dtos.Discord
{
    [Table("GuildUser", Schema = "Discord")]
    public class GuildUser
    {
        public int GuildId { get; set; }
        public Guild Guild { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
