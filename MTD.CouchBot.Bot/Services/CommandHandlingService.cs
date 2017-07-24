using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace MTD.CouchBot.Bot.Services
{
    public class CommandHandlingService
    {
        private readonly CommandService _commands;
        private readonly IConfiguration _config;
        private readonly DiscordShardedClient _client;
        private IServiceProvider _provider;

        public CommandHandlingService(IServiceProvider provider, DiscordShardedClient client, IConfiguration config, CommandService commands)
        {
            _commands = commands;
            _config = config;
            _client = client;
            _provider = provider;

            _client.MessageReceived += MessageReceived;
        }

        public async Task Initialize(IServiceProvider provider)
        {
            _provider = provider;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
            var test = _commands.Commands;
        }

        private async Task MessageReceived(SocketMessage rawMessage)
        {
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != Discord.MessageSource.User) return;

            int argPos = 0;
            if (!(message.HasMentionPrefix(_client.CurrentUser, ref argPos) || 
                message.HasStringPrefix(_config.GetSection("BotSettings")["Prefix"] + " ", ref argPos))) return;

            var context = new ShardedCommandContext(_client, message);
            var result = await _commands.ExecuteAsync(context, argPos, _provider);

            if(result.Error.HasValue &&
                result.Error.Value != CommandError.UnknownCommand)
            {
                await context.Channel.SendMessageAsync(result.ToString());
            }

            else if(result.Error.HasValue &&
                result.Error.Value == CommandError.UnknownCommand)
            {
                await context.Channel.SendMessageAsync("Invalid Command.");
            }
        }
    }
}
