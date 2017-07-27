using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MTD.CouchBot.Bot.Services;
using MTD.CouchBot.Dals;
using MTD.CouchBot.Dals.Implementations;
using MTD.CouchBot.Domain;
using MTD.CouchBot.Managers;
using MTD.CouchBot.Managers.Implementations;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MTD.CouchBot.Bot
{
    class Program
    {
        // Discord
        private DiscordShardedClient _client;
        private IConfiguration _config;

        // Timers
        private static Timer _smashcastTimer;
        private static Timer _smashcastOwnerTimer;
                             
        private static Timer _twitchTimer;
        private static Timer _twitchOwnerTimer;
        private static Timer _twitchFeedTimer;
        private static Timer _twitchOwnerFeedTimer;
        private static Timer _twitchTeamTimer;
        private static Timer _twitchGameTimer;
                             
        private static Timer _youtubeTimer;
        private static Timer _youtubeOwnerTimer;
        private static Timer _youtubePublishedTimer;
        private static Timer _youtubePublishedOwnerTimer;
                             
        private static Timer _picartoTimer;
        private static Timer _picartoOwnerTimer;
                             
        private static Timer _vidMeTimer;
        private static Timer _vidMeOwnerTimer;
                             
        private static Timer _cleanupTimer;
        private static Timer _uptimeTimer;

        // Managers
        IStatisticsManager _statisticsManager;
        IYouTubeManager _youtubeManager;
        ITwitchManager _twitchManager;
        ISmashcastManager _smashcastManager;
        IMixerManager _mixerManager;
        IPicartoManager _picartoManager;
        IVidMeManager _vidMeManager;

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

            _statisticsManager = services.GetService<StatisticsManager>();
            _youtubeManager = services.GetService<YouTubeManager>();
            _twitchManager = services.GetService<TwitchManager>();
            _smashcastManager = services.GetService<SmashcastManager>();
            _mixerManager = services.GetService<MixerManager>();
            _picartoManager = services.GetService<PicartoManager>();
            _vidMeManager = services.GetService<VidMeManager>();

            if (Constants.EnableTwitch)
            {
                QueueTwitchChecks();
            }

            if (Constants.EnableYouTube)
            {
                QueueYouTubeChecks();
            }

            if (Constants.EnableSmashcast)
            {
                QueueHitboxChecks();
            }

            if (Constants.EnablePicarto)
            {
                QueuePicartoChecks();
            }

            if (Constants.EnableVidMe)
            {
                QueueVidMeChecks();
            }

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