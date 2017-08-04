using Microsoft.EntityFrameworkCore;
using MTD.CouchBot.Data.EF;
using MTD.CouchBot.Domain.Dtos.Bot;
using System.Threading.Tasks;

namespace MTD.CouchBot.Dals.Implementations
{
    public class BotDal : IBotDal
    {
        CouchDbContext _couchDbContext;

        public BotDal(CouchDbContext couchDbContext)
        {
            _couchDbContext = couchDbContext;
        }

        public async Task<Statistics> GetStatistics()
        {
            return await _couchDbContext.Statististics.FirstOrDefaultAsync(x => x.Id == 1);
        }

        public async Task SaveStatistics(Statistics stats)
        {
            _couchDbContext.Statististics.Update(stats);
            await _couchDbContext.SaveChangesAsync();
        }

        public async Task<Configuration> GetConfiguration()
        {
            return await _couchDbContext.Configuration.FirstOrDefaultAsync(x => x.Id == 1);
        }
    }
}
