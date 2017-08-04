using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MTD.CouchBot.Domain.Dtos.Live;
using MTD.CouchBot.Domain.Enumerations;
using MTD.CouchBot.Data.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MTD.CouchBot.Dals.Implementations
{
    public class LiveQueueDal : ILiveQueueDal
    {
        CouchDbContext _couchDbContext;

        public LiveQueueDal(CouchDbContext couchDbContext)
        {
            _couchDbContext = couchDbContext;
        }

        public async Task<List<Queue>> GetAllLiveChannels()
        {
            return await _couchDbContext.LiveQueue.ToListAsync();
        }

        public async Task<List<Queue>> GetLiveChannelsByPlatformId(Platform platform)
        {
            return await _couchDbContext.LiveQueue.Where(x => x.PlatformId == ((int)platform)).ToListAsync();
        }

        public async Task UpdateLiveChannel(Queue queue)
        {
            _couchDbContext.LiveQueue.Update(queue);
            await _couchDbContext.SaveChangesAsync();
        }
    }
}
