using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace MTD.CouchBot.Bot.Modules
{
    public class SecurityModule : ModuleBase
    {
        public readonly DiscordShardedClient _client;

        public SecurityModule(DiscordShardedClient client)
        {
            _client = client;
        }

        public bool IsAdmin
        {
            get
            {
                var user = ((IGuildUser)Context.Message.Author);

                if (!user.GuildPermissions.ManageGuild)
                {
                    return false;
                }

                return true;
            }
        }
    }
}
