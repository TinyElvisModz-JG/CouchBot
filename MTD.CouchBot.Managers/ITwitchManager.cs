using MTD.CouchBot.Domain.Models.Twitch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MTD.CouchBot.Managers
{
    public interface ITwitchManager
    {
        Task<TwitchStream> GetStreamById(string twitchId);
        Task<string> GetTwitchIdByLogin(string name);
        Task<TwitchStreams> GetStreamsByIdList(List<string> twitchIdList);
        Task<TwitchStreams> GetStreamsByIdList(string twitchIdList);
        Task<TwitchChannelFeed> GetChannelFeedPosts(string twitchId);
        Task<TwitchTeam> GetTwitchTeamByName(string name);
        Task<string> GetDelimitedListOfTwitchMemberIds(string teamToken);
        Task<List<Stream>> GetStreamsByGameName(string gameName);
        Task<TwitchGameSearchResponse> SearchForGameByName(string gameName);
    }
}
