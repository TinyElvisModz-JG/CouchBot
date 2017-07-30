using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using MTD.CouchBot.Domain;
using MTD.CouchBot.Domain.Dtos.Discord;
using MTD.CouchBot.Managers;
using MTD.CouchBot.Managers.Implementations;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace MTD.CouchBot.Bot.Services
{
    public class GuildInteractionService
    {
        private readonly CommandService _commands;
        private readonly IConfiguration _config;
        private readonly DiscordShardedClient _client;
        private IServiceProvider _provider;
        private readonly IGuildManager _guildManager;

        public GuildInteractionService(IServiceProvider provider, DiscordShardedClient client, IConfiguration config, CommandService commands, IGuildManager guildManager)
        {
            _commands = commands;
            _config = config;
            _client = client;
            _provider = provider;
            _guildManager = guildManager;
        }

        public async Task Initialize(IServiceProvider provider)
        {
            _provider = provider;
            _client.JoinedGuild += JoinedGuild;
            _client.LeftGuild += LeftGuild;
            _client.UserJoined += UserJoined;
            _client.UserLeft += UserLeft;
        }

        private async Task UserLeft(SocketGuildUser arg)
        {
            var guild = await _guildManager.GetGuildById(arg.Guild.Id.ToString());

            if(guild != null 
               && !string.IsNullOrEmpty(guild.GreetingsChannel) 
               && !string.IsNullOrEmpty(guild.GoodbyeMessage)
               && guild.AllowGoodbyes)
            {
                var channel = (IMessageChannel)_client.GetChannel(ulong.Parse(guild.GreetingsChannel));

                await channel.SendMessageAsync(guild.GoodbyeMessage.Replace("%USER%", arg.Username).Replace("%NEWLINE%", "\r\n"));
            }
        }

        private async Task UserJoined(SocketGuildUser arg)
        {
            var guild = await _guildManager.GetGuildById(arg.Guild.Id.ToString());

            if(guild != null
               && !string.IsNullOrEmpty(guild.GreetingsChannel)
               && !string.IsNullOrEmpty(guild.GreetingMessage)
               && guild.AllowGreetings)
            {
                var channel = (IMessageChannel)_client.GetChannel(ulong.Parse(guild.GreetingsChannel));

                await channel.SendMessageAsync(guild.GreetingMessage.Replace("%USER%", arg.Mention).Replace("%NEWLINE%", "\r\n"));
            }
        }

        private async Task LeftGuild(SocketGuild arg)
        {
            var guild = await _guildManager.GetGuildById(arg.Id.ToString());

            if(guild != null)
            {
                await _guildManager.RemoveGuild(guild);
            }
        }

        private async Task JoinedGuild(SocketGuild arg)
        {
            var existingGuild = await _guildManager.GetGuildById(arg.Id.ToString());

            if (existingGuild == null)
            {
                var guild = new Domain.Dtos.Discord.Guild
                {
                    AllowChannelFeed = true,
                    AllowEveryone = true,
                    AllowGoodbyes = true,
                    AllowGreetings = true,
                    AllowLive = true,
                    AllowOwnerChannelFeed = true,
                    AllowPublished = true,
                    AllowThumbnails = true,
                    AnnouncementsChannel = "0",
                    YtgDomainPublished = true,
                    UseTextAnnouncements = true,
                    DeleteWhenOffline = true,
                    GoodbyeMessage = Constants.DefaultGoodbyeMessage,
                    GreetingMessage = Constants.DefaultGreetingMessage,
                    GreetingsChannel = "0",
                    LiveChannel = "0",
                    LiveMessage = Constants.DefaultLiveMessage,
                    MentionRole = "0",
                    OwnerId = arg.OwnerId.ToString(),
                    OwnerLiveChannel = "0",
                    OwnerPublishedChannel = "0",
                    OwnerTwitchFeedChannel = "0",
                    PublishedChannel = "0",
                    PublishedMessage = Constants.DefaultPublishedMessage,
                    StreamOfflineMessage = Constants.DefaultOfflineMessage,
                    TimeZoneOffset = 0.0,
                    TwitchFeedChannel = "0",
                    GuildId = arg.Id.ToString()
                };

                await _guildManager.AddNewGuild(guild);
            }
        }
    }
}
