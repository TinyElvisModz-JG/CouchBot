using MTD.CouchBot.Domain.Dtos.Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTD.CouchBot.Managers
{
    public interface IGuildManager
    {
        Task<Guild> AddNewGuild(Guild newGuild);
        Task<List<Guild>> GetAllGuilds();
        Task<Guild> GetGuildById(string id);
    }
}
