using System.ComponentModel.DataAnnotations.Schema;

namespace MTD.CouchBot.Domain.Dtos.Activity
{
    [Table("FunEventType", Schema = "Activity")]
    public class FunEventType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
