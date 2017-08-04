using MTD.CouchBot.Domain.Dtos.Live;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MTD.CouchBot.Managers
{
    public interface ILiveQueueManager
    {
        Task<List<Queue>> GetAllLiveChannels();
        Task<List<Queue>> GetLiveChannelsByPlatformId(Domain.Enumerations.Platform platform);
        Task UpdateLiveChannel(Queue queue);
    }
}
