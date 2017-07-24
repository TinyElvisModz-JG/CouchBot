using MTD.CouchBot.Domain.Models.StrawPoll;
using System.Threading.Tasks;

namespace MTD.CouchBot.Managers
{
    public interface IStrawPollManager
    {
        Task<StrawPoll> CreateStrawPoll(StrawPollRequest poll);
    }
}
