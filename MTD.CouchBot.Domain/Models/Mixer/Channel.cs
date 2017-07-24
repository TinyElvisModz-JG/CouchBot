using Newtonsoft.Json;

namespace MTD.CouchBot.Domain.Models.Mixer
{
    public class Channel
    {
        [JsonProperty("id")]
        public int? Id { get; set; }
        [JsonProperty("userId")]
        public int? UserId { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("online")]
        public bool Online { get; set; }
        [JsonProperty("featured")]
        public bool Featured { get; set; }
        [JsonProperty("partnered")]
        public bool Partnered { get; set; }
        [JsonProperty("transcodingProfileId")]
        public int? TranscodingProfileId { get; set; }
        [JsonProperty("suspended")]
        public bool Suspended { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("audience")]
        public string Audience { get; set; }
        [JsonProperty("viewersTotal")]
        public int ViewersTotal { get; set; }
        [JsonProperty("viewersCurrent")]
        public int ViewersCurrent { get; set; }
        [JsonProperty("numFollowers")]
        public int NumFollowers { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("typeId")]
        public int? TypeId { get; set; }
        [JsonProperty("interactive")]
        public bool Interactive { get; set; }
        [JsonProperty("interactiveGameId")]
        public int? InteractiveGameId { get; set; }
        [JsonProperty("ftl")]
        public int Ftl { get; set; }
        [JsonProperty("hasVod")]
        public bool HasVod { get; set; }
        [JsonProperty("languageId")]
        public object LanguageId { get; set; }
        [JsonProperty("coverId")]
        public int? CoverId { get; set; }
        [JsonProperty("thumbnailId")]
        public object ThumbnailId { get; set; }
        [JsonProperty("badgeId")]
        public object BadgeId { get; set; }
        [JsonProperty("hosteeId")]
        public object HosteeId { get; set; }
        [JsonProperty("hasTranscodes")]
        public bool HasTranscodes { get; set; }
        [JsonProperty("vodsEnabled")]
        public bool VodsEnabled { get; set; }
        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }
        [JsonProperty("updatedAt")]
        public string UpdatedAt { get; set; }
        [JsonProperty("deletedAt")]
        public object DeletedAt { get; set; }
    }
}
