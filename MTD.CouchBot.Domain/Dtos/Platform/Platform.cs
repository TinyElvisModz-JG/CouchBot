using System.ComponentModel.DataAnnotations.Schema;

namespace MTD.CouchBot.Domain.Dtos.Platform
{
    [Table("Platform", Schema = "Platform")]
    public class Platform
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
