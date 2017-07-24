using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTD.CouchBot.Domain.Models.VidMe
{
    public class Page
    {
        [JsonProperty("limit")]
        public int Limit { get; set; }
        [JsonProperty("offset")]
        public int Offset { get; set; }
        [JsonProperty("total")]
        public int Total { get; set; }
    }
}
