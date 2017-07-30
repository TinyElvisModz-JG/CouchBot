using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTD.CouchBot.Domain.Dtos.Discord
{
    [Table("User", Schema = "Discord")]
    public class User
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<GuildUser> GuildUsers { get; set; }
    }
}
