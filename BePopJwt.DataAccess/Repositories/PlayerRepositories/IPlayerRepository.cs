using BePopJwt.DataAccess.Repositories.GenericRepositories;
using BePopJwt.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.DataAccess.Repositories.PlayerRepositories
{
    public interface IPlayerRepository : IRepository<Player>
    {
        Task<Player?> GetByUserIdAsync(int userId);
    }
}
