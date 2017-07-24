using Newtonsoft.Json;

namespace MTD.CouchBot.Domain.Models.ApiAi
{
    public class ApiAiResponse
    {    
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
        [JsonProperty("lang")]
        public string Lang { get; set; }
        [JsonProperty("result")]
        public Result Result { get; set; }
        [JsonProperty("status")]
        public Status Status { get; set; }
        [JsonProperty("sessionId")]
        public string SessionId { get; set; }
    }
}
