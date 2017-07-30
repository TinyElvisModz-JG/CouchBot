using MTD.CouchBot.Domain.Dtos.Discord;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTD.CouchBot.Domain.Dtos.Activity
{
    [Table("Activity", Schema ="Activity")]
    public class Activity
    {
        public int Id { get; set; }
        public int GuildId { get; set; }
        public int UserId { get; set; }
        public int ActivityTypeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public ActivityType ActivityType { get; set; }
        public Guild Guild { get; set; }
    }
}
