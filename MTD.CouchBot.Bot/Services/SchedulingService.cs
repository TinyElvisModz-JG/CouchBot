using MTD.CouchBot.Domain.Dtos.Bot;
using MTD.CouchBot.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace MTD.CouchBot.Bot.Services
{
    public class SchedulingService
    {
        private readonly LoggingService _logging;
        private readonly LiveService _live;
        private readonly IBotManager _botManager;
        private Configuration _configuration;

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

        public SchedulingService(LoggingService logging, LiveService live, IBotManager botManager)
        {
            _logging = logging;
            _live = live;
            _botManager = botManager;

            _configuration = (_botManager.GetConfiguration()).Result;
        }

        public void QueueSmashcastChecks()
        {
            _smashcastTimer = new Timer(async (e) =>
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                _logging.LogSmashcast("Checking Smashcast Channels.");
                await _live.CheckSmashcastLive();
                sw.Stop();
                _logging.LogSmashcast("Smashcast Check Complete - Elapsed Runtime: " + sw.ElapsedMilliseconds + " milliseconds.");
            }, null, 0, _configuration.SmashcastInterval);

            _smashcastOwnerTimer = new Timer(async (e) =>
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                _logging.LogSmashcast("Checking Owner Smashcast Channels.");
                await _live.CheckOwnerSmashcastLive();
                sw.Stop();
                _logging.LogSmashcast("Owner Smashcast Check Complete - Elapsed Runtime: " + sw.ElapsedMilliseconds + " milliseconds.");
            }, null, 0, _configuration.SmashcastInterval);
        }

        public void QueueTwitchChecks()
        {
            _twitchTimer = new Timer(async (e) =>
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                _logging.LogTwitch("Checking Twitch Channels.");
                await _live.CheckTwitchLive();
                sw.Stop();
                _logging.LogTwitch("Twitch Check Complete - Elapsed Runtime: " + sw.ElapsedMilliseconds + " milliseconds.");
            }, null, 0, _configuration.TwitchInterval);

            _twitchOwnerTimer = new Timer(async (e) =>
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                _logging.LogTwitch("Checking Owner Twitch Channels.");
                await _live.CheckOwnerTwitchLive();
                sw.Stop();
                _logging.LogTwitch("Owner Twitch Check Complete - Elapsed Runtime: " + sw.ElapsedMilliseconds + " milliseconds.");
            }, null, 0, _configuration.TwitchInterval);

            _twitchFeedTimer = new Timer(async (e) =>
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                _logging.LogTwitch("Checking Twitch Channel Feeds.");
                await _live.CheckTwitchChannelFeeds();
                sw.Stop();
                _logging.LogTwitch("Twitch Channel Feed Check Complete - Elapsed Runtime: " + sw.ElapsedMilliseconds + " milliseconds.");
            }, null, 0, _configuration.TwitchFeedInterval);

            _twitchOwnerFeedTimer = new Timer(async (e) =>
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                _logging.LogTwitch("Checking Owner Twitch Channel Feeds.");
                await _live.CheckTwitchOwnerChannelFeeds();
                sw.Stop();
                _logging.LogTwitch("Owner Twitch Channel Feed Check Complete - Elapsed Runtime: " + sw.ElapsedMilliseconds + " milliseconds.");
            }, null, 0, _configuration.TwitchFeedInterval);

            _twitchTeamTimer = new Timer(async (e) =>
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                _logging.LogTwitch("Checking Twitch Teams.");
                await _live.CheckTwitchTeams();
                sw.Stop();
                _logging.LogTwitch("Checking Twitch Teams Check Complete - Elapsed Runtime: " + sw.ElapsedMilliseconds + " milliseconds.");
            }, null, 0, _configuration.TwitchInterval);

            _twitchGameTimer = new Timer(async (e) =>
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                _logging.LogTwitch("Checking Twitch Games.");
                await _live.CheckTwitchGames();
                sw.Stop();
                _logging.LogTwitch("Checking Twitch Games Check Complete - Elapsed Runtime: " + sw.ElapsedMilliseconds + " milliseconds.");
            }, null, 0, _configuration.TwitchInterval);
        }

        public void QueueYouTubeChecks()
        {
            _youtubeTimer = new Timer(async (e) =>
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                _logging.LogYouTubeGaming("Checking YouTube Gaming Channels.");
                await _live.CheckYouTubeLive();
                sw.Stop();
                _logging.LogYouTubeGaming("YouTube Gaming Check Complete - Elapsed Runtime: " + sw.ElapsedMilliseconds + " milliseconds.");
            }, null, 0, _configuration.YouTubeLiveInterval);

            _youtubeOwnerTimer = new Timer(async (e) =>
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                _logging.LogYouTubeGaming("Checking Owner YouTube Gaming Channels.");
                await _live.CheckOwnerYouTubeLive();
                sw.Stop();
                _logging.LogYouTubeGaming("Owner YouTube Gaming Check Complete - Elapsed Runtime: " + sw.ElapsedMilliseconds + " milliseconds.");
            }, null, 0, _configuration.YouTubeLiveInterval);

            _youtubePublishedTimer = new Timer(async (e) =>
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                _logging.LogYouTube("Checking YouTube Published");
                await _live.CheckPublishedYouTube();
                sw.Stop();
                _logging.LogYouTube("YouTube Published Complete - Elapsed Runtime: " + sw.ElapsedMilliseconds + " milliseconds.");
            }, null, 0, _configuration.YouTubePublishedInterval);

            _youtubePublishedOwnerTimer = new Timer(async (e) =>
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                _logging.LogYouTube("Checking Owner YouTube Published");
                await _live.CheckOwnerPublishedYouTube();
                sw.Stop();
                _logging.LogYouTube("Owner YouTube Published Complete - Elapsed Runtime: " + sw.ElapsedMilliseconds + " milliseconds.");
            }, null, 0, _configuration.YouTubePublishedInterval);
        }

        public void QueueVidMeChecks()
        {
            _vidMeTimer = new Timer(async (e) =>
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                _logging.LogVidMe("Checking VidMe");
                await _live.CheckVidMe();
                sw.Stop();
                _logging.LogVidMe("VidMe Complete - Elapsed Runtime: " + sw.ElapsedMilliseconds + " milliseconds.");
            }, null, 0, _configuration.VidMeInterval);

            _vidMeOwnerTimer = new Timer(async (e) =>
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                _logging.LogVidMe("Checking Owner VidMe Published");
                await _live.CheckOwnerVidMe();
                sw.Stop();
                _logging.LogVidMe("Owner VidMe Complete - Elapsed Runtime: " + sw.ElapsedMilliseconds + " milliseconds.");
            }, null, 0, _configuration.VidMeInterval);
        }

        public void QueuePicartoChecks()
        {
            _picartoTimer = new Timer(async (e) =>
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                _logging.LogPicarto("Checking Picarto Channels.");
                await _live.CheckPicartoLive();
                sw.Stop();
                _logging.LogPicarto("Picarto Check Complete - Elapsed Runtime: " + sw.ElapsedMilliseconds + " milliseconds.");
            }, null, 0, _configuration.PicartoInterval);

            _picartoOwnerTimer = new Timer(async (e) =>
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                _logging.LogPicarto("Checking Picarto Smashcast Channels.");
                await _live.CheckOwnerPicartoLive();
                sw.Stop();
                _logging.LogPicarto("Owner Picarto Check Complete - Elapsed Runtime: " + sw.ElapsedMilliseconds + " milliseconds.");
            }, null, 0, _configuration.PicartoInterval);
        }
    }
}
