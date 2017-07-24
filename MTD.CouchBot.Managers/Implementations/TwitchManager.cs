using MTD.CouchBot.Dals;
using MTD.CouchBot.Dals.Implementations;
using MTD.CouchBot.Domain.Models.Twitch;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTD.CouchBot.Managers.Implementations
{
    public class TwitchManager : ITwitchManager
    {
        private readonly ITwitchDal _twitchDal;

        public TwitchManager(TwitchDal twitchDal)
        {
            _twitchDal = twitchDal;
        }

        public async Task<TwitchStream> GetStreamById(string twitchId)
        {
            return await _twitchDal.GetStreamById(twitchId);
        }

        public async Task<string> GetTwitchIdByLogin(string name)
        {
            return await _twitchDal.GetTwitchIdByLogin(name);
        }

        public async Task<TwitchStreams> GetStreamsByIdList(List<string> twitchIdList)
        {
            var list = new StringBuilder();

            foreach (var id in twitchIdList)
            {
                list.Append(id + ",");
            }

            return await _twitchDal.GetStreamsByIdList(list.ToString().TrimEnd(','));
        }

        public async Task<TwitchStreams> GetStreamsByIdList(string twitchIdList)
        {
            return await _twitchDal.GetStreamsByIdList(twitchIdList);
        }

        public async Task<TwitchChannelFeed> GetChannelFeedPosts(string twitchId)
        {
            return await _twitchDal.GetChannelFeedPosts(twitchId);
        }
        
        public async Task<TwitchTeam> GetTwitchTeamByName(string name)
        {
            return await _twitchDal.GetTwitchTeamByName(name);
        }

        public async Task<string> GetDelimitedListOfTwitchMemberIds(string teamToken)
        {
            return await _twitchDal.GetDelimitedListOfTwitchMemberIds(teamToken);
        }

        public async Task<List<Stream>> GetStreamsByGameName(string gameName)
        {
            return await _twitchDal.GetStreamsByGameName(gameName);
        }

        public async Task<TwitchGameSearchResponse> SearchForGameByName(string gameName)
        {
            return await _twitchDal.SearchForGameByName(gameName);
        }
    }
}
