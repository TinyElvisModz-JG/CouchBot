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
    // Create a module with the 'sample' prefix
    [Group("greetings")]
    public class Greetings : SecurityModule
    {
        private readonly IGuildManager _guildManager;

        public Greetings(DiscordShardedClient client, IGuildManager guildManager) : base(client)
        {
            _guildManager = guildManager;
        }

        [Command("on")]
        public async Task On()
        {
            var guild = await _guildManager.GetGuildById(Context.Guild.Id.ToString());
            var user = ((IGuildUser)Context.Message.Author);

            if (!user.GuildPermissions.ManageGuild)
            {
                return;
            }

            guild.AllowGreetings = true;
            if (string.IsNullOrEmpty(guild.GreetingMessage))
            {
                guild.GreetingMessage = "Welcome to the server, %USER%";
                guild.GoodbyeMessage = "Good bye, %USER%, thanks for hanging out!";
            }

            await _guildManager.UpdateGuild(guild);
            await Context.Channel.SendMessageAsync("Greetings have been turned on.");
        }

        [Command("off")]
        public async Task Off()
        {
            var guild = await _guildManager.GetGuildById(Context.Guild.Id.ToString());
            var user = ((IGuildUser)Context.Message.Author);

            if (!user.GuildPermissions.ManageGuild)
            {
                return;
            }

            guild.AllowGreetings = false;
            await _guildManager.UpdateGuild(guild);
            await Context.Channel.SendMessageAsync("Greetings have been turned off.");
        }

        [Command("set")]
        public async Task Set(string message)
        {
            var guild = await _guildManager.GetGuildById(Context.Guild.Id.ToString());

            var user = ((IGuildUser)Context.Message.Author);

            if (!user.GuildPermissions.ManageGuild)
            {
                return;
            }

            guild.GreetingMessage = message;
            await _guildManager.UpdateGuild(guild);
            await Context.Channel.SendMessageAsync("Greeting has been set.");
        }
    }
}
