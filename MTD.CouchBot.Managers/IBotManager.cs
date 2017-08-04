using MTD.CouchBot.Domain.Dtos.Bot;
using System.Threading.Tasks;

namespace MTD.CouchBot.Managers
{
    public interface IBotManager
    {
        Task<Statistics> GetStatistics();
        Task SaveStatistics(Statistics stats);
        Task<Configuration> GetConfiguration();
    }
}
