using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MTD.CouchBot.Managers;
using System.Threading.Tasks;

namespace MTD.CouchBot.Bot.Modules
{
    // Create a module with the 'sample' prefix
    [Group("goodbyes")]
    public class Goodbyes : SecurityModule
    {
        private readonly IGuildManager _guildManager;

        public Goodbyes(DiscordShardedClient client, IGuildManager guildManager) : base(client)
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

            guild.AllowGoodbyes = true;
            if (string.IsNullOrEmpty(guild.GoodbyeMessage))
            {
                guild.GoodbyeMessage = "Good bye, %USER%, thanks for hanging out!";
            }

            await _guildManager.UpdateGuild(guild);
            await Context.Channel.SendMessageAsync("Goodbyes have been turned on.");
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

            guild.AllowGoodbyes = false;
            await _guildManager.UpdateGuild(guild);
            await Context.Channel.SendMessageAsync("Goodbyes have been turned off.");
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

            guild.GoodbyeMessage = message;
            await _guildManager.UpdateGuild(guild);
            await Context.Channel.SendMessageAsync("Goodbye Message has been set.");
        }
    }
}
