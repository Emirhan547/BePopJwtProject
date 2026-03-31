using BePopJwt.DataAccess.Repositories.GenericRepositories;
using BePopJwt.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.DataAccess.Repositories.UserSongRepositories
{
    public interface IUserSongHistoryRepository:IRepository<UserSongHistory>
    {
        Task<List<UserSongHistory>> GetHistoriesWithSongAndUserAsync();
        Task<UserSongHistory?> GetHistoryWithSongAndUserByIdAsync(int id);
    }
}
