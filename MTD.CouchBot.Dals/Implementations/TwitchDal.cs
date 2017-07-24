using MTD.CouchBot.Domain;
using MTD.CouchBot.Domain.Models.Twitch;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MTD.CouchBot.Dals.Implementations
{
    public class TwitchDal : ITwitchDal
    {
        public async Task<TwitchStream> GetStreamById(string twitchId)
        {
            var request = (HttpWebRequest)WebRequest.Create("https://api.twitch.tv/kraken/streams/" + twitchId);
            request.Headers["Client-Id"] = Constants.TwitchClientId;
            request.Accept = "application/vnd.twitchtv.v5+json";
            var response = await request.GetResponseAsync();
            var responseText = "";

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                responseText = sr.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<TwitchStream>(responseText);
        }

        public async Task<TwitchStreams> GetStreamsByIdList(string twitchIdList)
        {
            var request = (HttpWebRequest)WebRequest.Create("https://api.twitch.tv/kraken/streams?channel=" + twitchIdList + "&api_version=5");
            request.Headers["Client-Id"] = Constants.TwitchClientId;
            request.Accept = "application/vnd.twitchtv.v5+json";
            var response = await request.GetResponseAsync();
            var responseText = "";

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                responseText = sr.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<TwitchStreams>(responseText);
        }

        public async Task<string> GetTwitchIdByLogin(string name)
        {
            var request = (HttpWebRequest)WebRequest.Create("https://api.twitch.tv/kraken/users?login=" + name + "&api_version=5");
            request.Headers["Client-Id"] = Constants.TwitchClientId;
            request.Accept = "application/vnd.twitchtv.v5+json";
            var response = await request.GetResponseAsync();
            var responseText = "";

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                responseText = sr.ReadToEnd();
            }

            var users = JsonConvert.DeserializeObject<TwitchUser>(responseText);

            if(users != null && users.Users != null && users.Users.Count > 0)
            {
                return users.Users[0].Id;
            }
            else
            {
                return null;
            }
        }

        public async Task<TwitchChannelFeed> GetChannelFeedPosts(string twitchId)
        {
            var request = (HttpWebRequest)WebRequest.Create("https://api.twitch.tv/kraken/feed/" + twitchId + "/posts?limit=5&api_version=5");
            request.Headers["Client-Id"] = Constants.TwitchClientId;
            request.Accept = "application/vnd.twitchtv.v5+json";
            var response = await request.GetResponseAsync();
            var responseText = "";

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                responseText = sr.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<TwitchChannelFeed>(responseText);
        }

        public async Task<TwitchTeam> GetTwitchTeamByName(string name)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create("https://api.twitch.tv/kraken/teams/" + name);
                request.Headers["Client-Id"] = Constants.TwitchClientId;
                request.Accept = "application/vnd.twitchtv.v5+json";
                var response = await request.GetResponseAsync();
                var responseText = "";

                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    responseText = sr.ReadToEnd();
                }

                return JsonConvert.DeserializeObject<TwitchTeam>(responseText);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> GetDelimitedListOfTwitchMemberIds(string teamToken)
        {
            var team = await GetTwitchTeamByName(teamToken).ConfigureAwait(false);

            return team == null ? null : string.Join(",", team.Users.Select(u => u.Id));
        }

        public async Task<List<Domain.Models.Twitch.Stream>> GetStreamsByGameName(string gameName)
        {
            List<Domain.Models.Twitch.Stream> streams = new List<Domain.Models.Twitch.Stream>();

            var offset = 0;
            while (true)
            {
                var request = (HttpWebRequest)WebRequest.Create("https://api.twitch.tv/kraken/streams/?game=" + gameName + "&limit=100&stream_type=live&offset=" + offset);
                request.Headers["Client-Id"] = Constants.TwitchClientId;
                request.Accept = "application/vnd.twitchtv.v5+json";
                var response = await request.GetResponseAsync();
                var responseText = "";

                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    responseText = sr.ReadToEnd();
                }

                var streamResponse = JsonConvert.DeserializeObject<TwitchStreams>(responseText);

                if (streamResponse.Streams.Count < 1)
                {
                    break;
                }

                streams.AddRange(streamResponse.Streams);
                offset += 100;
            }

            return streams;
        }

        public async Task<TwitchGameSearchResponse> SearchForGameByName(string gameName)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create("https://api.twitch.tv/kraken/search/games?query=" + gameName);
                request.Headers["Client-Id"] = Constants.TwitchClientId;
                request.Accept = "application/vnd.twitchtv.v5+json";
                var response = await request.GetResponseAsync();
                var responseText = "";

                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    responseText = sr.ReadToEnd();
                }

                return JsonConvert.DeserializeObject<TwitchGameSearchResponse>(responseText);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
