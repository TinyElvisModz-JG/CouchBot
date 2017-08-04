using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MTD.CouchBot.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTD.CouchBot.Bot.Modules
{
    [Group("config")]
    public class Config : SecurityModule
    {
        private readonly IGuildManager _guildManager;

        public Config(DiscordShardedClient client, IGuildManager guildManager) : base(client)
        {
            _guildManager = guildManager;
        }

        [Command("timezoneoffset")]
        public async Task TimeZoneOffset(float offset)
        {
            var guild = await _guildManager.GetGuildById(Context.Guild.Id.ToString());
            var user = ((IGuildUser)Context.Message.Author);

            if (!user.GuildPermissions.ManageGuild)
            {
                return;
            }

            guild.TimeZoneOffset = offset;
            await _guildManager.UpdateGuild(guild);
            await Context.Channel.SendMessageAsync("Your Server Time Zone Offset has been set.");
        }

        [Command("textannouncements")]
        public async Task TextAnnouncements(string trueFalse)
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
                await Context.Channel.SendMessageAsync("Pass true or false when configuring Text Announcements. (ie: !cb config textannouncements true)");
                return;
            }

            guild.UseTextAnnouncements = bool.Parse(trueFalse);
            await _guildManager.UpdateGuild(guild);
            await Context.Channel.SendMessageAsync("Text announcements has been set to: " + trueFalse);
        }

        [Command("list")]
        public async Task List()
        {
            var guild = await _guildManager.GetGuildById(Context.Guild.Id.ToString());
            var user = ((IGuildUser)Context.Message.Author);

            if (!user.GuildPermissions.ManageGuild)
            {
                return;
            }

            if (guild != null)
            {
                var discordGuild = _client.GetGuild(ulong.Parse(guild.GuildId));

                var announce = (IGuildChannel) _client.GetChannel(ulong.Parse(guild.AnnouncementsChannel));
                var announceChannel = announce != null ? announce.Name : "Not Set";

                var golive = (IGuildChannel) _client.GetChannel(ulong.Parse(guild.LiveChannel));
                var goliveChannel = golive != null ? golive.Name : "Not Set";

                var ownerGolive = (IGuildChannel) _client.GetChannel(ulong.Parse(guild.OwnerLiveChannel));
                var ownerGoliveChannel = ownerGolive != null ? ownerGolive.Name : "Not Set";

                var greetings = (IGuildChannel) _client.GetChannel(ulong.Parse(guild.GreetingsChannel));
                var greetingsChannel = greetings != null ? greetings.Name : "Not Set";

                var vod = (IGuildChannel) _client.GetChannel(ulong.Parse(guild.PublishedChannel));
                var vodChannel = vod != null ? vod.Name : "Not Set";

                var ownerVod = (IGuildChannel) _client.GetChannel(ulong.Parse(guild.OwnerPublishedChannel));
                var ownerVodChannel = ownerVod != null ? ownerVod.Name : "Not Set";

                var ownerTwitchFeed = (IGuildChannel) _client.GetChannel(ulong.Parse(guild.OwnerTwitchFeedChannel));
                var ownerTwitchFeedChannel = ownerTwitchFeed != null ? ownerTwitchFeed.Name : "Not Set";

                var role = discordGuild.Roles.FirstOrDefault(r => r.Id == ulong.Parse(guild.MentionRole));

                string info = "```Markdown\r\n" +
                              "# " + discordGuild.Name + " Configuration Settings\r\n" +
                              "- Owner Go Live Channel: " + ownerGoliveChannel + "\r\n" +
                              "- Owner Published Channel: " + ownerVodChannel + "\r\n" +
                              "- Owner Twitch Channel Feed Channel: " + ownerTwitchFeedChannel + "\r\n" +
                              "- Go Live Channel: " + goliveChannel + "\r\n" +
                              "- Published Channel: " + vodChannel + "\r\n" +
                              "- Greetings Channel: " + greetingsChannel + "\r\n" +
                              "- Allow @ Role: " + guild.AllowEveryone + "\r\n" +
                              "- Allow Thumbnails: " + guild.AllowThumbnails + "\r\n" +
                              "- Allow Greetings: " + guild.AllowGreetings + "\r\n" +
                              "- Allow Goodbyes: " + guild.AllowGoodbyes + "\r\n" +
                              "- Allow Live Content: " + guild.AllowLive + "\r\n" +
                              "- Allow Published Content: " + guild.AllowPublished + "\r\n" +
                              "- Allow Owner Twitch Channel Feed: " + guild.AllowOwnerChannelFeed + "\r\n" +
                              "- Use Text Announcements: " + guild.UseTextAnnouncements + "\r\n" +
                              "- Use YTG URLS For VOD Content: " + guild.YtgDomainPublished + "\r\n" +
                              "- Live Message: " + (string.IsNullOrEmpty(guild.LiveMessage) ? "Default" : guild.LiveMessage) + "\r\n" +
                              "- Published Message: " + (string.IsNullOrEmpty(guild.PublishedMessage) ? "Default" : guild.PublishedMessage) + "\r\n" +
                              "- Greeting Message: " + (string.IsNullOrEmpty(guild.GreetingMessage) ? "Default" : guild.GreetingMessage) + "\r\n" +
                              "- Goodbye Message: " + (string.IsNullOrEmpty(guild.GoodbyeMessage) ? "Default" : guild.GoodbyeMessage) + "\r\n" +
                              "- Stream Offline Message: " + (string.IsNullOrEmpty(guild.StreamOfflineMessage) ? "Default" : guild.StreamOfflineMessage) + "\r\n" +
                              "- Mention Role: " + ((guild.MentionRole == "0") ? "Everyone" : role.Name.Replace("@", "")) + "\r\n" +
                              "- Time Zone Offset: " + guild.TimeZoneOffset + "\r\n" +
                              "```\r\n";

                await Context.Channel.SendMessageAsync(info);
            }
        }

        [Command("publishedytg")]
        public async Task PublishedYtg(string trueFalse)
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
                await Context.Channel.SendMessageAsync("Pass true or false when configuring PublishedYTG. (ie: !cb config publishedytg true)");
                return;
            }
            
            guild.YtgDomainPublished = bool.Parse(trueFalse);
            await _guildManager.UpdateGuild(guild);
            await Context.Channel.SendMessageAsync("Publised YTG has been set to: " + trueFalse);
        }

        [Command("deleteoffline")]
        public async Task DeleteWhenOffline(string trueFalse)
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
                await Context.Channel.SendMessageAsync("Pass true or false when configuring DeleteOffline. (ie: !cb config deleteoffline true)");
                return;
            }

            guild.DeleteWhenOffline = bool.Parse(trueFalse);
            await _guildManager.UpdateGuild(guild);
            await Context.Channel.SendMessageAsync("Delete Offline has been set to: " + trueFalse);
        }

        [Command("mentionrole"), Summary("Set the role to mention instead of Everyone.")]
        public async Task MentionRole(IRole role)
        {
            var guild = await _guildManager.GetGuildById(Context.Guild.Id.ToString());
            var user = ((IGuildUser)Context.Message.Author);

            if (!user.GuildPermissions.ManageGuild)
            {
                return;
            }

            if (role.Name.ToLower().Contains("everyone"))
            {
                guild.MentionRole = "0";
            }
            else
            {
                guild.MentionRole = role.Id.ToString();
            }

            await _guildManager.UpdateGuild(guild);
            await Context.Channel.SendMessageAsync("Mention Role has been set to: " + role.Name);
        }
    }
}
