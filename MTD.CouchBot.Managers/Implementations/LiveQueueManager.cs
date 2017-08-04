using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MTD.CouchBot.Domain.Dtos.Live;
using MTD.CouchBot.Domain.Enumerations;
using MTD.CouchBot.Dals;

namespace MTD.CouchBot.Managers.Implementations
{
    public class LiveQueueManager : ILiveQueueManager
    {
        private readonly ILiveQueueDal _liveQueueDal;

        public LiveQueueManager(ILiveQueueDal liveQueueDal)
        {
            _liveQueueDal = liveQueueDal;
        }

        public async Task<List<Queue>> GetAllLiveChannels()
        {
            return await _liveQueueDal.GetAllLiveChannels();
        }

        public async Task<List<Queue>> GetLiveChannelsByPlatformId(Platform platform)
        {
            return await _liveQueueDal.GetLiveChannelsByPlatformId(platform);
        }

        public async Task UpdateLiveChannel(Queue queue)
        {
            await _liveQueueDal.UpdateLiveChannel(queue);
        }
    }
}
