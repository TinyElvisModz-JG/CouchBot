using MTD.CouchBot.Domain.Models.ApiAi;
using System.Threading.Tasks;

namespace MTD.CouchBot.Dals
{
    public interface IApiAiDal
    {
        Task<ApiAiResponse> AskAQuestion(string question);
    }
}
