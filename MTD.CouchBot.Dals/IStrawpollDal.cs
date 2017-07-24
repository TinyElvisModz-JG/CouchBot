using MTD.CouchBot.Domain.Models.StrawPoll;
using System.Threading.Tasks;

namespace MTD.CouchBot.Dals
{
    public interface IStrawPollDal
    {
        Task<StrawPoll> CreateStrawPoll(StrawPollRequest poll);
    }
}
