using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MTD.CouchBot.Bot.Services;
using MTD.CouchBot.Dals;
using MTD.CouchBot.Dals.Implementations;
using MTD.CouchBot.Managers;
using MTD.CouchBot.Managers.Implementations;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MTD.CouchBot.Bot
{
    class Program
    {
        private DiscordShardedClient _client;
        private IConfiguration _config;

        static void Main(string[] args) => new Program().Start().GetAwaiter().GetResult();

        public async Task Start()
        {
            await DoBotStuff().ConfigureAwait(false);
        }

        private async Task DoBotStuff()
        {
            _client = new DiscordShardedClient(new DiscordSocketConfig
            {
                TotalShards = 2
            });
            _config = BuildConfig();

            var services = ConfigureServices();
            await services.GetRequiredService<CommandHandlingService>().Initialize(services);

            await _client.LoginAsync(TokenType.Bot, _config.GetSection("Credentials")["DiscordToken"]);
            await _client.StartAsync();

            await Task.Delay(-1).ConfigureAwait(false);
        }

        private IConfiguration BuildConfig()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json")
                .Build();
        }

        private IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                // Discord
                .AddSingleton(_client)
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                // Managers
                .AddTransient<IApiAiDal, ApiAiDal>()
                .AddTransient<IMixerDal, MixerDal>()
                .AddTransient<IPicartoDal, PicartoDal>()
                .AddTransient<ISmashcastDal, SmashcastDal>()
                .AddTransient<IStatisticsDal, StatisticsDal>()
                .AddTransient<IStrawPollDal, StrawPollDal>()
                .AddTransient<ITwitchDal, TwitchDal>()
                .AddTransient<IVidMeDal, VidMeDal>()
                .AddTransient<IYouTubeDal, YouTubeDal>()
                // Dals
                .AddTransient<IApiAiManager, ApiAiManager>()
                .AddTransient<IMixerManager, MixerManager>()
                .AddTransient<IPicartoManager, PicartoManager>()
                .AddTransient<ISmashcastManager, SmashcastManager>()
                .AddTransient<IStatisticsManager, StatisticsManager>()
                .AddTransient<IStrawPollManager, StrawPollManager>()
                .AddTransient<ITwitchManager, TwitchManager>()
                .AddTransient<IVidMeManager, VidMeManager>()
                .AddTransient<IYouTubeManager, YouTubeManager>()
                // Misc
                .AddSingleton(_config)
                .BuildServiceProvider();
        }
    }
}