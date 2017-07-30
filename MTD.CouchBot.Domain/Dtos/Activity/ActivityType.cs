using System.ComponentModel.DataAnnotations.Schema;

namespace MTD.CouchBot.Domain.Dtos.Activity
{
    [Table("ActivityType", Schema = "Activity")]
    public class ActivityType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
