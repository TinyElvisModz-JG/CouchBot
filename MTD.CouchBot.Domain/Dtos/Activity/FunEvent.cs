using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTD.CouchBot.Domain.Dtos.Activity
{
    [Table("FunEvent", Schema ="Activity")]
    public class FunEvent
    {
        public int Id { get; set; }
        public ulong ServerId { get; set; }
        public ulong UserId { get; set; }
        public int FunEventTypeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public FunEventType FunEventType { get; set; }
    }
}
