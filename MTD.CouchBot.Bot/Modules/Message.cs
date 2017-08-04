using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MTD.CouchBot.Bot.Services;
using MTD.CouchBot.Domain;
using MTD.CouchBot.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTD.CouchBot.Bot.Modules
{
    [Group("message")]
    public class Message : SecurityModule
    {
        private readonly IGuildManager _guildManager;
        private readonly LoggingService _loggingService;
        private readonly MessagingService _messagingService;

        public Message(DiscordShardedClient client, IGuildManager guildManager, LoggingService loggingService,
            MessagingService messagingService) : base(client)
        {
            _guildManager = guildManager;
            _loggingService = loggingService;
            _messagingService = messagingService;
        }

        [Command("live")]
        public async Task Live(string message)
        {
            var guild = await _guildManager.GetGuildById(Context.Guild.Id.ToString());
            var user = ((IGuildUser)Context.Message.Author);

            if (!user.GuildPermissions.ManageGuild)
            {
                return;
            }

            if (message.ToLower().Equals("clear"))
            {
                guild.LiveMessage = "%CHANNEL% just went live with %GAME% - %TITLE% - %URL%";
                await Context.Channel.SendMessageAsync("Live Message has been reset to the default message.");
            }
            else
            {
                guild.LiveMessage = message;
                await Context.Channel.SendMessageAsync("Live Message has been set.");
            }

            await _guildManager.UpdateGuild(guild);
        }

        [Command("published")]
        public async Task Published(string message)
        {
            var guild = await _guildManager.GetGuildById(Context.Guild.Id.ToString());
            var user = ((IGuildUser)Context.Message.Author);

            if (!user.GuildPermissions.ManageGuild)
            {
                return;
            }
            
            if (message.ToLower().Equals("clear"))
            {
                guild.PublishedMessage = "%CHANNEL% just published a new video - %TITLE% - %URL%";
                await Context.Channel.SendMessageAsync("Published Message has been reset to the default message.");
            }
            else
            {
                guild.PublishedMessage = message;
                await Context.Channel.SendMessageAsync("Published Message has been set.");
            }

            await _guildManager.UpdateGuild(guild);
        }

        [Command("offline")]
        public async Task Offline(string message)
        {
            var guild = await _guildManager.GetGuildById(Context.Guild.Id.ToString());
            var user = ((IGuildUser)Context.Message.Author);

            if (!user.GuildPermissions.ManageGuild)
            {
                return;
            }
            
            if (message.ToLower().Equals("clear"))
            {
                guild.StreamOfflineMessage = "This stream is now offline.";
                await Context.Channel.SendMessageAsync("Stream Offline Message has been reset to the default message.");
            }
            else
            {
                guild.StreamOfflineMessage = message;
                await Context.Channel.SendMessageAsync("Stream Offline Message has been set.");
            }

            await _guildManager.UpdateGuild(guild);
        }

        [Command("testlive")]
        public async Task TestLive(string platform)
        {
            if (platform.ToLower() != Constants.Mixer.ToLower() && platform.ToLower() != Constants.YouTube.ToLower() && platform.ToLower() != Constants.Twitch.ToLower() && platform.ToLower() != Constants.Smashcast.ToLower())
            {
                await Context.Channel.SendMessageAsync("Please pass in mixer, smashcast, twitch, youtube or youtube gaming when requesting a test message. (ie: !cb message test youtube)");
                return;
            }

            var guild = await _guildManager.GetGuildById(Context.Guild.Id.ToString());

            var user = ((IGuildUser)Context.Message.Author);

            if (!user.GuildPermissions.ManageGuild)
            {
                return;
            }

            var message = await _messagingService.BuildTestMessage((SocketUser)Context.User, Context.Guild.Id, Context.Channel.Id, platform.ToLower());

            if (message != null)
            {
                try
                {
                    if (message.Embed != null)
                    {
                        RequestOptions options = new RequestOptions();
                        options.RetryMode = RetryMode.AlwaysRetry;
                        var msg = await Context.Channel.SendMessageAsync(message.Message, false, message.Embed, options);
                    }
                    else
                    {
                        var msg = await Context.Channel.SendMessageAsync(message.Message);
                    }
                }
                catch (Exception ex)
                {
                    _loggingService.LogError("Error in Message.Test Command: " + ex.Message);
                }
            }
        }

        [Command("testpublished")]
        public async Task TestPublished()
        {
            var guild = await _guildManager.GetGuildById(Context.Guild.Id.ToString());

            var user = ((IGuildUser)Context.Message.Author);

            if (!user.GuildPermissions.ManageGuild)
            {
                return;
            }

            var message = await _messagingService.BuildTestPublishedMessage((SocketUser)Context.User, Context.Guild.Id, Context.Channel.Id);

            if (message != null)
            {
                try
                {
                    if (message.Embed != null)
                    {
                        RequestOptions options = new RequestOptions();
                        options.RetryMode = RetryMode.AlwaysRetry;
                        var msg = await Context.Channel.SendMessageAsync(message.Message, false, message.Embed, options);
                    }
                    else
                    {
                        var msg = await Context.Channel.SendMessageAsync(message.Message);
                    }
                }
                catch (Exception ex)
                {
                    _loggingService.LogError("Error in Message.Test Command: " + ex.Message);
                }
            }
        }
    }
}
