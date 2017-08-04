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

        public async Task<User> AddNewUser(User user)
        {
            return await _guildDal.AddNewUser(user);
        }

        public async Task<List<Guild>> GetAllGuilds()
        {
            return await _guildDal.GetAllGuilds();
        }

        public async Task<Guild> GetGuildById(string id)
        {
            return await _guildDal.GetGuildById(id);
        }

        public async Task<List<Guild>> GetGuildsForLive()
        {
            return await _guildDal.GetGuildsForLive();
        }

        public async Task<User> GetUserByUserId(string userId)
        {
            return await _guildDal.GetUserByUserId(userId);
        }

        public async Task RemoveGuild(Guild guild)
        {
            await _guildDal.RemoveGuild(guild);
        }

        public async Task UpdateGuild(Guild guild)
        {
            await _guildDal.UpdateGuild(guild);
        }
    }
}
