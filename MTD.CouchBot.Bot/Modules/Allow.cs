using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using MTD.CouchBot.Managers;

namespace MTD.CouchBot.Bot.Modules
{
    [Group("allow")]
    public class Allow : SecurityModule
    {
        private readonly IGuildManager _guildManager;

        public Allow(DiscordShardedClient client, IGuildManager guildManager) : base(client)
        {
            _guildManager = guildManager;
        }

        [Command("mention")]
        public async Task Mention(string trueFalse)
        {
            var guild = await _guildManager.GetGuildById(Context.Guild.Id.ToString());
            var user = ((IGuildUser)Context.Message.Author);

            if (!user.GuildPermissions.ManageGuild)
            {
                return;
            }

            trueFalse = trueFalse.ToLower();
            if (!trueFalse.Equals("true") && !trueFalse.Equals("false"))
            {
                await Context.Channel.SendMessageAsync("Pass true or false when configuring AllowEveryone. (ie: !cb config AllowEveryone true)");
                return;
            }

            guild.AllowEveryone = bool.Parse(trueFalse);
            await _guildManager.UpdateGuild(guild);
            await Context.Channel.SendMessageAsync("Allow everyone has been set to: " + trueFalse);
        }

        [Command("thumbnails"), Summary("Sets use of thumbnails.")]
        public async Task Thumbnails(string trueFalse)
        {
            var guild = await _guildManager.GetGuildById(Context.Guild.Id.ToString());
            var user = ((IGuildUser)Context.Message.Author);

            if (!user.GuildPermissions.ManageGuild)
            {
                return;
            }

            trueFalse = trueFalse.ToLower();
            if (!trueFalse.Equals("true") && !trueFalse.Equals("false"))
            {
                await Context.Channel.SendMessageAsync("Pass true or false when configuring AllowThumbnails. (ie: !cb config AllowThumbnails true)");
                return;
            }

            guild.AllowThumbnails = bool.Parse(trueFalse);
            await Context.Channel.SendMessageAsync("Allow thumbnails has been set to: " + trueFalse);
        }

        [Command("live"), Summary("Sets announcing of published content.")]
        public async Task Live(string trueFalse)
        {
            var guild = await _guildManager.GetGuildById(Context.Guild.Id.ToString());
            var user = ((IGuildUser)Context.Message.Author);

            if (!user.GuildPermissions.ManageGuild)
            {
                return;
            }

            trueFalse = trueFalse.ToLower();
            if (!trueFalse.Equals("true") && !trueFalse.Equals("false"))
            {
                await Context.Channel.SendMessageAsync("Pass true or false when configuring allow live. (ie: !cb allow live true)");
                return;
            }

            guild.AllowLive = bool.Parse(trueFalse);
            await Context.Channel.SendMessageAsync("Allow live has been set to: " + trueFalse);
        }

        [Command("published"), Summary("Sets announcing of published content.")]
        public async Task Published(string trueFalse)
        {
            var guild = await _guildManager.GetGuildById(Context.Guild.Id.ToString());
            var user = ((IGuildUser)Context.Message.Author);

            if (!user.GuildPermissions.ManageGuild)
            {
                return;
            }

            trueFalse = trueFalse.ToLower();
            if (!trueFalse.Equals("true") && !trueFalse.Equals("false"))
            {
                await Context.Channel.SendMessageAsync("Pass true or false when configuring allow published. (ie: !cb allow published true)");
                return;
            }

            guild.AllowPublished = bool.Parse(trueFalse);
            await Context.Channel.SendMessageAsync("Allow published has been set to: " + trueFalse);
        }

        //[Command("goals"), Summary("Sets broadcasting of sub goals being met.")]
        //public async Task Goals(string trueFalse)
        //{
        //    var guild = await _guildManager.GetGuildById(Context.Guild.Id.ToString());
        //    var user = ((IGuildUser)Context.Message.Author);

        //    if (!user.GuildPermissions.ManageGuild)
        //    {
        //        return;
        //    }

        //    trueFalse = trueFalse.ToLower();
        //    if (!trueFalse.Equals("true") && !trueFalse.Equals("false"))
        //    {
        //        await Context.Channel.SendMessageAsync("Pass true or false when configuring BroadcastSubGoals. (ie: !cb config BroadcastSubGoals true)");
        //        return;
        //    }

        //    guild.BroadcastSubGoals = bool.Parse(trueFalse);
        //    await Context.Channel.SendMessageAsync("Allow sub goals has been set to: " + trueFalse);
        //}

        [Command("channelfeed"), Summary("Sets announcing of channel feed.")]
        public async Task ChannelFeed(string trueFalse)
        {
            var guild = await _guildManager.GetGuildById(Context.Guild.Id.ToString());
            var user = ((IGuildUser)Context.Message.Author);

            if (!user.GuildPermissions.ManageGuild)
            {
                return;
            }

            trueFalse = trueFalse.ToLower();
            if (!trueFalse.Equals("true") && !trueFalse.Equals("false"))
            {
                await Context.Channel.SendMessageAsync("Pass true or false when configuring allow channel feed. (ie: !cb allow channelfeed true)");
                return;
            }

            guild.AllowChannelFeed = bool.Parse(trueFalse);
            await Context.Channel.SendMessageAsync("Allow channel feed has been set to: " + trueFalse);
        }

        [Command("ownerchannelfeed"), Summary("Sets announcing of owner channel feed.")]
        public async Task ChannelFeedOwner(string trueFalse)
        {
            var guild = await _guildManager.GetGuildById(Context.Guild.Id.ToString());
            var user = ((IGuildUser)Context.Message.Author);

            if (!user.GuildPermissions.ManageGuild)
            {
                return;
            }

            trueFalse = trueFalse.ToLower();
            if (!trueFalse.Equals("true") && !trueFalse.Equals("false"))
            {
                await Context.Channel.SendMessageAsync("Pass true or false when configuring allow owner channel feed. (ie: !cb allow ownerchannelfeed true)");
                return;
            }

            guild.AllowOwnerChannelFeed = bool.Parse(trueFalse);
            await Context.Channel.SendMessageAsync("Allow owner channel feed has been set to: " + trueFalse);
        }
    }
}
