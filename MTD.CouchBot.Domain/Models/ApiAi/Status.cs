using Newtonsoft.Json;

namespace MTD.CouchBot.Domain.Models.ApiAi
{
    public class Status
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("errorType")]
        public string ErrorType { get; set; }
    }
}
