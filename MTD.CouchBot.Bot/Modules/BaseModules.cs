using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MTD.CouchBot.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MTD.CouchBot.Bot.Modules
{
    public class BaseModules : SecurityModule
    {
        public BaseModules(DiscordShardedClient client) : base(client)
        {

        }

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
            var then = Context.Message.Timestamp;
            var now = DateTime.UtcNow;
            TimeSpan ts = now - then;

            await Context.Channel.SendMessageAsync("Pong! *(" + ts.TotalMilliseconds + "ms)*");
        }


        [Command("invite")]
        public async Task Invite()
        {
            await (await ((IGuildUser)(Context.Message.Author)).GetOrCreateDMChannelAsync()).SendMessageAsync("Want me to join your server? Click here: <https://discordapp.com/oauth2/authorize?client_id=308371905667137536&scope=bot&permissions=158720>");
        }

        [Command("setbotgame")]
        public async Task SetBotGame(string game)
        {
            if (Context.User.Id != 93015586698727424)
            {
                await Context.Channel.SendMessageAsync("*Bbbbbzzztttt* You are not *zzzzt* Dawgeth. Acc *bbrrrttt* Access Denied.");

                return;
            }
            
            await _client.SetGameAsync(game, "", StreamType.NotStreaming);
        }


        [Command("purge")]
        public async Task Purge(IGuildUser user)
        {
            var deleteList = new List<IMessage>();

            if (!IsAdmin)
            {
                return;
            }

            var messages = Context.Channel.GetMessagesAsync(100);
            var message = (await messages.Flatten()).GetEnumerator();

            while (message.MoveNext())
            {
                if (message.Current.Author.Id == user.Id)
                {
                    deleteList.Add(message.Current);
                }
            }

            if (deleteList.Count > 0)
            {
                await Context.Channel.DeleteMessagesAsync(deleteList);
                await Context.Message.DeleteAsync();
            }
        }

        [Command("purgeall")]
        public async Task PurgeAll()
        { 
            var deleteList = new List<IMessage>();

            if (!IsAdmin)
            {
                return;
            }

            var messages = Context.Channel.GetMessagesAsync(100);
            var message = (await messages.Flatten()).GetEnumerator();

            while (message.MoveNext())
            {
                deleteList.Add(message.Current);
            }

            if (deleteList.Count > 0)
            {
                await Context.Channel.DeleteMessagesAsync(deleteList);
            }
        }
    }
}
