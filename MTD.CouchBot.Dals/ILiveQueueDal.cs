using MTD.CouchBot.Domain.Dtos.Live;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTD.CouchBot.Dals
{
    public interface ILiveQueueDal
    {
        Task<List<Queue>> GetAllLiveChannels();
        Task<List<Queue>> GetLiveChannelsByPlatformId(Domain.Enumerations.Platform platform);
        Task UpdateLiveChannel(Queue queue);
    }
}
