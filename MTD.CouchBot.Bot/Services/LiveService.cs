using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTD.CouchBot.Bot.Services
{
    public class LiveService
    {
        public async Task CheckYouTubeLive()
        {
            // TODO Get Servers with Live Channel set from EF.
            var servers = BotFiles.GetConfiguredServersWithLiveChannel();
            // TODO Get Currently Live Channels from EF..
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

                    Logging.LogError("YouTube Error: " + wex.Message + " for user: " + c.YouTubeChannelId);
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
                            channelData = await youtubeManager.GetYouTubeChannelSnippetById(stream.snippet.channelId);
                        }
                        catch (Exception wex)
                        {
                            // Log our error and move to the next user.

                            Logging.LogError("YouTube Error: " + wex.Message + " for user: " + c.YouTubeChannelId);
                            continue;
                        }

                        if (channelData == null)
                        {
                            continue;
                        }

                        string url = "http://" + (server.UseYouTubeGamingPublished ? "gaming" : "www") + ".youtube.com/watch?v=" + stream.id;
                        string channelTitle = stream.snippet.channelTitle;
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

                            Logging.LogYouTubeGaming(channelTitle + " has gone online.");
                        }
                    }
                }
            }
        }
    }
}
