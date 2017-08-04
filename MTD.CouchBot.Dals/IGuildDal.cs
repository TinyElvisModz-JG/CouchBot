using MTD.CouchBot.Domain.Dtos.Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTD.CouchBot.Dals
{
    public interface IGuildDal
    {
        Task<Guild> AddNewGuild(Guild newGuild);
        Task<List<Guild>> GetAllGuilds();
        Task<Guild> GetGuildById(string id);
        Task RemoveGuild(Guild guild);
        Task UpdateGuild(Guild guild);
        Task<List<Guild>> GetGuildsForLive();
        Task<User> GetUserByUserId(string userId);
        Task<User> AddNewUser(User user);
    }
}
