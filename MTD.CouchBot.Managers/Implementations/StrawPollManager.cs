using MTD.CouchBot.Dals;
using MTD.CouchBot.Dals.Implementations;
using MTD.CouchBot.Domain.Models.StrawPoll;
using System.Threading.Tasks;

namespace MTD.CouchBot.Managers.Implementations
{
    public class StrawPollManager : IStrawPollManager
    {
        private readonly IStrawPollDal _strawPollDal;

        public StrawPollManager(IStrawPollDal strawPollDal)
        {
            _strawPollDal = strawPollDal;
        }

        public async Task<StrawPoll> CreateStrawPoll(StrawPollRequest poll)
        {
            return await _strawPollDal.CreateStrawPoll(poll);
        }
    }
}
