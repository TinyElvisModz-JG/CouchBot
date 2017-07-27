using Discord;
using Discord.WebSocket;
using MTD.CouchBot.Domain.Models.Bot;
using MTD.CouchBot.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTD.CouchBot.Bot.Services
{
    public class MessagingService
    {
        private readonly DiscordShardedClient _client;
        private readonly IStatisticsManager _statisticsManager;

        public MessagingService(DiscordShardedClient client)
        {
            _client = client;
        }

        public static async Task<List<ChannelMessage>> SendMessages(string platform, List<BroadcastMessage> messages)
        {
            var channelMessages = new List<ChannelMessage>();

            foreach (var message in messages)
            {
                var chat = await DiscordHelper.GetMessageChannel(message.GuildId, message.ChannelId);

                if (chat != null)
                {
                    try
                    {
                        ChannelMessage channelMessage = new ChannelMessage();
                        channelMessage.ChannelId = message.ChannelId;
                        channelMessage.GuildId = message.GuildId;
                        channelMessage.DeleteOffline = message.DeleteOffline;

                        if (message.Embed != null)
                        {
                            RequestOptions options = new RequestOptions();
                            options.RetryMode = RetryMode.AlwaysRetry;
                            var msg = await chat.SendMessageAsync(message.Message, false, message.Embed, options);

                            if (msg != null || msg.Id != 0)
                            {
                                channelMessage.MessageId = msg.Id;
                            }
                        }
                        else
                        {
                            var msg = await chat.SendMessageAsync(message.Message);

                            if (msg != null || msg.Id != 0)
                            {
                                channelMessage.MessageId = msg.Id;
                            }
                        }

                        channelMessages.Add(channelMessage);

                        if (platform.Equals(Constants.Mixer))
                        {
                            statisticsManager.AddToBeamAlertCount();
                        }
                        else if (platform.Equals(Constants.Smashcast))
                        {
                            statisticsManager.AddToHitboxAlertCount();
                        }
                        else if (platform.Equals(Constants.Twitch))
                        {
                            statisticsManager.AddToTwitchAlertCount();
                        }
                        else if (platform.Equals(Constants.YouTubeGaming))
                        {
                            statisticsManager.AddToYouTubeAlertCount();
                        }
                        else if (platform.Equals(Constants.Picarto))
                        {
                            statisticsManager.AddToPicartoAlertCount();
                        }
                        else if (platform.Equals(Constants.VidMe))
                        {
                            statisticsManager.AddToVidMeAlertCount();
                        }

                    }
                    catch (Exception ex)
                    {
                        Logging.LogError("Send Message Error: " + ex.Message + " in server " + message.GuildId);
                    }
                }
            }
        }
    }
}
