using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTD.CouchBot.Domain.Dtos.Platform
{
    [Table("Team", Schema = "Platform")]
    public class Team
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
