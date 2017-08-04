using MTD.CouchBot.Domain.Dtos.Bot;
using System.Threading.Tasks;

namespace MTD.CouchBot.Dals
{
    public interface IBotDal
    {
        Task<Statistics> GetStatistics();
        Task SaveStatistics(Statistics stats);
        Task<Configuration> GetConfiguration();
    }
}
