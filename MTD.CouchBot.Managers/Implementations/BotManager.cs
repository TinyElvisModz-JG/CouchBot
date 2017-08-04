using MTD.CouchBot.Dals;
using MTD.CouchBot.Domain.Dtos.Bot;
using System;
using System.Threading.Tasks;

namespace MTD.CouchBot.Managers.Implementations
{
    public class BotManager : IBotManager
    {
        private readonly IBotDal _botDal;

        public BotManager(IBotDal botDal)
        {
            _botDal = botDal;
        }

        public async Task<Configuration> GetConfiguration()
        {
            return await _botDal.GetConfiguration();
        }

        public async Task<Statistics> GetStatistics()
        {
            return await _botDal.GetStatistics();
        }

        public async Task SaveStatistics(Statistics stats)
        {
            await _botDal.SaveStatistics(stats);
        }
    }
}
