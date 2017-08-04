using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MTD.CouchBot.Bot.Services;
using MTD.CouchBot.Dals;
using MTD.CouchBot.Dals.Implementations;
using MTD.CouchBot.Data.EF;
using MTD.CouchBot.Domain.Dtos.Bot;
using MTD.CouchBot.Managers;
using MTD.CouchBot.Managers.Implementations;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MTD.CouchBot.Bot
{
    class Program
    {
        // Discord
        private DiscordShardedClient _client;
        private IConfiguration _config;

        // Bot
        private Configuration _botConfig;

        // Managers
        IStatisticsManager _statisticsManager;
        IYouTubeManager _youtubeManager;
        ITwitchManager _twitchManager;
        ISmashcastManager _smashcastManager;
        IMixerManager _mixerManager;
        IPicartoManager _picartoManager;
        IVidMeManager _vidMeManager;
        IBotManager _botManager;

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
            await services.GetRequiredService<GuildInteractionService>().Initialize(services);
            var schedulingService = services.GetRequiredService<SchedulingService>();

            _statisticsManager = services.GetService<IStatisticsManager>();
            _youtubeManager = services.GetService<IYouTubeManager>();
            _twitchManager = services.GetService<ITwitchManager>();
            _smashcastManager = services.GetService<ISmashcastManager>();
            _mixerManager = services.GetService<IMixerManager>();
            _picartoManager = services.GetService<IPicartoManager>();
            _vidMeManager = services.GetService<IVidMeManager>();
            _botManager = services.GetService<IBotManager>();

            _botConfig = await _botManager.GetConfiguration();

            if (_botConfig.EnableTwitch)
            {
                schedulingService.QueueTwitchChecks();
            }

            if (_botConfig.EnableYouTube)
            {
                schedulingService.QueueYouTubeChecks();
            }

            if (_botConfig.EnableSmashcast)
            {
                schedulingService.QueueSmashcastChecks();
            }

            if (_botConfig.EnablePicarto)
            {
                schedulingService.QueuePicartoChecks();
            }

            if (_botConfig.EnableVidMe)
            {
                schedulingService.QueueVidMeChecks();
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
                // Database
                .AddDbContext<CouchDbContext>(db => db.UseSqlServer(_config.GetSection("ConnectionStrings")["CouchDb"]))
                // Discord
                .AddSingleton(_client)
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<LoggingService>()
                .AddSingleton<GuildInteractionService>()
                .AddSingleton<MessagingService>()
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
                .AddTransient<IGuildDal, GuildDal>()
                .AddTransient<IBotDal, BotDal>()
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
                .AddTransient<IGuildManager, GuildManager>()
                .AddTransient<IBotManager, BotManager>()
                // Misc
                .AddSingleton(_config)
                .BuildServiceProvider();
        }
    }
}