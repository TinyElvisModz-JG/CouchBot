using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTD.CouchBot.Domain.Models.VidMe
{
    public class VidMeUserVideos
    { 
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
            [JsonProperty("video_views")]
            public string VideoViews { get; set; }
            [JsonProperty("videos_scores")]
            public int VideosScores { get; set; }
            [JsonProperty("comments_scores")]
            public int CommentsScores { get; set; }
            [JsonProperty("bio")]
            public string Bio { get; set; }
            [JsonProperty("is_following")]
            public bool IsFollowing { get; set; }
            [JsonProperty("is_followed_by")]
            public bool IsFollowerBy { get; set; }
            [JsonProperty("receive_notifications_following")]
            public bool ReceiveNotificationsFollowing{ get; set; }
            [JsonProperty("receive_notifications_followed")]
            public bool ReceiveNotificationsFollowed { get; set; }
            [JsonProperty("is_subscribed")]
            public bool IsSubscribed { get; set; }
            [JsonProperty("is_subscribed_by")]
            public bool IsSubscribedBy { get; set; }
            [JsonProperty("disallow_downloads")]
            public bool DisallowDownloads { get; set; }
            [JsonProperty("vip")]
            public bool Vip { get; set; }
        }

        public class Channel
        {
            [JsonProperty("channel_id")]
            public string channelId { get; set; }
            [JsonProperty("url")]
            public string Url { get; set; }
            [JsonProperty("title")]
            public string Title { get; set; }
            [JsonProperty("description")]
            public string Description { get; set; }
            [JsonProperty("date_created")]
            public string DateCreated { get; set; }
            [JsonProperty("is_default")]
            public bool IsDefault { get; set; }
            [JsonProperty("hide_suggest")]
            public bool HideSuggest { get; set; }
            [JsonProperty("show_unmoderated")]
            public bool ShowUnmoderated { get; set; }
            [JsonProperty("nsfw")]
            public bool Nsfw { get; set; }
            [JsonProperty("follower_count")]
            public int FollowerCount { get; set; }
            [JsonProperty("video_count")]
            public int VideoCount { get; set; }
            [JsonProperty("full_url")]
            public string FullUrl { get; set; }
            [JsonProperty("avatar_url")]
            public string AvatarUrl { get; set; }
            [JsonProperty("avatar_ai")]
            public string AvatarAi { get; set; }
            [JsonProperty("cover_url")]
            public string CoverUrl { get; set; }
            [JsonProperty("cover_ai")]
            public string CoverAi { get; set; }
        }

        public class Format
        {
            [JsonProperty("type")]
            public string Type { get; set; }
            [JsonProperty("uri")]
            public string Uri { get; set; }
            [JsonProperty("width")]
            public object Width { get; set; }
            [JsonProperty("height")]
            public object Height { get; set; }
            [JsonProperty("version")]
            public int Version { get; set; }
        }

        public class Video
        {
            [JsonProperty("video_id")]
            public string VideoId { get; set; }
            [JsonProperty("url")]
            public string Url { get; set; }
            [JsonProperty("full_url")]
            public string FullUrl { get; set; }
            [JsonProperty("embed_url")]
            public string EmbedUrl { get; set; }
            [JsonProperty("user_id")]
            public string UserId { get; set; }
            [JsonProperty("state")]
            public string State { get; set; }
            [JsonProperty("title")]
            public string Title { get; set; }
            [JsonProperty("description")]
            public string Description { get; set; }
            [JsonProperty("duration")]
            public double Duration { get; set; }
            [JsonProperty("height")]
            public int Height { get; set; }
            [JsonProperty("width")]
            public int Width { get; set; }
            [JsonProperty("date_created")]
            public string DateCreated { get; set; }
            [JsonProperty("date_stored")]
            public string DateStored { get; set; }
            [JsonProperty("date_completed")]
            public string DateCompleted { get; set; }
            [JsonProperty("comment_count")]
            public int CommentCount { get; set; }
            [JsonProperty("view_count")]
            public int ViewCount { get; set; }
            [JsonProperty("share_count")]
            public int ShareCount { get; set; }
            [JsonProperty("version")]
            public int Version { get; set; }
            [JsonProperty("nsfw")]
            public bool Nsfw { get; set; }
            [JsonProperty("thumbnail")]
            public string Thumbnail { get; set; }
            [JsonProperty("thumbnail_url")]
            public string ThumbnailUrl { get; set; }
            [JsonProperty("thumbnail_gif")]
            public object ThumbnailGif { get; set; }
            [JsonProperty("thumbnail_gif_url")]
            public object ThumbnailGifUrl { get; set; }
            [JsonProperty("thumbnail_ai")]
            public string ThumbnailAi { get; set; }
            [JsonProperty("storyboard")]
            public string Storyboard { get; set; }
            [JsonProperty("score")]
            public int Score { get; set; }
            [JsonProperty("likes_count")]
            public int LikesCount { get; set; }
            [JsonProperty("channel_id")]
            public string ChannelId { get; set; }
            [JsonProperty("source")]
            public string Source { get; set; }
            [JsonProperty("@private")]
            public bool Private { get; set; }
            [JsonProperty("scheduled")]
            public bool Scheduled { get; set; }
            [JsonProperty("subscribed_only")]
            public bool SubscribedOnly { get; set; }
            [JsonProperty("date_published")]
            public string DatePublished { get; set; }
            [JsonProperty("latitude")]
            public int Latitude { get; set; }
            [JsonProperty("longitude")]
            public int Longitude { get; set; }
            [JsonProperty("place_id")]
            public object PlaceId { get; set; }
            [JsonProperty("place_name")]
            public object PlaceName { get; set; }
            [JsonProperty("colors")]
            public string Colors { get; set; }
            [JsonProperty("reddit_link")]
            public object RedditLink { get; set; }
            [JsonProperty("youtube_override_source")]
            public object YoutubeOverrideSource { get; set; }
            [JsonProperty("complete")]
            public string Complete { get; set; }
            [JsonProperty("complete_url")]
            public string CompleteUrl { get; set; }
            [JsonProperty("watching_count")]
            public int WatchingCount { get; set; }
            [JsonProperty("hot_score")]
            public double HotScore { get; set; }
            [JsonProperty("user")]
            public User User { get; set; }
            [JsonProperty("channel")]
            public Channel Channel { get; set; }
            [JsonProperty("formats")]
            public List<Format> Formats { get; set; }
            [JsonProperty("total_tipped")]
            public int? TotalTipped { get; set; }
        }

        [JsonProperty("status")]
        public bool Status { get; set; }
        [JsonProperty("page")]
        public Page Page { get; set; }
        [JsonProperty("videos")]
        public List<Video> Videos { get; set; }
    }
}
