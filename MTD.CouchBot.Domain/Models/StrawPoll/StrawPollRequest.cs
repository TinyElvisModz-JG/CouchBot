using System.Collections.Generic;

namespace MTD.CouchBot.Domain.Models.StrawPoll
{
    public class StrawPollRequest
    {
        public string title { get; set; }
        public List<string> options { get; set; }
        public bool multi { get; set; }
    }
}
