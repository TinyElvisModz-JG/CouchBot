using Discord;
using Discord.WebSocket;
using MTD.CouchBot.Domain;
using MTD.CouchBot.Domain.Models.Bot;
using MTD.CouchBot.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTD.CouchBot.Bot.Services
{
    public class MessagingService
    {
        private readonly DiscordShardedClient _client;
        private readonly IStatisticsManager _statisticsManager;
        private readonly LoggingService _logging;
        private readonly IGuildManager _guildManager;

        public MessagingService(DiscordShardedClient client, IStatisticsManager statisticsManager, LoggingService logging,
            IGuildManager guildManager)
        {
            _client = client;
            _statisticsManager = statisticsManager;
            _logging = logging;
            _guildManager = guildManager;
        }

        public async Task<List<ChannelMessage>> SendMessages(string platform, List<BroadcastMessage> messages)
        {
            var channelMessages = new List<ChannelMessage>();

            foreach (var message in messages)
            {
                var chat = (IMessageChannel) _client.GetChannel(message.ChannelId); 

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
                            _statisticsManager.AddToBeamAlertCount();
                        }
                        else if (platform.Equals(Constants.Smashcast))
                        {
                            _statisticsManager.AddToHitboxAlertCount();
                        }
                        else if (platform.Equals(Constants.Twitch))
                        {
                            _statisticsManager.AddToTwitchAlertCount();
                        }
                        else if (platform.Equals(Constants.YouTubeGaming))
                        {
                            _statisticsManager.AddToYouTubeAlertCount();
                        }
                        else if (platform.Equals(Constants.Picarto))
                        {
                            _statisticsManager.AddToPicartoAlertCount();
                        }
                        else if (platform.Equals(Constants.VidMe))
                        {
                            _statisticsManager.AddToVidMeAlertCount();
                        }

                    }
                    catch (Exception ex)
                    {
                        _logging.LogError("Send Message Error: " + ex.Message + " in server " + message.GuildId);
                    }
                }
            }

            return channelMessages;
        }

        public async Task<BroadcastMessage> BuildTestPublishedMessage(SocketUser user, ulong guildId, ulong channelId)
        {
            var server = await _guildManager.GetGuildById(guildId.ToString());

            if (server == null)
                return null;

            string url = "http://" + (server.YtgDomainPublished ? "gaming" : "www") + ".youtube.com/watch?v=B7wkzmZ4GBw";

            EmbedBuilder embed = new EmbedBuilder();
            EmbedAuthorBuilder author = new EmbedAuthorBuilder();
            EmbedFooterBuilder footer = new EmbedFooterBuilder();

            if (server.PublishedMessage == null)
            {
                server.PublishedMessage = "%CHANNEL% just published a new video.";
            }

            Color red = new Color(179, 18, 23);
            author.IconUrl = user.GetAvatarUrl() + "?_=" + Guid.NewGuid().ToString().Replace("-", "");
            author.Name = _client.CurrentUser.Username;
            author.Url = url;
            footer.Text = "[" + Constants.YouTube + "] - " + DateTime.UtcNow.AddHours(server.TimeZoneOffset);
            footer.IconUrl = "http://couchbot.io/img/ytg.jpg";
            embed.Author = author;
            embed.Color = red;
            embed.Description = server.PublishedMessage.Replace("%CHANNEL%", "Test Channel").Replace("%TITLE%", "Test Title").Replace("%URL%", url);

            embed.Title = "Test Channel published a new video!";
            embed.ThumbnailUrl = "http://couchbot.io/img/bot/vader.jpg" + "?_=" + Guid.NewGuid().ToString().Replace("-", "");
            embed.ImageUrl = server.AllowThumbnails ? "http://couchbot.io/img/bot/test_thumbnail.jpg" + "?_=" + Guid.NewGuid().ToString().Replace("-", "") : "";
            embed.Footer = footer;

            var guild = _client.GetGuild(guildId);
            var role = guild.Roles.FirstOrDefault(x => x.Id == ulong.Parse(server.MentionRole));

            if (role == null)
            {
                server.MentionRole = "0";
            }

            var message = (server.AllowEveryone ? server.MentionRole != "0" ? role.Mention : "@everyone " : "");

            if (server.UseTextAnnouncements)
            {
                if (!server.AllowThumbnails)
                {
                    url = "<" + url + ">";
                }

                message += "**[Test]** " + server.PublishedMessage.Replace("%CHANNEL%", "Test Channel").Replace("%TITLE%", "Test Title").Replace("%URL%", url);
            }

            var broadcastMessage = new BroadcastMessage()
            {
                GuildId = ulong.Parse(server.GuildId),
                ChannelId = channelId,
                UserId = "0",
                Message = message,
                Platform = "Test",
                Embed = (!server.UseTextAnnouncements ? embed.Build() : null)
            };

            return broadcastMessage;
        }

        public async Task<BroadcastMessage> BuildTestMessage(SocketUser user, ulong guildId, ulong channelId, string platform)
        {
            var server = await _guildManager.GetGuildById(guildId.ToString());

            if (server == null)
                return null;

            string gameName = "a game"; ;
            string url = "http://couchbot.io";

            EmbedBuilder embed = new EmbedBuilder();
            EmbedAuthorBuilder author = new EmbedAuthorBuilder();
            EmbedFooterBuilder footer = new EmbedFooterBuilder();

            if (server.LiveMessage == null)
            {
                server.LiveMessage = "%CHANNEL% just went live with %GAME% - %TITLE% - %URL%";
            }

            Color color = new Color(76, 144, 243);
            if (platform.Equals(Constants.Twitch))
            {
                color = new Color(100, 65, 164);
                footer.Text = "[" + Constants.Twitch + "] - " + DateTime.UtcNow.AddHours(server.TimeZoneOffset);
                footer.IconUrl = "http://couchbot.io/img/twitch.jpg";
            }
            else if (platform.Equals(Constants.YouTube) || platform.Equals(Constants.YouTubeGaming))
            {
                color = new Color(179, 18, 23);
                footer.Text = "[" + Constants.YouTubeGaming + "] - " + DateTime.UtcNow.AddHours(server.TimeZoneOffset);
                footer.IconUrl = "http://couchbot.io/img/ytg.jpg";
            }
            else if (platform.Equals(Constants.Smashcast))
            {
                color = new Color(153, 204, 0);
                footer.Text = "[" + Constants.Smashcast + "] - " + DateTime.UtcNow.AddHours(server.TimeZoneOffset);
                footer.IconUrl = "http://couchbot.io/img/smashcast.png";
            }
            else if (platform.Equals(Constants.Mixer))
            {
                color = new Color(76, 144, 243);
                footer.Text = "[" + Constants.Mixer + "] - " + DateTime.UtcNow.AddHours(server.TimeZoneOffset);
                footer.IconUrl = "http://couchbot.io/img/beam.jpg";
            }

            author.IconUrl = (user.GetAvatarUrl() != null ? user.GetAvatarUrl() : "http://couchbot.io/img/bot/discord.png") + "?_=" + Guid.NewGuid().ToString().Replace("-", "");
            author.Name = _client.CurrentUser.Username;
            author.Url = url;
            embed.Author = author;
            embed.Color = color;
            embed.Description = server.LiveMessage.Replace("%CHANNEL%", "Test Channel").Replace("%GAME%", gameName).Replace("%TITLE%", "Test Title").Replace("%URL%", url);
            embed.Title = "Test Channel has gone live!";
            embed.ThumbnailUrl = "http://couchbot.io/img/bot/vader.jpg" + "?_=" + Guid.NewGuid().ToString().Replace("-", "");
            embed.ImageUrl = server.AllowThumbnails ? "http://couchbot.io/img/bot/test_thumbnail.jpg" + "?_=" + Guid.NewGuid().ToString().Replace("-", "") : "";
            embed.Footer = footer;

            var guild = _client.GetGuild(guildId);
            var role = guild.Roles.FirstOrDefault(x => x.Id == ulong.Parse(server.MentionRole));

            if (role == null)
            {
                server.MentionRole = "0";
            }

            var message = (server.AllowEveryone ? server.MentionRole != "0" ? role.Mention : "@everyone " : "");

            if (server.UseTextAnnouncements)
            {
                if (!server.AllowThumbnails)
                {
                    url = "<" + url + ">";
                }

                message += "**[Test]** " + server.LiveMessage.Replace("%CHANNEL%", "Test Channel").Replace("%GAME%", gameName).Replace("%TITLE%", "Test Title").Replace("%URL%", url);
            }

            var broadcastMessage = new BroadcastMessage()
            {
                GuildId = ulong.Parse(server.GuildId),
                ChannelId = channelId,
                UserId = "0",
                Message = message,
                Platform = "Test",
                Embed = (!server.UseTextAnnouncements ? embed.Build() : null)
            };

            return broadcastMessage;
        }

    }
}
