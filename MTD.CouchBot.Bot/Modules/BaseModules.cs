using Discord;
using Discord.Commands;
using MTD.CouchBot.Domain.Utilities;
using System.Threading.Tasks;

namespace MTD.CouchBot.Bot.Modules
{
    public class BaseModules : ModuleBase
    {
        [Command("echo")]
        public async Task Echo(string message)
        {
            await Context.Channel.SendMessageAsync(StringUtilities.ScrubChatMessage(message));
        }

        [Command("echoembed")]
        public async Task EchoEmbed(string message)
        {
            EmbedBuilder eb = new EmbedBuilder();
            eb.AddField("ECHO!", StringUtilities.ScrubChatMessage(message));

            await Context.Channel.SendMessageAsync("", false, eb.Build(), null);
        }

        [Command("ping")]
        public async Task Ping()
        {
            await Context.Channel.SendMessageAsync("Pong!");
        }
    }
}
