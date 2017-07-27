using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTD.CouchBot.Domain.Dtos.Platform
{
    [Table("Game", Schema = "Platform")]
    public class Game
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
