using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MTD.CouchBot.Domain.Dtos.Discord;
using MTD.CouchBot.Dals;
using MTD.CouchBot.Dals.Implementations;

namespace MTD.CouchBot.Managers.Implementations
{
    public class GuildManager : IGuildManager
    {
        IGuildDal _guildDal;

        public GuildManager(IGuildDal guildDal)
        {
            _guildDal = guildDal;
        }

        public async Task<Guild> AddNewGuild(Guild newGuild)
        {
            return await _guildDal.AddNewGuild(newGuild);
        }

        public async Task<List<Guild>> GetAllGuilds()
        {
            return await _guildDal.GetAllGuilds();
        }

        public async Task<Guild> GetGuildById(string id)
        {
            return await _guildDal.GetGuildById(id);
        }
    }
}
