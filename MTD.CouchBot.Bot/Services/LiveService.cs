using MTD.CouchBot.Domain;
using MTD.CouchBot.Domain.Models.Bot;
using MTD.CouchBot.Domain.Models.Twitch;
using MTD.CouchBot.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTD.CouchBot.Bot.Services
{
    public class LiveService
    {
        private readonly IGuildManager _guildManager;
        private readonly LoggingService _logging;
        private readonly ITwitchManager _twitchManager;
        private readonly ILiveQueueManager _liveQueueManager;

        public LiveService(IGuildManager guildManager, LoggingService logging, ITwitchManager twitchManager, ILiveQueueManager liveQueueManager)
        {
            _guildManager = guildManager;
            _logging = logging;
            _twitchManager = twitchManager;
            _liveQueueManager = liveQueueManager;
        }

        public async Task CheckTwitchLive()
        {
            var servers = await _guildManager.GetGuildsForLive();
            var liveChannels = await _liveQueueManager.GetLiveChannelsByPlatformId(Domain.Enumerations.Platform.Twitch);

            // Loop through servers to broadcast.
            foreach (var server in servers)
            {
                if (!server.AllowLive)
                {
                    continue;
                }

                if (server.Id != 0 && ulong.Parse(server.LiveChannel) != 0 &&
                    server.ServerTwitchChannels != null && server.ServerTwitchChannelIds != null)
                {
                    TwitchStreams streams = null;

                    try
                    {
                        // Query Twitch for our stream.
                        streams = await _twitchManager.GetStreamsByIdList(server.ServerTwitchChannelIds);
                    }
                    catch (Exception wex)
                    {
                        // Log our error and move to the next user.

                        _logging.LogError("Twitch Server Error: " + wex.Message + " in Discord Server Id: " + server.Id);
                        continue;
                    }

                    if (streams == null || streams.Streams == null || streams.Streams.Count < 1)
                    {
                        continue;
                    }

                    foreach (var stream in streams.Streams)
                    {
                        // Get currently live channel from Live/Twitch, if it exists.
                        var channel = liveChannels.FirstOrDefault(x => x.Name == stream.Channel._id.ToString());

                        if (stream != null)
                        {
                            var chat = await DiscordHelper.GetMessageChannel(server.Id, server.GoLiveChannel);

                            if (chat == null)
                            {
                                continue;
                            }

                            bool checkChannelBroadcastStatus = channel == null || !channel.Servers.Contains(server.Id);
                            bool checkGoLive = !string.IsNullOrEmpty(server.LiveChannel) && ulong.Parse(server.LiveChannel) != 0;

                            if (checkChannelBroadcastStatus)
                            {
                                if (checkGoLive)
                                {
                                    if (channel == null)
                                    {
                                        channel = new LiveChannel()
                                        {
                                            Name = stream.Channel.Id.ToString(),
                                            Servers = new List<ulong>()
                                        };

                                        channel.Servers.Add(server.Id);

                                        liveChannels.Add(channel);
                                    }
                                    else
                                    {
                                        channel.Servers.Add(server.Id);
                                    }

                                    // Build our message
                                    string url = stream.Channel.Url;
                                    string channelName = StringUtilities.ScrubChatMessage(stream.Channel.display_name);
                                    string avatarUrl = stream.Channel.logo != null ? stream.Channel.logo : "https://static-cdn.jtvnw.net/jtv_user_pictures/xarth/404_user_70x70.png";
                                    string thumbnailUrl = stream.preview.large;

                                    var message = await MessagingHelper.BuildMessage(channelName, stream.game, stream.Channel.status, url, avatarUrl,
                                        thumbnailUrl, Constants.Twitch, stream.Channel._id.ToString(), server, server.GoLiveChannel, null);

                                    var finalCheck = BotFiles.GetCurrentlyLiveTwitchChannels().FirstOrDefault(x => x.Name == stream.Channel._id.ToString());

                                    if (finalCheck == null || !finalCheck.Servers.Contains(server.Id))
                                    {
                                        if (channel.ChannelMessages == null)
                                            channel.ChannelMessages = new List<ChannelMessage>();

                                        channel.ChannelMessages.AddRange(await MessagingHelper.SendMessages(Constants.Twitch, new List<BroadcastMessage>() { message }));

                                        File.WriteAllText(Constants.ConfigRootDirectory + Constants.LiveDirectory + Constants.TwitchDirectory + stream.Channel._id.ToString() + ".json",
                                            JsonConvert.SerializeObject(channel));

                                        _logging.LogTwitch(channelName + " has gone online.");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public async Task CheckOwnerTwitchLive()
        {
            var servers = await _guildManager.GetGuildsForLive();
            var liveChannels = BotFiles.GetCurrentlyLiveTwitchChannels();

            // Loop through servers to broadcast.
            foreach (var server in servers)
            {
                if (!server.AllowLive)
                {
                    continue;
                }

                if (server.Id != 0 && server.OwnerLiveChannel != 0 &&
                    !string.IsNullOrEmpty(server.OwnerTwitchChannel) && !string.IsNullOrEmpty(server.OwnerTwitchChannelId))
                {
                    TwitchStreamV5 twitchStream = null;

                    try
                    {
                        // Query Twitch for our stream.
                        twitchStream = await _twitchManager.GetStreamById(server.OwnerTwitchChannelId);
                    }
                    catch (Exception wex)
                    {
                        // Log our error and move to the next user.

                        _logging.LogError("Twitch Server Error: " + wex.Message + " in Discord Server Id: " + server.Id);
                        continue;
                    }

                    if (twitchStream == null || twitchStream.stream == null)
                    {
                        continue;
                    }

                    var stream = twitchStream.stream;

                    // Get currently live channel from Live/Twitch, if it exists.
                    var channel = liveChannels.FirstOrDefault(x => x.Name == stream._id.ToString());

                    if (stream != null)
                    {
                        var chat = await DiscordHelper.GetMessageChannel(server.Id, server.OwnerLiveChannel);

                        if (chat == null)
                        {
                            continue;
                        }

                        bool checkChannelBroadcastStatus = channel == null || !channel.Servers.Contains(server.Id);
                        bool checkGoLive = !string.IsNullOrEmpty(server.OwnerLiveChannel.ToString()) && server.OwnerLiveChannel != 0;

                        if (checkChannelBroadcastStatus)
                        {
                            if (checkGoLive)
                            {
                                if (channel == null)
                                {
                                    channel = new LiveChannel()
                                    {
                                        Name = stream.Channel._id.ToString(),
                                        Servers = new List<ulong>()
                                    };

                                    channel.Servers.Add(server.Id);

                                    liveChannels.Add(channel);
                                }
                                else
                                {
                                    channel.Servers.Add(server.Id);
                                }

                                // Build our message
                                string url = stream.Channel.url;
                                string channelName = StringUtilities.ScrubChatMessage(stream.Channel.display_name);
                                string avatarUrl = stream.Channel.logo != null ? stream.Channel.logo : "https://static-cdn.jtvnw.net/jtv_user_pictures/xarth/404_user_70x70.png";
                                string thumbnailUrl = stream.preview.large;

                                var message = await MessagingHelper.BuildMessage(channelName, stream.game, stream.Channel.status, url, avatarUrl,
                                    thumbnailUrl, Constants.Twitch, stream.Channel._id.ToString(), server, server.OwnerLiveChannel, null);

                                var finalCheck = BotFiles.GetCurrentlyLiveTwitchChannels().FirstOrDefault(x => x.Name == stream.Channel._id.ToString());

                                if (finalCheck == null || !finalCheck.Servers.Contains(server.Id))
                                {
                                    if (channel.ChannelMessages == null)
                                        channel.ChannelMessages = new List<ChannelMessage>();

                                    channel.ChannelMessages.AddRange(await MessagingHelper.SendMessages(Constants.Twitch, new List<BroadcastMessage>() { message }));

                                    File.WriteAllText(Constants.ConfigRootDirectory + Constants.LiveDirectory + Constants.TwitchDirectory + stream.Channel._id.ToString() + ".json",
                                        JsonConvert.SerializeObject(channel));

                                    _logging.LogTwitch(channelName + " has gone online.");
                                }
                            }
                        }
                    }
                }
            }
        }

        public async Task CheckTwitchChannelFeeds()
        {
            // Nothing.
        }

        public async Task CheckTwitchOwnerChannelFeeds()
        {
            var servers = await _guildManager.GetGuildsForLive();

            // Loop through servers to broadcast.
            foreach (var server in servers)
            {
                if (!server.AllowOwnerChannelFeed)
                {
                    continue;
                }

                if (server.Id != 0 && server.OwnerTwitchFeedChannel != 0 &&
                    !string.IsNullOrEmpty(server.OwnerTwitchChannel) && !string.IsNullOrEmpty(server.OwnerTwitchChannelId))
                {
                    TwitchChannelFeed feed = null;

                    try
                    {
                        feed = await _twitchManager.GetChannelFeedPosts(server.OwnerTwitchChannelId);
                    }
                    catch (Exception wex)
                    {
                        _logging.LogError("Twitch Server Error: " + wex.Message + " in Discord Server Id: " + server.Id);
                        continue;
                    }

                    if (feed == null || feed.posts == null || feed.posts.Count == 0)
                    {
                        continue;
                    }

                    var chat = await DiscordHelper.GetMessageChannel(server.Id, server.OwnerTwitchFeedChannel);

                    if (chat == null)
                    {
                        continue;
                    }

                    foreach (var post in feed.posts)
                    {
                        DateTime now = DateTime.UtcNow;
                        DateTime created = DateTime.Parse(post.created_at).ToUniversalTime();
                        TimeSpan diff = now - created;

                        if (diff.TotalMinutes <= 2)
                        {
                            string message = "**[New Channel Feed Update - " + created.AddHours(server.TimeZoneOffset).ToString("MM/dd/yyyy hh:mm tt") + "]**\r\n" +
                                post.body + "\r\n\r\n" +
                                "<https://twitch.tv/" + server.OwnerTwitchChannel + "/p/" + post.id + ">";

                            try
                            {
                                await chat.SendMessageAsync(message);
                            }
                            catch (Exception wex)
                            {
                                _logging.LogError("Twitch Channel Feed Error: " + wex.Message + " for user: " + server.OwnerTwitchChannel + " in server: " + server.Id);
                            }

                            _logging.LogTwitch(server.OwnerTwitchChannel + " posted a new channel feed message.");
                        }
                    }
                }
            }
        }

        public async Task CheckTwitchTeams()
        {
            var servers = await _guildManager.GetGuildsForLive();
            var liveChannels = BotFiles.GetCurrentlyLiveTwitchChannels();

            // Loop through servers to broadcast.
            foreach (var server in servers)
            {
                if (!server.AllowLive)
                {
                    continue;
                }

                if (server.Id != 0 && server.GoLiveChannel != 0 &&
                    server.TwitchTeams != null && server.TwitchTeams.Count > 0)
                {

                    if (server.TwitchTeams == null)
                    {
                        continue;
                    }

                    foreach (var team in server.TwitchTeams)
                    {
                        var userList = await _twitchManager.GetDelimitedListOfTwitchMemberIds(team);
                        var teamResponse = await _twitchManager.GetTwitchTeamByName(team);

                        TwitchStreamsV5 streams = null;

                        try
                        {
                            // Query Twitch for our stream.
                            streams = await _twitchManager.GetStreamsByIdList(userList);
                        }
                        catch (Exception wex)
                        {
                            // Log our error and move to the next user.

                            _logging.LogError("Twitch Team Server Error: " + wex.Message + " in Discord Server Id: " + server.Id);
                            continue;
                        }

                        if (streams == null || streams.Streams == null || streams.Streams.Count < 1)
                        {
                            continue;
                        }

                        foreach (var stream in streams.Streams)
                        {
                            // Get currently live channel from Live/Twitch, if it exists.
                            var channel = liveChannels.FirstOrDefault(x => x.Name == stream.Channel._id.ToString());

                            if (stream != null)
                            {
                                var chat = await DiscordHelper.GetMessageChannel(server.Id, server.GoLiveChannel);

                                if (chat == null)
                                {
                                    continue;
                                }

                                bool checkChannelBroadcastStatus = channel == null || !channel.Servers.Contains(server.Id);
                                bool checkGoLive = !string.IsNullOrEmpty(server.GoLiveChannel.ToString()) && server.GoLiveChannel != 0;

                                if (checkChannelBroadcastStatus)
                                {
                                    if (checkGoLive)
                                    {
                                        if (channel == null)
                                        {
                                            channel = new LiveChannel()
                                            {
                                                Name = stream.Channel._id.ToString(),
                                                Servers = new List<ulong>()
                                            };

                                            channel.Servers.Add(server.Id);

                                            liveChannels.Add(channel);
                                        }
                                        else
                                        {
                                            channel.Servers.Add(server.Id);
                                        }

                                        // Build our message
                                        string url = stream.Channel.url;
                                        string channelName = StringUtilities.ScrubChatMessage(stream.Channel.display_name);
                                        string avatarUrl = stream.Channel.logo != null ? stream.Channel.logo : "https://static-cdn.jtvnw.net/jtv_user_pictures/xarth/404_user_70x70.png";
                                        string thumbnailUrl = stream.preview.large;

                                        var message = await MessagingHelper.BuildMessage(channelName, stream.game, stream.Channel.status, url, avatarUrl,
                                            thumbnailUrl, Constants.Twitch, stream.Channel._id.ToString(), server, server.GoLiveChannel, teamResponse.DisplayName);

                                        var finalCheck = BotFiles.GetCurrentlyLiveTwitchChannels().FirstOrDefault(x => x.Name == stream.Channel._id.ToString());

                                        if (finalCheck == null || !finalCheck.Servers.Contains(server.Id))
                                        {
                                            if (channel.ChannelMessages == null)
                                                channel.ChannelMessages = new List<ChannelMessage>();

                                            channel.ChannelMessages.AddRange(await MessagingHelper.SendMessages(Constants.Twitch, new List<BroadcastMessage>() { message }));

                                            File.WriteAllText(Constants.ConfigRootDirectory + Constants.LiveDirectory + Constants.TwitchDirectory + stream.Channel._id.ToString() + ".json",
                                                JsonConvert.SerializeObject(channel));

                                            _logging.LogTwitch(teamResponse.Name + " team member " + channelName + " has gone online.");
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }
        }

        public async Task CheckTwitchGames()
        {
            var servers = BotFiles.GetConfiguredServersWithLiveChannel();
            var liveChannels = BotFiles.GetCurrentlyLiveTwitchChannels();
            var gameList = new List<TwitchGameServerModel>();

            foreach (var s in servers)
            {
                if (s.ServerGameList == null)
                {
                    continue;
                }

                foreach (var g in s.ServerGameList)
                {
                    var gameServerModel = gameList.FirstOrDefault(x => x.Name.Equals(g, StringComparison.CurrentCultureIgnoreCase));

                    if (gameServerModel == null)
                    {
                        gameList.Add(new TwitchGameServerModel() { Name = g, Servers = new List<ulong>() { s.Id } });
                    }
                    else
                    {
                        gameServerModel.Servers.Add(s.Id);
                    }
                }
            }

            foreach (var game in gameList)
            {
                List<TwitchStreamsV5.Stream> gameResponse = null;

                try
                {
                    // Query Twitch for our stream.
                    gameResponse = await _twitchManager.GetStreamsByGameName(game.Name);
                }
                catch (Exception wex)
                {
                    // Log our error and move to the next user.

                    _logging.LogError("Twitch Game Error: " + wex.Message);
                    continue;
                }

                if (gameResponse == null || gameResponse.Count == 0)
                {
                    continue;
                }

                int count = 0;

                foreach (var stream in gameResponse)
                {
                    if (count >= 5)
                    {
                        continue;
                    }

                    DateTime now = DateTime.UtcNow;
                    DateTime created = DateTime.Parse(stream.created_at).ToUniversalTime();
                    TimeSpan diff = now - created;
                    var interval = ((Constants.TwitchInterval / 1000) / 60);

                    if (diff.TotalMinutes > interval)
                    {
                        continue;
                    }

                    foreach (var s in game.Servers)
                    {
                        var server = BotFiles.GetConfiguredServerById(s);

                        var channel = liveChannels.FirstOrDefault(x => x.Name == stream.Channel._id.ToString());

                        var chat = await DiscordHelper.GetMessageChannel(server.Id, server.GoLiveChannel);

                        if (chat == null)
                        {
                            continue;
                        }

                        bool checkChannelBroadcastStatus = channel == null || !channel.Servers.Contains(server.Id);

                        if (checkChannelBroadcastStatus)
                        {
                            if (channel == null)
                            {
                                channel = new LiveChannel()
                                {
                                    Name = stream.Channel._id.ToString(),
                                    Servers = new List<ulong>()
                                };

                                channel.Servers.Add(server.Id);

                                liveChannels.Add(channel);
                            }
                            else
                            {
                                channel.Servers.Add(server.Id);
                            }

                            // Build our message
                            string url = stream.Channel.url;
                            string channelName = StringUtilities.ScrubChatMessage(stream.Channel.display_name);
                            string avatarUrl = stream.Channel.logo != null ? stream.Channel.logo : "https://static-cdn.jtvnw.net/jtv_user_pictures/xarth/404_user_70x70.png";
                            string thumbnailUrl = stream.preview.large;

                            var message = await MessagingHelper.BuildMessage(channelName, stream.game, stream.Channel.status, url, avatarUrl,
                                thumbnailUrl, Constants.Twitch, stream.Channel._id.ToString(), server, server.GoLiveChannel, null);

                            var finalCheck = BotFiles.GetCurrentlyLiveTwitchChannels().FirstOrDefault(x => x.Name == stream.Channel._id.ToString());

                            if (finalCheck == null || !finalCheck.Servers.Contains(server.Id))
                            {
                                if (channel.ChannelMessages == null)
                                    channel.ChannelMessages = new List<ChannelMessage>();

                                channel.ChannelMessages.AddRange(await MessagingHelper.SendMessages(Constants.Twitch, new List<BroadcastMessage>() { message }));

                                File.WriteAllText(Constants.ConfigRootDirectory + Constants.LiveDirectory + Constants.TwitchDirectory + stream.Channel._id.ToString() + ".json",
                                    JsonConvert.SerializeObject(channel));

                                _logging.LogTwitch(channelName + " has gone live playing " + game.Name);
                            }
                        }
                    }

                    count++;
                }
            }
        }

        public async Task CheckYouTubeLive()
        {
            var servers = BotFiles.GetConfiguredServersWithLiveChannel();
            var liveChannels = BotFiles.GetCurrentlyLiveYouTubeChannels();
            var youTubeChannelList = new List<YouTubeChannelServerModel>();

            foreach (var server in servers)
            {
                if (!server.AllowLive)
                {
                    continue;
                }

                if (server.ServerYouTubeChannelIds == null)
                {
                    continue;
                }

                foreach (var c in server.ServerYouTubeChannelIds)
                {
                    var channelServerModel = youTubeChannelList.FirstOrDefault(x => x.YouTubeChannelId.Equals(c, StringComparison.CurrentCultureIgnoreCase));

                    if (channelServerModel == null)
                    {
                        youTubeChannelList.Add(new YouTubeChannelServerModel { YouTubeChannelId = c, Servers = new List<ulong> { server.Id } });
                    }
                    else
                    {
                        channelServerModel.Servers.Add(server.Id);
                    }
                }
            }

            foreach (var c in youTubeChannelList)
            {
                YouTubeSearchListChannel streamResult = null;

                try
                {
                    // Query Youtube for our stream.
                    streamResult = await youtubeManager.GetLiveVideoByChannelId(c.YouTubeChannelId);
                }
                catch (Exception wex)
                {
                    // Log our error and move to the next user.

                    _logging.LogError("YouTube Error: " + wex.Message + " for user: " + c.YouTubeChannelId);
                    continue;
                }

                if (streamResult != null && streamResult.items.Count > 0)
                {
                    var stream = streamResult.items[0];

                    foreach (var s in c.Servers)
                    {
                        var server = BotFiles.GetConfiguredServerById(s);
                        var channel = liveChannels.FirstOrDefault(x => x.Name.ToLower() == c.YouTubeChannelId.ToLower());
                        bool allowEveryone = server.AllowEveryone;
                        var chat = await DiscordHelper.GetMessageChannel(server.Id, server.GoLiveChannel);

                        if (chat == null)
                        {
                            continue;
                        }

                        if (channel == null)
                        {
                            channel = new LiveChannel()
                            {
                                Name = c.YouTubeChannelId,
                                Servers = new List<ulong>()
                            };

                            channel.Servers.Add(server.Id);

                            liveChannels.Add(channel);
                        }
                        else
                        {
                            channel.Servers.Add(server.Id);
                        }

                        // Build our message
                        YouTubeChannelSnippet channelData = null;

                        try
                        {
                            channelData = await youtubeManager.GetYouTubeChannelSnippetById(stream.snippet.ChannelId);
                        }
                        catch (Exception wex)
                        {
                            // Log our error and move to the next user.

                            _logging.LogError("YouTube Error: " + wex.Message + " for user: " + c.YouTubeChannelId);
                            continue;
                        }

                        if (channelData == null)
                        {
                            continue;
                        }

                        string url = "http://" + (server.UseYouTubeGamingPublished ? "gaming" : "www") + ".youtube.com/watch?v=" + stream.id;
                        string channelTitle = stream.snippet.ChannelTitle;
                        string avatarUrl = channelData.items.Count > 0 ? channelData.items[0].snippet.thumbnails.high.url : "";
                        string thumbnailUrl = stream.snippet.thumbnails.high.url;

                        var message = await MessagingHelper.BuildMessage(channelTitle, "a game", stream.snippet.title, url, avatarUrl, thumbnailUrl,
                            Constants.YouTubeGaming, c.YouTubeChannelId, server, server.GoLiveChannel, null);

                        var finalCheck = BotFiles.GetCurrentlyLiveYouTubeChannels().FirstOrDefault(x => x.Name == c.YouTubeChannelId);

                        if (finalCheck == null || !finalCheck.Servers.Contains(server.Id))
                        {
                            if (channel.ChannelMessages == null)
                                channel.ChannelMessages = new List<ChannelMessage>();

                            channel.ChannelMessages.AddRange(await MessagingHelper.SendMessages(Constants.YouTubeGaming, new List<BroadcastMessage>() { message }));

                            File.WriteAllText(Constants.ConfigRootDirectory + Constants.LiveDirectory + Constants.YouTubeDirectory + c.YouTubeChannelId + ".json", JsonConvert.SerializeObject(channel));

                            _logging.LogYouTubeGaming(channelTitle + " has gone online.");
                        }
                    }
                }
            }
        }

        public async Task CheckOwnerYouTubeLive()
        {
            var servers = BotFiles.GetConfiguredServersWithOwnerLiveChannel();
            var liveChannels = BotFiles.GetCurrentlyLiveYouTubeChannels();
            var youTubeChannelList = new List<YouTubeChannelServerModel>();

            foreach (var server in servers)
            {
                if (!server.AllowLive)
                {
                    continue;
                }

                if (server.ServerYouTubeChannelIds == null)
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(server.OwnerYouTubeChannelId))
                {
                    var channelServerModel = youTubeChannelList.FirstOrDefault(x => x.YouTubeChannelId.Equals(server.OwnerYouTubeChannelId, StringComparison.CurrentCultureIgnoreCase));

                    if (channelServerModel == null)
                    {
                        youTubeChannelList.Add(new YouTubeChannelServerModel { YouTubeChannelId = server.OwnerYouTubeChannelId, Servers = new List<ulong> { server.Id } });
                    }
                    else
                    {
                        channelServerModel.Servers.Add(server.Id);
                    }
                }
            }

            foreach (var c in youTubeChannelList)
            {
                YouTubeSearchListChannel streamResult = null;

                try
                {
                    // Query Youtube for our stream.
                    streamResult = await youtubeManager.GetLiveVideoByChannelId(c.YouTubeChannelId);
                }
                catch (Exception wex)
                {
                    // Log our error and move to the next user.

                    _logging.LogError("YouTube Error: " + wex.Message + " for user: " + c.YouTubeChannelId);
                    continue;
                }

                if (streamResult != null && streamResult.items.Count > 0)
                {
                    var stream = streamResult.items[0];

                    foreach (var s in c.Servers)
                    {
                        var server = BotFiles.GetConfiguredServerById(s);
                        var channel = liveChannels.FirstOrDefault(x => x.Name.ToLower() == c.YouTubeChannelId.ToLower());
                        bool allowEveryone = server.AllowEveryone;
                        var chat = await DiscordHelper.GetMessageChannel(server.Id, server.OwnerLiveChannel);

                        if (chat == null)
                        {
                            continue;
                        }

                        if (channel == null)
                        {
                            channel = new LiveChannel()
                            {
                                Name = c.YouTubeChannelId,
                                Servers = new List<ulong>()
                            };

                            channel.Servers.Add(server.Id);

                            liveChannels.Add(channel);
                        }
                        else
                        {
                            channel.Servers.Add(server.Id);
                        }

                        // Build our message
                        YouTubeChannelSnippet channelData = null;

                        try
                        {
                            channelData = await youtubeManager.GetYouTubeChannelSnippetById(stream.snippet.ChannelId);
                        }
                        catch (Exception wex)
                        {
                            // Log our error and move to the next user.

                            _logging.LogError("YouTube Error: " + wex.Message + " for user: " + c.YouTubeChannelId);
                            continue;
                        }

                        if (channelData == null)
                        {
                            continue;
                        }

                        string url = "http://" + (server.UseYouTubeGamingPublished ? "gaming" : "www") + ".youtube.com/watch?v=" + stream.id;
                        string channelTitle = stream.snippet.ChannelTitle;
                        string avatarUrl = channelData.items.Count > 0 ? channelData.items[0].snippet.thumbnails.high.url : "";
                        string thumbnailUrl = stream.snippet.thumbnails.high.url;

                        var message = await MessagingHelper.BuildMessage(channelTitle, "a game", stream.snippet.title, url, avatarUrl, thumbnailUrl,
                            Constants.YouTubeGaming, c.YouTubeChannelId, server, server.OwnerLiveChannel, null);

                        var finalCheck = BotFiles.GetCurrentlyLiveYouTubeChannels().FirstOrDefault(x => x.Name == c.YouTubeChannelId);

                        if (finalCheck == null || !finalCheck.Servers.Contains(server.Id))
                        {
                            if (channel.ChannelMessages == null)
                                channel.ChannelMessages = new List<ChannelMessage>();

                            channel.ChannelMessages.AddRange(await MessagingHelper.SendMessages(Constants.YouTubeGaming, new List<BroadcastMessage>() { message }));

                            File.WriteAllText(Constants.ConfigRootDirectory + Constants.LiveDirectory + Constants.YouTubeDirectory + c.YouTubeChannelId + ".json", JsonConvert.SerializeObject(channel));

                            _logging.LogYouTubeGaming(channelTitle + " has gone online.");
                        }
                    }
                }
            }
        }

        public async Task CheckSmashcastLive()
        {
            var servers = await _guildManager.GetGuildsForLive();
            var liveChannels = BotFiles.GetCurrentlyLiveHitboxChannels();

            // Loop through servers to broadcast.
            foreach (var server in servers)
            {
                if (!server.AllowLive)
                {
                    continue;
                }

                if (server.Id == 0 || server.GoLiveChannel == 0)
                { continue; }

                if (server.ServerHitboxChannels != null)
                {
                    foreach (var hitboxChannel in server.ServerHitboxChannels)
                    {
                        var channel = liveChannels.FirstOrDefault(x => x.Name.ToLower() == hitboxChannel.ToLower());

                        SmashcastChannel stream = null;

                        try
                        {
                            stream = await smashcastManager.GetChannelByName(hitboxChannel);
                        }
                        catch (Exception wex)
                        {
                            // Log our error and move to the next user.

                            _logging.LogError("Smashcast Error: " + wex.Message + " for user: " + hitboxChannel + " in Discord Server Id: " + server.Id);
                            continue;
                        }

                        // if our stream isnt null, and we have a return from mixer.
                        if (stream != null && stream.livestream != null && stream.livestream.Count > 0)
                        {
                            if (stream.livestream[0].media_is_live == "1")
                            {
                                bool allowEveryone = server.AllowEveryone;
                                var chat = await DiscordHelper.GetMessageChannel(server.Id, server.GoLiveChannel);

                                if (chat == null)
                                {
                                    continue;
                                }

                                bool checkChannelBroadcastStatus = channel == null || !channel.Servers.Contains(server.Id);
                                bool checkGoLive = !string.IsNullOrEmpty(server.GoLiveChannel.ToString()) && server.GoLiveChannel != 0;

                                if (checkChannelBroadcastStatus)
                                {
                                    if (checkGoLive)
                                    {
                                        if (chat != null)
                                        {
                                            if (channel == null)
                                            {
                                                channel = new LiveChannel()
                                                {
                                                    Name = hitboxChannel,
                                                    Servers = new List<ulong>()
                                                };

                                                channel.Servers.Add(server.Id);

                                                liveChannels.Add(channel);
                                            }
                                            else
                                            {
                                                channel.Servers.Add(server.Id);
                                            }

                                            string gameName = stream.livestream[0].category_name == null ? "a game" : stream.livestream[0].category_name;
                                            string url = "http://smashcast.tv/" + hitboxChannel;

                                            var message = await MessagingHelper.BuildMessage(
                                                hitboxChannel, gameName, stream.livestream[0].media_status, url, "http://edge.sf.hitbox.tv" +
                                                stream.livestream[0].Channel.user_logo, "http://edge.sf.hitbox.tv" +
                                                stream.livestream[0].media_thumbnail_large, Constants.Smashcast, hitboxChannel, server, server.GoLiveChannel, null);

                                            var finalCheck = BotFiles.GetCurrentlyLiveHitboxChannels().FirstOrDefault(x => x.Name == hitboxChannel);

                                            if (finalCheck == null || !finalCheck.Servers.Contains(server.Id))
                                            {
                                                if (channel.ChannelMessages == null)
                                                    channel.ChannelMessages = new List<ChannelMessage>();

                                                channel.ChannelMessages.AddRange(await MessagingHelper.SendMessages(Constants.Smashcast, new List<BroadcastMessage>() { message }));

                                                File.WriteAllText(Constants.ConfigRootDirectory + Constants.LiveDirectory + Constants.SmashcastDirectory + hitboxChannel + ".json", JsonConvert.SerializeObject(channel));

                                                _logging.LogSmashcast(hitboxChannel + " has gone online.");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public async Task CheckOwnerSmashcastLive()
        {
            var servers = await _guildManager.GetGuildsForLive();
            var liveChannels = BotFiles.GetCurrentlyLiveHitboxChannels();

            // Loop through servers to broadcast.
            foreach (var server in servers)
            {
                if (!server.AllowLive)
                {
                    continue;
                }

                if (server.Id == 0 || server.OwnerLiveChannel == 0)
                { continue; }

                if (server.OwnerHitboxChannel != null)
                {
                    var channel = liveChannels.FirstOrDefault(x => x.Name.ToLower() == server.OwnerHitboxChannel.ToLower());

                    SmashcastChannel stream = null;

                    try
                    {
                        stream = await smashcastManager.GetChannelByName(server.OwnerHitboxChannel);
                    }
                    catch (Exception wex)
                    {
                        // Log our error and move to the next user.

                        _logging.LogError("Smashcast Error: " + wex.Message + " for user: " + server.OwnerHitboxChannel + " in Discord Server Id: " + server.Id);
                        continue;
                    }

                    // if our stream isnt null, and we have a return from mixer.
                    if (stream != null && stream.livestream != null && stream.livestream.Count > 0)
                    {
                        if (stream.livestream[0].media_is_live == "1")
                        {
                            bool allowEveryone = server.AllowEveryone;
                            var chat = await DiscordHelper.GetMessageChannel(server.Id, server.OwnerLiveChannel);

                            if (chat == null)
                            {
                                continue;
                            }

                            bool checkChannelBroadcastStatus = channel == null || !channel.Servers.Contains(server.Id);
                            bool checkGoLive = !string.IsNullOrEmpty(server.OwnerLiveChannel.ToString()) && server.OwnerLiveChannel != 0;

                            if (checkChannelBroadcastStatus)
                            {
                                if (checkGoLive)
                                {
                                    if (chat != null)
                                    {
                                        if (channel == null)
                                        {
                                            channel = new LiveChannel()
                                            {
                                                Name = server.OwnerHitboxChannel,
                                                Servers = new List<ulong>()
                                            };

                                            channel.Servers.Add(server.Id);

                                            liveChannels.Add(channel);
                                        }
                                        else
                                        {
                                            channel.Servers.Add(server.Id);
                                        }

                                        string gameName = stream.livestream[0].category_name == null ? "a game" : stream.livestream[0].category_name;
                                        string url = "http://smashcast.tv/" + server.OwnerHitboxChannel;

                                        var message = await MessagingHelper.BuildMessage(
                                            server.OwnerHitboxChannel, gameName, stream.livestream[0].media_status, url, "http://edge.sf.hitbox.tv" +
                                            stream.livestream[0].Channel.user_logo, "http://edge.sf.hitbox.tv" +
                                            stream.livestream[0].media_thumbnail_large, Constants.Smashcast, server.OwnerHitboxChannel, server, server.OwnerLiveChannel, null);

                                        var finalCheck = BotFiles.GetCurrentlyLiveHitboxChannels().FirstOrDefault(x => x.Name == server.OwnerHitboxChannel);

                                        if (finalCheck == null || !finalCheck.Servers.Contains(server.Id))
                                        {
                                            if (channel.ChannelMessages == null)
                                                channel.ChannelMessages = new List<ChannelMessage>();

                                            channel.ChannelMessages.AddRange(await MessagingHelper.SendMessages(Constants.Smashcast, new List<BroadcastMessage>() { message }));

                                            File.WriteAllText(Constants.ConfigRootDirectory + Constants.LiveDirectory + Constants.SmashcastDirectory + server.OwnerHitboxChannel + ".json", JsonConvert.SerializeObject(channel));

                                            _logging.LogSmashcast(server.OwnerHitboxChannel + " has gone online.");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public async Task CheckPicartoLive()
        {
            var servers = await _guildManager.GetGuildsForLive();
            var liveChannels = BotFiles.GetCurrentlyLivePicartoChannels();

            // Loop through servers to broadcast.
            foreach (var server in servers)
            {
                if (!server.AllowLive)
                {
                    continue;
                }

                if (server.Id == 0 || server.GoLiveChannel == 0)
                { continue; }

                if (server.PicartoChannels != null)
                {
                    foreach (var picartoChannel in server.PicartoChannels)
                    {
                        var channel = liveChannels.FirstOrDefault(x => x.Name.ToLower() == picartoChannel.ToLower());

                        PicartoChannel stream = null;

                        try
                        {
                            stream = await picartoManager.GetChannelByName(picartoChannel);
                        }
                        catch (Exception wex)
                        {
                            // Log our error and move to the next user.

                            _logging.LogError("Picarto Error: " + wex.Message + " for user: " + picartoChannel + " in Discord Server Id: " + server.Id);
                            continue;
                        }

                        // if our stream isnt null, and we have a return from mixer.
                        if (stream != null)
                        {
                            if (stream.Online)
                            {
                                bool allowEveryone = server.AllowEveryone;
                                var chat = await DiscordHelper.GetMessageChannel(server.Id, server.GoLiveChannel);

                                if (chat == null)
                                {
                                    continue;
                                }

                                bool checkChannelBroadcastStatus = channel == null || !channel.Servers.Contains(server.Id);
                                bool checkGoLive = !string.IsNullOrEmpty(server.GoLiveChannel.ToString()) && server.GoLiveChannel != 0;

                                if (checkChannelBroadcastStatus)
                                {
                                    if (checkGoLive)
                                    {
                                        if (chat != null)
                                        {
                                            if (channel == null)
                                            {
                                                channel = new LiveChannel()
                                                {
                                                    Name = picartoChannel,
                                                    Servers = new List<ulong>()
                                                };

                                                channel.Servers.Add(server.Id);

                                                liveChannels.Add(channel);
                                            }
                                            else
                                            {
                                                channel.Servers.Add(server.Id);
                                            }

                                            if (server.LiveMessage == null)
                                            {
                                                server.LiveMessage = "%CHANNEL% just went live - %TITLE% - %URL%";
                                            }

                                            string url = "https://picarto.tv/user_data/usrimg/" + stream.Name.ToLower() + "/dsdefault.jpg";

                                            EmbedBuilder embedBuilder = new EmbedBuilder();
                                            EmbedAuthorBuilder author = new EmbedAuthorBuilder();
                                            EmbedFooterBuilder footer = new EmbedFooterBuilder();

                                            author.IconUrl = "https://picarto.tv/user_data/usrimg/" + stream.Name.ToLower() + "/dsdefault.jpg";
                                            author.Name = stream.Name;
                                            author.Url = "https://picarto.tv/" + stream.Name;
                                            embedBuilder.Author = author;

                                            footer.IconUrl = "https://picarto.tv/images/Picarto_logo.png";
                                            footer.Text = "[Picarto] - " + DateTime.UtcNow.AddHours(server.TimeZoneOffset);
                                            embedBuilder.Footer = footer;

                                            embedBuilder.Title = stream.Name + " has gone live!";
                                            embedBuilder.Color = new Color(192, 192, 192);
                                            embedBuilder.ThumbnailUrl = server.AllowThumbnails ? "https://picarto.tv/user_data/usrimg/" + stream.Name.ToLower() + "/dsdefault.jpg" : "";
                                            embedBuilder.ImageUrl = "https://thumb.picarto.tv/thumbnail/" + stream.Name + ".jpg";

                                            embedBuilder.Description = server.LiveMessage.Replace("%CHANNEL%", stream.Name).Replace("%TITLE%", stream.Title).Replace("%URL%", "https://picarto.tv/" + stream.Name).Replace("%GAME%", stream.Category);

                                            embedBuilder.AddField(f =>
                                            {
                                                f.Name = "Category";
                                                f.Value = stream.Category;
                                                f.IsInline = true;
                                            });

                                            embedBuilder.AddField(f =>
                                            {
                                                f.Name = "Adult Stream?";
                                                f.Value = stream.Adult ? "Yup!" : "Nope!";
                                                f.IsInline = true;
                                            });

                                            embedBuilder.AddField(f =>
                                            {
                                                f.Name = "Total Viewers";
                                                f.Value = stream.ViewersTotal;
                                                f.IsInline = true;
                                            });

                                            embedBuilder.AddField(f =>
                                            {
                                                f.Name = "Total Followers";
                                                f.Value = stream.Followers;
                                                f.IsInline = true;
                                            });

                                            string tags = "";
                                            foreach (var t in stream.Tags)
                                            {
                                                tags += t + ", ";
                                            }

                                            embedBuilder.AddField(f =>
                                            {
                                                f.Name = "Stream Tags";
                                                f.Value = string.IsNullOrEmpty(tags.Trim().TrimEnd(',')) ? "None" : tags.Trim().TrimEnd(',');
                                                f.IsInline = false;
                                            });

                                            var role = await DiscordHelper.GetRoleByGuildAndId(server.Id, server.MentionRole);

                                            if (role == null)
                                            {
                                                server.MentionRole = 0;
                                            }

                                            var message = (server.AllowEveryone ? server.MentionRole != 0 ? role.Mention : "@everyone " : "");

                                            if (server.UseTextAnnouncements)
                                            {
                                                if (!server.AllowThumbnails)
                                                {
                                                    url = "<" + url + ">";
                                                }

                                                message += "**[Picarto]** " + server.LiveMessage.Replace("%CHANNEL%", stream.Name).Replace("%TITLE%", stream.Title).Replace("%URL%", "https://picarto.tv/" + stream.Name).Replace("%GAME%", stream.Category);
                                            }

                                            var broadcastMessage = new BroadcastMessage()
                                            {
                                                GuildId = server.Id,
                                                ChannelId = server.GoLiveChannel,
                                                UserId = picartoChannel,
                                                Message = message,
                                                Platform = Constants.Picarto,
                                                Embed = (!server.UseTextAnnouncements ? embedBuilder.Build() : null)
                                            };

                                            var finalCheck = BotFiles.GetCurrentlyLivePicartoChannels().FirstOrDefault(x => x.Name == picartoChannel);

                                            if (finalCheck == null || !finalCheck.Servers.Contains(server.Id))
                                            {
                                                if (channel.ChannelMessages == null)
                                                    channel.ChannelMessages = new List<ChannelMessage>();

                                                channel.ChannelMessages.AddRange(await MessagingHelper.SendMessages(Constants.Picarto, new List<BroadcastMessage>() { broadcastMessage }));

                                                File.WriteAllText(Constants.ConfigRootDirectory + Constants.LiveDirectory + Constants.PicartoDirectory + picartoChannel + ".json", JsonConvert.SerializeObject(channel));

                                                _logging.LogPicarto(picartoChannel + " has gone online.");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public async Task CheckOwnerPicartoLive()
        {
            var servers = await _guildManager.GetGuildsForLive();
            var liveChannels = BotFiles.GetCurrentlyLivePicartoChannels();

            // Loop through servers to broadcast.
            foreach (var server in servers)
            {
                if (!server.AllowLive)
                {
                    continue;
                }

                if (server.Id == 0 || server.OwnerLiveChannel == 0)
                { continue; }

                if (server.OwnerPicartoChannel != null)
                {
                    var channel = liveChannels.FirstOrDefault(x => x.Name.ToLower() == server.OwnerPicartoChannel.ToLower());

                    PicartoChannel stream = null;

                    try
                    {
                        stream = await picartoManager.GetChannelByName(server.OwnerPicartoChannel);
                    }
                    catch (Exception wex)
                    {
                        // Log our error and move to the next user.

                        _logging.LogError("Picarto Error: " + wex.Message + " for user: " + server.OwnerPicartoChannel + " in Discord Server Id: " + server.Id);
                        continue;
                    }

                    // if our stream isnt null, and we have a return from mixer.
                    if (stream != null)
                    {
                        if (stream.Online)
                        {
                            bool allowEveryone = server.AllowEveryone;
                            var chat = await DiscordHelper.GetMessageChannel(server.Id, server.OwnerLiveChannel);

                            if (chat == null)
                            {
                                continue;
                            }

                            bool checkChannelBroadcastStatus = channel == null || !channel.Servers.Contains(server.Id);
                            bool checkGoLive = !string.IsNullOrEmpty(server.OwnerLiveChannel.ToString()) && server.OwnerLiveChannel != 0;

                            if (checkChannelBroadcastStatus)
                            {
                                if (checkGoLive)
                                {
                                    if (chat != null)
                                    {
                                        if (channel == null)
                                        {
                                            channel = new LiveChannel()
                                            {
                                                Name = server.OwnerPicartoChannel,
                                                Servers = new List<ulong>()
                                            };

                                            channel.Servers.Add(server.Id);

                                            liveChannels.Add(channel);
                                        }
                                        else
                                        {
                                            channel.Servers.Add(server.Id);
                                        }

                                        if (server.LiveMessage == null)
                                        {
                                            server.LiveMessage = "%CHANNEL% just went live - %TITLE% - %URL%";
                                        }

                                        string url = "https://picarto.tv/user_data/usrimg/" + stream.Name.ToLower() + "/dsdefault.jpg";

                                        EmbedBuilder embedBuilder = new EmbedBuilder();
                                        EmbedAuthorBuilder author = new EmbedAuthorBuilder();
                                        EmbedFooterBuilder footer = new EmbedFooterBuilder();

                                        author.IconUrl = "https://picarto.tv/user_data/usrimg/" + stream.Name.ToLower() + "/dsdefault.jpg";
                                        author.Name = stream.Name;
                                        author.Url = "https://picarto.tv/" + stream.Name;
                                        embedBuilder.Author = author;

                                        footer.IconUrl = "https://picarto.tv/images/Picarto_logo.png";
                                        footer.Text = "[Picarto] - " + DateTime.UtcNow.AddHours(server.TimeZoneOffset);
                                        embedBuilder.Footer = footer;

                                        embedBuilder.Title = stream.Name + " has gone live!";
                                        embedBuilder.Color = new Color(192, 192, 192);
                                        embedBuilder.ThumbnailUrl = server.AllowThumbnails ? "https://picarto.tv/user_data/usrimg/" + stream.Name.ToLower() + "/dsdefault.jpg" : "";
                                        embedBuilder.ImageUrl = "https://thumb.picarto.tv/thumbnail/" + stream.Name + ".jpg";

                                        embedBuilder.Description = server.LiveMessage.Replace("%CHANNEL%", stream.Name).Replace("%TITLE%", stream.Title).Replace("%URL%", "https://picarto.tv/" + stream.Name).Replace("%GAME%", stream.Category);

                                        embedBuilder.AddField(f =>
                                        {
                                            f.Name = "Category";
                                            f.Value = stream.Category;
                                            f.IsInline = true;
                                        });

                                        embedBuilder.AddField(f =>
                                        {
                                            f.Name = "Adult Stream?";
                                            f.Value = stream.Adult ? "Yup!" : "Nope!";
                                            f.IsInline = true;
                                        });

                                        embedBuilder.AddField(f =>
                                        {
                                            f.Name = "Total Viewers";
                                            f.Value = stream.ViewersTotal;
                                            f.IsInline = true;
                                        });

                                        embedBuilder.AddField(f =>
                                        {
                                            f.Name = "Total Followers";
                                            f.Value = stream.Followers;
                                            f.IsInline = true;
                                        });

                                        //string tags = "";
                                        //foreach (var t in stream.Tags)
                                        //{
                                        //    tags += t + ", ";
                                        //}

                                        //embedBuilder.AddField(f =>
                                        //{
                                        //    f.Name = "Stream Tags";
                                        //    f.Value = tags.Trim().TrimEnd(',');
                                        //    f.IsInline = false;
                                        //});

                                        var role = await DiscordHelper.GetRoleByGuildAndId(server.Id, server.MentionRole);

                                        if (role == null)
                                        {
                                            server.MentionRole = 0;
                                        }

                                        var message = (server.AllowEveryone ? server.MentionRole != 0 ? role.Mention : "@everyone " : "");

                                        if (server.UseTextAnnouncements)
                                        {
                                            if (!server.AllowThumbnails)
                                            {
                                                url = "<" + url + ">";
                                            }

                                            message += "**[Picarto]** " + server.LiveMessage.Replace("%CHANNEL%", stream.Name).Replace("%TITLE%", stream.Title).Replace("%URL%", "https://picarto.tv/" + stream.Name).Replace("%GAME%", stream.Category);
                                        }

                                        var broadcastMessage = new BroadcastMessage()
                                        {
                                            GuildId = server.Id,
                                            ChannelId = server.GoLiveChannel,
                                            UserId = server.OwnerPicartoChannel,
                                            Message = message,
                                            Platform = Constants.Picarto,
                                            Embed = (!server.UseTextAnnouncements ? embedBuilder.Build() : null)
                                        };

                                        var finalCheck = BotFiles.GetCurrentlyLivePicartoChannels().FirstOrDefault(x => x.Name == server.OwnerPicartoChannel);

                                        if (finalCheck == null || !finalCheck.Servers.Contains(server.Id))
                                        {
                                            if (channel.ChannelMessages == null)
                                                channel.ChannelMessages = new List<ChannelMessage>();

                                            channel.ChannelMessages.AddRange(await MessagingHelper.SendMessages(Constants.Picarto, new List<BroadcastMessage>() { broadcastMessage }));

                                            File.WriteAllText(Constants.ConfigRootDirectory + Constants.LiveDirectory + Constants.PicartoDirectory + server.OwnerPicartoChannel + ".json", JsonConvert.SerializeObject(channel));

                                            _logging.LogPicarto(server.OwnerPicartoChannel + " has gone online.");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public async Task CheckPublishedYouTube()
        {
            var servers = await _guildManager.GetGuildsForLive();
            var users = BotFiles.GetConfiguredUsers();
            var now = DateTime.UtcNow;
            var then = now.AddMilliseconds(-(Constants.YouTubePublishedInterval));

            foreach (var server in servers)
            {
                if (!server.AllowPublished)
                {
                    continue;
                }

                // If server isnt set or published channel isnt set, skip it.
                if (server.Id == 0 || server.PublishedChannel == 0)
                {
                    continue;
                }

                // If they dont allow published, skip it.
                if (!server.AllowPublished)
                {
                    continue;
                }

                var chat = await DiscordHelper.GetMessageChannel(server.Id, server.PublishedChannel);

                if (chat == null)
                {
                    continue;
                }

                if (server.ServerYouTubeChannelIds == null || server.ServerYouTubeChannelIds.Count < 0)
                {
                    continue;
                }

                foreach (var user in server.ServerYouTubeChannelIds)
                {
                    if (string.IsNullOrEmpty(user))
                    {
                        continue;
                    }

                    YouTubePlaylist playlist = null;

                    try
                    {
                        var details = await youtubeManager.GetContentDetailsByChannelId(user);

                        if (details == null || details.items == null || details.items.Count < 1 || string.IsNullOrEmpty(details.items[0].contentDetails.relatedPlaylists.uploads))
                        {
                            continue;
                        }

                        playlist = await youtubeManager.GetPlaylistItemsByPlaylistId(details.items[0].contentDetails.relatedPlaylists.uploads);

                        if (playlist == null || playlist.items == null || playlist.items.Count < 1)
                        {
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logging.LogError("YouTube Published Error: " + ex.Message + " for user: " + user + " in Discord Server: " + server.Id);
                        continue;
                    }

                    foreach (var video in playlist.items)
                    {
                        var publishDate = DateTime.Parse(video.snippet.publishedAt, null, System.Globalization.DateTimeStyles.AdjustToUniversal);

                        if (!(publishDate > then && publishDate < now))
                        {
                            continue;
                        }

                        string url = "http://" + (server.UseYouTubeGamingPublished ? "gaming" : "www") + ".youtube.com/watch?v=" + video.snippet.resourceId.videoId;

                        EmbedBuilder embed = new EmbedBuilder();
                        EmbedAuthorBuilder author = new EmbedAuthorBuilder();
                        EmbedFooterBuilder footer = new EmbedFooterBuilder();

                        YouTubeChannelSnippet channelData = null;

                        try
                        {
                            channelData = await youtubeManager.GetYouTubeChannelSnippetById(video.snippet.ChannelId);
                        }
                        catch (Exception wex)
                        {
                            // Log our error and move to the next user.

                            _logging.LogError("YouTube Error: " + wex.Message + " for user: " + video.snippet.ChannelId);
                            continue;
                        }

                        if (channelData == null)
                        {
                            continue;
                        }

                        if (server.PublishedMessage == null)
                        {
                            server.PublishedMessage = "%CHANNEL% just published a new video - %TITLE% - %URL%";
                        }

                        Color red = new Color(179, 18, 23);
                        author.IconUrl = client.CurrentUser.GetAvatarUrl() + "?_=" + Guid.NewGuid().ToString().Replace("-", "");
                        author.Name = Program.client.CurrentUser.Username;
                        author.Url = url;
                        footer.Text = "[" + Constants.YouTube + "] - " + DateTime.UtcNow.AddHours(server.TimeZoneOffset);
                        footer.IconUrl = "http://couchbot.io/img/ytg.jpg";
                        embed.Author = author;
                        embed.Color = red;
                        embed.Description = server.PublishedMessage.Replace("%CHANNEL%", video.snippet.ChannelTitle).Replace("%GAME%", "a game").Replace("%TITLE%", video.snippet.title).Replace("%URL%", url);
                        embed.Title = video.snippet.ChannelTitle + " published a new video!";
                        embed.ThumbnailUrl = channelData.items.Count > 0 ? channelData.items[0].snippet.thumbnails.high.url + "?_=" + Guid.NewGuid().ToString().Replace("-", "") : "";
                        embed.ImageUrl = server.AllowThumbnails ? video.snippet.thumbnails.high.url + "?_=" + Guid.NewGuid().ToString().Replace("-", "") : "";
                        embed.Footer = footer;

                        var role = await DiscordHelper.GetRoleByGuildAndId(server.Id, server.MentionRole);

                        if (role == null)
                        {
                            server.MentionRole = 0;
                        }

                        var message = (server.AllowEveryone ? server.MentionRole != 0 ? role.Mention : "@everyone " : "");

                        if (server.UseTextAnnouncements)
                        {
                            if (!server.AllowThumbnails)
                            {
                                url = "<" + url + ">";
                            }

                            message += "**[" + Constants.YouTube + "]** " + server.PublishedMessage.Replace("%CHANNEL%", video.snippet.ChannelTitle).Replace("%TITLE%", video.snippet.title).Replace("%URL%", url);
                        }

                        _logging.LogYouTube(video.snippet.ChannelTitle + " has published a new video.");

                        await SendMessage(new BroadcastMessage()
                        {
                            GuildId = server.Id,
                            ChannelId = server.PublishedChannel,
                            UserId = user,
                            Message = message,
                            Platform = Constants.YouTube,
                            Embed = (!server.UseTextAnnouncements ? embed.Build() : null)
                        });
                    }
                }
            }
        }

        public async Task CheckOwnerPublishedYouTube()
        {
            var servers = await _guildManager.GetGuildsForLive();
            var users = BotFiles.GetConfiguredUsers();
            var now = DateTime.UtcNow;
            var then = now.AddMilliseconds(-(Constants.YouTubePublishedInterval));

            foreach (var server in servers)
            {
                if (!server.AllowPublished)
                {
                    continue;
                }

                // If server isnt set or published channel isnt set, skip it.
                if (server.Id == 0 || server.OwnerPublishedChannel == 0)
                {
                    continue;
                }

                var chat = await DiscordHelper.GetMessageChannel(server.Id, server.OwnerPublishedChannel);

                if (chat == null)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(server.OwnerYouTubeChannelId))
                {
                    continue;
                }

                YouTubePlaylist playlist = null;

                try
                {
                    var details = await youtubeManager.GetContentDetailsByChannelId(server.OwnerYouTubeChannelId);

                    if (details == null || details.items == null || details.items.Count < 1 || string.IsNullOrEmpty(details.items[0].contentDetails.relatedPlaylists.uploads))
                    {
                        continue;
                    }

                    playlist = await youtubeManager.GetPlaylistItemsByPlaylistId(details.items[0].contentDetails.relatedPlaylists.uploads);

                    if (playlist == null || playlist.items == null || playlist.items.Count < 1)
                    {
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    _logging.LogError("YouTube Published Error: " + ex.Message + " for user: " + server.OwnerYouTubeChannelId + " in Discord Server: " + server.Id);
                    continue;
                }

                foreach (var video in playlist.items)
                {
                    var publishDate = DateTime.Parse(video.snippet.publishedAt, null, System.Globalization.DateTimeStyles.AdjustToUniversal);

                    if (!(publishDate > then && publishDate < now))
                    {
                        continue;
                    }

                    string url = "http://" + (server.UseYouTubeGamingPublished ? "gaming" : "www") + ".youtube.com/watch?v=" + video.snippet.resourceId.videoId;

                    EmbedBuilder embed = new EmbedBuilder();
                    EmbedAuthorBuilder author = new EmbedAuthorBuilder();
                    EmbedFooterBuilder footer = new EmbedFooterBuilder();

                    YouTubeChannelSnippet channelData = null;

                    try
                    {
                        channelData = await youtubeManager.GetYouTubeChannelSnippetById(video.snippet.ChannelId);

                    }
                    catch (Exception wex)
                    {
                        // Log our error and move to the next user.

                        _logging.LogError("YouTube Error: " + wex.Message + " for user: " + video.snippet.ChannelId);
                        continue;
                    }

                    if (channelData == null)
                    {
                        continue;
                    }

                    if (server.PublishedMessage == null)
                    {
                        server.PublishedMessage = "%CHANNEL% just published a new video - %TITLE% - %URL%";
                    }

                    Color red = new Color(179, 18, 23);
                    author.IconUrl = client.CurrentUser.GetAvatarUrl() + "?_=" + Guid.NewGuid().ToString().Replace("-", "");
                    author.Name = Program.client.CurrentUser.Username;
                    author.Url = url;
                    footer.Text = "[" + Constants.YouTube + "] - " + DateTime.UtcNow.AddHours(server.TimeZoneOffset);
                    footer.IconUrl = "http://couchbot.io/img/ytg.jpg";
                    embed.Author = author;
                    embed.Color = red;
                    embed.Description = server.PublishedMessage.Replace("%CHANNEL%", video.snippet.ChannelTitle).Replace("%GAME%", "a game").Replace("%TITLE%", video.snippet.title).Replace("%URL%", url);
                    embed.Title = video.snippet.ChannelTitle + " published a new video!";
                    embed.ThumbnailUrl = channelData.items.Count > 0 ? channelData.items[0].snippet.thumbnails.high.url + "?_=" + Guid.NewGuid().ToString().Replace("-", "") : "";
                    embed.ImageUrl = server.AllowThumbnails ? video.snippet.thumbnails.high.url + "?_=" + Guid.NewGuid().ToString().Replace("-", "") : "";
                    embed.Footer = footer;

                    var role = await DiscordHelper.GetRoleByGuildAndId(server.Id, server.MentionRole);

                    if (role == null)
                    {
                        server.MentionRole = 0;
                    }

                    var message = (server.AllowEveryone ? server.MentionRole != 0 ? role.Mention : "@everyone " : "");

                    if (server.UseTextAnnouncements)
                    {
                        if (!server.AllowThumbnails)
                        {
                            url = "<" + url + ">";
                        }

                        message += "**[" + Constants.YouTube + "]** " + server.PublishedMessage.Replace("%CHANNEL%", video.snippet.ChannelTitle).Replace("%TITLE%", video.snippet.title).Replace("%URL%", url);
                    }

                    _logging.LogYouTube(video.snippet.ChannelTitle + " has published a new video.");

                    await SendMessage(new BroadcastMessage()
                    {
                        GuildId = server.Id,
                        ChannelId = server.OwnerPublishedChannel,
                        UserId = server.OwnerYouTubeChannelId,
                        Message = message,
                        Platform = Constants.YouTube,
                        Embed = (!server.UseTextAnnouncements ? embed.Build() : null)
                    });
                }
            }
        }

        public async Task CheckVidMe()
        {
            var servers = await _guildManager.GetGuildsForLive();
            var users = BotFiles.GetConfiguredUsers();
            var now = DateTime.UtcNow;
            var then = now.AddMilliseconds(-(Constants.VidMeInterval));

            foreach (var server in servers)
            {
                // If server isnt set or published channel isnt set, skip it.
                if (server.Id == 0 || server.PublishedChannel == 0)
                {
                    continue;
                }

                // If they dont allow published, skip it.
                if (!server.AllowPublished)
                {
                    continue;
                }

                var chat = await DiscordHelper.GetMessageChannel(server.Id, server.PublishedChannel);

                if (chat == null)
                {
                    continue;
                }

                if (server.ServerVidMeChannelIds == null || server.ServerVidMeChannelIds.Count < 0)
                {
                    continue;
                }

                foreach (var channelId in server.ServerVidMeChannelIds)
                {
                    if (channelId == 0)
                    {
                        continue;
                    }

                    VidMeUserVideos videoResponse = null;

                    try
                    {
                        videoResponse = await vidMeManager.GetChannelVideosById(channelId);

                        if (videoResponse == null || videoResponse.videos == null || videoResponse.videos.Count < 1)
                        {
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logging.LogError("VidMe Published Error: " + ex.Message + " for user: " + channelId + " in Discord Server: " + server.Id);
                        continue;
                    }

                    foreach (var video in videoResponse.videos)
                    {
                        var publishDate = DateTime.Parse(video.date_published, null, System.Globalization.DateTimeStyles.AdjustToUniversal);

                        if (!(publishDate > then && publishDate < now))
                        {
                            continue;
                        }

                        string url = video.full_url;

                        EmbedBuilder embed = new EmbedBuilder();
                        EmbedAuthorBuilder author = new EmbedAuthorBuilder();
                        EmbedFooterBuilder footer = new EmbedFooterBuilder();

                        if (server.PublishedMessage == null)
                        {
                            server.PublishedMessage = "%CHANNEL% just published a new video - %TITLE% - %URL%";
                        }

                        Color red = new Color(179, 18, 23);
                        author.IconUrl = client.CurrentUser.GetAvatarUrl() + "?_=" + Guid.NewGuid().ToString().Replace("-", "");
                        author.Name = Program.client.CurrentUser.Username;
                        author.Url = url;
                        footer.Text = "[" + Constants.VidMe + "] - " + DateTime.UtcNow.AddHours(server.TimeZoneOffset);
                        footer.IconUrl = "http://couchbot.io/img/vidme.png";
                        embed.Author = author;
                        embed.Color = red;
                        embed.Description = server.PublishedMessage.Replace("%CHANNEL%", video.user.username).Replace("%GAME%", "a video").Replace("%TITLE%", video.title).Replace("%URL%", url);
                        embed.Title = video.user.username + " published a new video!";
                        embed.ThumbnailUrl = video.user.avatar_url;
                        embed.ImageUrl = video.thumbnail_url;
                        embed.Footer = footer;

                        var role = await DiscordHelper.GetRoleByGuildAndId(server.Id, server.MentionRole);

                        if (role == null)
                        {
                            server.MentionRole = 0;
                        }

                        var message = (server.AllowEveryone ? server.MentionRole != 0 ? role.Mention : "@everyone " : "");

                        if (server.UseTextAnnouncements)
                        {
                            if (!server.AllowThumbnails)
                            {
                                url = "<" + url + ">";
                            }

                            message += "**[" + Constants.VidMe + "]** " +
                                server.PublishedMessage.Replace("%CHANNEL%", video.user.username).Replace("%GAME%", "a video").Replace("%TITLE%", video.title).Replace("%URL%", url);
                        }

                        _logging.LogVidMe(video.user.username + " has published a new video.");

                        await SendMessage(new BroadcastMessage()
                        {
                            GuildId = server.Id,
                            ChannelId = server.PublishedChannel,
                            UserId = video.user.username,
                            Message = message,
                            Platform = Constants.YouTube,
                            Embed = (!server.UseTextAnnouncements ? embed.Build() : null)
                        });
                    }
                }
            }
        }

        public async Task CheckOwnerVidMe()
        {
            var servers = await _guildManager.GetGuildsForLive();
            var users = BotFiles.GetConfiguredUsers();
            var now = DateTime.UtcNow;
            var then = now.AddMilliseconds(-(Constants.VidMeInterval));

            foreach (var server in servers)
            {
                if (!server.AllowPublished)
                {
                    continue;
                }

                // If server isnt set or published channel isnt set, skip it.
                if (server.Id == 0 || server.OwnerPublishedChannel == 0)
                {
                    continue;
                }

                var chat = await DiscordHelper.GetMessageChannel(server.Id, server.OwnerPublishedChannel);

                if (chat == null)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(server.OwnerVidMeChannel) || server.OwnerVidMeChannelId == 0)
                {
                    continue;
                }

                var channelId = server.OwnerVidMeChannelId;
                VidMeUserVideos videoResponse = null;

                try
                {
                    videoResponse = await vidMeManager.GetChannelVideosById(channelId);

                    if (videoResponse == null || videoResponse.videos == null || videoResponse.videos.Count < 1)
                    {
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    _logging.LogError("VidMe Published Error: " + ex.Message + " for user: " + channelId + " in Discord Server: " + server.Id);
                    continue;
                }

                foreach (var video in videoResponse.videos)
                {
                    var publishDate = DateTime.Parse(video.date_published, null, System.Globalization.DateTimeStyles.AdjustToUniversal);

                    if (!(publishDate > then && publishDate < now))
                    {
                        continue;
                    }

                    string url = video.full_url;

                    EmbedBuilder embed = new EmbedBuilder();
                    EmbedAuthorBuilder author = new EmbedAuthorBuilder();
                    EmbedFooterBuilder footer = new EmbedFooterBuilder();

                    if (server.PublishedMessage == null)
                    {
                        server.PublishedMessage = "%CHANNEL% just published a new video - %TITLE% - %URL%";
                    }

                    Color red = new Color(179, 18, 23);
                    author.IconUrl = client.CurrentUser.GetAvatarUrl() + "?_=" + Guid.NewGuid().ToString().Replace("-", "");
                    author.Name = Program.client.CurrentUser.Username;
                    author.Url = url;
                    footer.Text = "[" + Constants.VidMe + "] - " + DateTime.UtcNow.AddHours(server.TimeZoneOffset);
                    footer.IconUrl = "http://couchbot.io/img/vidme.png";
                    embed.Author = author;
                    embed.Color = red;
                    embed.Description = server.PublishedMessage.Replace("%CHANNEL%", video.user.username).Replace("%GAME%", "a video").Replace("%TITLE%", video.title).Replace("%URL%", url);
                    embed.Title = video.user.username + " published a new video!";
                    embed.ThumbnailUrl = video.user.avatar_url;
                    embed.ImageUrl = video.thumbnail_url;
                    embed.Footer = footer;

                    var role = await DiscordHelper.GetRoleByGuildAndId(server.Id, server.MentionRole);

                    if (role == null)
                    {
                        server.MentionRole = 0;
                    }

                    var message = (server.AllowEveryone ? server.MentionRole != 0 ? role.Mention : "@everyone " : "");

                    if (server.UseTextAnnouncements)
                    {
                        if (!server.AllowThumbnails)
                        {
                            url = "<" + url + ">";
                        }

                        message += "**[" + Constants.VidMe + "]** " +
                            server.PublishedMessage.Replace("%CHANNEL%", video.user.username).Replace("%GAME%", "a video").Replace("%TITLE%", video.title).Replace("%URL%", url);
                    }

                    _logging.LogVidMe(video.user.username + " has published a new video.");

                    await SendMessage(new BroadcastMessage()
                    {
                        GuildId = server.Id,
                        ChannelId = server.OwnerPublishedChannel,
                        UserId = video.user.username,
                        Message = message,
                        Platform = Constants.YouTube,
                        Embed = (!server.UseTextAnnouncements ? embed.Build() : null)
                    });
                }
            }
        }

    }
}
