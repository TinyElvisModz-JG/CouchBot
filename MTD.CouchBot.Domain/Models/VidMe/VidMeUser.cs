using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTD.CouchBot.Domain.Models.VidMe
{
    public class VidMeUser
    {
        [JsonProperty("status")]
        public bool Status { get; set; }
        [JsonProperty("user")]
        public User User_ { get; set; }
        [JsonProperty("watchers")]
        public Watchers Watchers_ { get; set; }

        public class User
        {
            [JsonProperty("user_id")]
            public string UserId { get; set; }
            [JsonProperty("username")]
            public string Username { get; set; }
            [JsonProperty("full_url")]
            public string FullUrl { get; set; }
            [JsonProperty("avatar")]
            public string Avatar { get; set; }
            [JsonProperty("avatar_url")]
            public string AvatarUrl { get; set; }
            [JsonProperty("avatar_ai")]
            public string AvatarAi { get; set; }
            [JsonProperty("cover")]
            public string Cover { get; set; }
            [JsonProperty("cover_url")]
            public string CoverUrl { get; set; }
            [JsonProperty("cover_ai")]
            public string CoverAi { get; set; }
            [JsonProperty("displayname")]
            public string DisplayName { get; set; }
            [JsonProperty("follower_count")]
            public int FollowerCount { get; set; }
            [JsonProperty("likes_count")]
            public string LikesCount { get; set; }
            [JsonProperty("video_count")]
            public int VideoCount { get; set; }
            [JsonProperty("watching_count")]
            public int WatchingCount { get; set; }
            [JsonProperty("video_views")]
            public string VideoViews { get; set; }
            [JsonProperty("videos_scores")]
            public int VideosScores { get; set; }
            [JsonProperty("comments_scores")]
            public int CommentsScores { get; set; }
            [JsonProperty("bio")]
            public string Bio { get; set; }
            [JsonProperty("disallow_downloads")]
            public bool DisallowDownloads { get; set; }
        }

        public class Watchers
        {
            [JsonProperty("total")]
            public int Total { get; set; }
            [JsonProperty("countries")]
            public List<string> Countries { get; set; }
        }
    }
}
