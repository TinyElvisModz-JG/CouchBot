using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MTD.CouchBot.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTD.CouchBot.Bot.Modules
{
    [Group("channel")]
    public class Channel : SecurityModule
    {
        private readonly IGuildManager _guildManager;

        public Channel(DiscordShardedClient client, IGuildManager guildManager) : base(client)
        {
            _guildManager = guildManager;
        }

        [Command("announce"), Summary("Sets the server announcement channel.")]
        public async Task Announce(IGuildChannel guildChannel)
        {
            var guild = await _guildManager.GetGuildById(Context.Guild.Id.ToString());
            var user = ((IGuildUser)Context.Message.Author);

            if (!user.GuildPermissions.ManageGuild)
            {
                return;
            }

            guild.AnnouncementsChannel = guildChannel.Id.ToString();
            await _guildManager.UpdateGuild(guild);
            await Context.Channel.SendMessageAsync("The Announce Channel has been set.");
        }

        [Command("live"), Summary("Sets go live channel.")]
        public async Task Live(IGuildChannel guildChannel)
        {
            var guild = await _guildManager.GetGuildById(Context.Guild.Id.ToString());
            var user = ((IGuildUser)Context.Message.Author);

            if (!user.GuildPermissions.ManageGuild)
            {
                return;
            }

            guild.LiveChannel = guildChannel.Id.ToString();
            await _guildManager.UpdateGuild(guild);
            await Context.Channel.SendMessageAsync("The Live Channel has been set.");
        }

        [Command("ownerlive"), Summary("Sets owner live channel.")]
        public async Task OwnerLive(IGuildChannel guildChannel)
        {
            var guild = await _guildManager.GetGuildById(Context.Guild.Id.ToString());
            var user = ((IGuildUser)Context.Message.Author);

            if (!user.GuildPermissions.ManageGuild)
            {
                return;
            }

            guild.OwnerLiveChannel = guildChannel.Id.ToString();
            await _guildManager.UpdateGuild(guild);
            await Context.Channel.SendMessageAsync("The Owner Live Channel has been set.");
        }

        [Command("greetings"), Summary("Sets greetings channel.")]
        public async Task Greetings(IGuildChannel guildChannel)
        {
            var guild = await _guildManager.GetGuildById(Context.Guild.Id.ToString());
            var user = ((IGuildUser)Context.Message.Author);

            if (!user.GuildPermissions.ManageGuild)
            {
                return;
            }

            guild.GreetingsChannel = guildChannel.Id.ToString();
            await _guildManager.UpdateGuild(guild);
            await Context.Channel.SendMessageAsync("The Greetings Channel has been set.");
        }

        [Command("published"), Summary("Sets published video channel.")]
        public async Task Published(IGuildChannel guildChannel)
        {
            var guild = await _guildManager.GetGuildById(Context.Guild.Id.ToString());
            var user = ((IGuildUser)Context.Message.Author);

            if (!user.GuildPermissions.ManageGuild)
            {
                return;
            }

            guild.PublishedChannel = guildChannel.Id.ToString();
            await _guildManager.UpdateGuild(guild);
            await Context.Channel.SendMessageAsync("The Published Channel has been set.");
        }

        [Command("ownerpublished"), Summary("Sets owner published video channel.")]
        public async Task OwnerPublished(IGuildChannel guildChannel)
        {
            var guild = await _guildManager.GetGuildById(Context.Guild.Id.ToString());
            var user = ((IGuildUser)Context.Message.Author);

            if (!user.GuildPermissions.ManageGuild)
            {
                return;
            }

            guild.OwnerPublishedChannel = guildChannel.Id.ToString();
            await _guildManager.UpdateGuild(guild);
            await Context.Channel.SendMessageAsync("The Owner Published Channel has been set.");
        }

        [Command("ownertwitchfeed"), Summary("Sets owner twitch channel feed channel.")]
        public async Task OwnerTwitchFeedChannel(IGuildChannel guildChannel)
        {
            var guild = await _guildManager.GetGuildById(Context.Guild.Id.ToString());
            var user = ((IGuildUser)Context.Message.Author);

            if (!user.GuildPermissions.ManageGuild)
            {
                return;
            }

            guild.OwnerTwitchFeedChannel = guildChannel.Id.ToString();
            await _guildManager.UpdateGuild(guild);
            await Context.Channel.SendMessageAsync("The Owner Twitch Channel Feed Channel has been set.");
        }

        [Command("twitchfeed"), Summary("Sets twitch channel feed channel.")]
        public async Task TwitchFeedChannel(IGuildChannel guildChannel)
        {
            var guild = await _guildManager.GetGuildById(Context.Guild.Id.ToString());
            var user = ((IGuildUser)Context.Message.Author);

            if (!user.GuildPermissions.ManageGuild)
            {
                return;
            }

            guild.TwitchFeedChannel = guildChannel.Id.ToString();
            await _guildManager.UpdateGuild(guild);
            await Context.Channel.SendMessageAsync("The Twitch Channel Feed Channel has been set.");
        }

        //[Command("clear"), Summary("Clears channels settings for a guild.")]
        //public async Task Clear(string option)
        //{
        //    var guild = ((IGuildUser)Context.Message.Author).Guild;
        //    var user = ((IGuildUser)Context.Message.Author);

        //    if (!user.GuildPermissions.ManageGuild)
        //    {
        //        return;
        //    }

        //    var file = Constants.ConfigRootDirectory + Constants.GuildDirectory + guild.Id + ".json";
        //    var server = new DiscordServer();

        //    if (File.Exists(file))
        //        server = JsonConvert.DeserializeObject<DiscordServer>(File.ReadAllText(file));

        //    if (File.Exists(file))
        //    {
        //        option = option.ToLower();
        //        string label = "";

        //        switch (option)
        //        {
        //            case "live":
        //                guild.GoLiveChannel = 0;
        //                label = "Live Channel";
        //                break;
        //            case "ownerlive":
        //                guild.OwnerLiveChannel = 0;
        //                label = "Owner Live Channel";
        //                break;
        //            case "announce":
        //                guild.AnnouncementsChannel = 0;
        //                label = "Announcements";
        //                break;
        //            case "greetings":
        //                guild.GreetingsChannel = 0;
        //                label = "Greetings";
        //                break;
        //            case "ownerpublished":
        //                guild.OwnerPublishedChannel = 0;
        //                label = "Owner Published";
        //                break;
        //            case "published":
        //                guild.PublishedChannel = 0;
        //                label = "Published";
        //                break;
        //            case "ownertwitchfeed":
        //                guild.OwnerTwitchFeedChannel = 0;
        //                label = "Owner Twitch Feed";
        //                break;
        //            case "twitchfeed":
        //                guild.TwitchFeedChannel = 0;
        //                label = "Twitch Feed";
        //                break;
        //            case "all":
        //                guild.AnnouncementsChannel = 0;
        //                guild.GoLiveChannel = 0;
        //                guild.GreetingsChannel = 0;
        //                guild.PublishedChannel = 0;
        //                guild.OwnerPublishedChannel = 0;
        //                guild.OwnerLiveChannel = 0;
        //                guild.OwnerTwitchFeedChannel = 0;
        //                guild.TwitchFeedChannel = 0;
        //                label = "All";
        //                break;
        //            default:
        //                break;
        //        }

        //        if (!string.IsNullOrEmpty(label))
        //        {
        //            await BotFiles.SaveDiscordServer(server, Context.Guild);
        //            await Context.Channel.SendMessageAsync(label + " settings have been reset.");
        //        }
        //    }
        //}
    }
}
