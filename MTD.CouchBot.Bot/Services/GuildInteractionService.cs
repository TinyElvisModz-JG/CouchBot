using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
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
        }

        private async Task JoinedGuild(SocketGuild arg)
        {
            var existingGuild = await _guildManager.GetGuildById(arg.Id.ToString());

            if (existingGuild != null)
            {
                //List<User> users = new List<User>();

                //foreach(var user in arg.Users)
                //{
                //    users.Add(new User
                //    {
                //        CreatedDate = DateTime.UtcNow,
                //        UserId = user.Id.ToString()
                //    });
                //}

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
                    GoodbyeMessage = "Test",
                    GreetingMessage = "Test",
                    GreetingsChannel = "0",
                    LiveChannel = "0",
                    LiveMessage = "Test",
                    MentionRole = "0",
                    OwnerId = "0",
                    OwnerLiveChannel = "0",
                    OwnerPublishedChannel = "0",
                    OwnerTwitchFeedChannel = "0",
                    PublishedChannel = "0",
                    PublishedMessage = "Test",
                    StreamOfflineMessage = "Test",
                    TimeZoneOffset = 0,
                    TwitchFeedChannel = "0",
                    GuildId = arg.Id.ToString()
                };

                await _guildManager.AddNewGuild(guild);
            }
        }
    }
}
