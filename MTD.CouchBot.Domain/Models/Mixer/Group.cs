using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTD.CouchBot.Domain.Models.Mixer
{
    public class Group
    {
        [JsonProperty("id")]
        public int? Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
