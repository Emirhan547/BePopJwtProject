using BePopJwt.DataAccess.Context;
using BePopJwt.DataAccess.Repositories.GenericRepositories;
using BePopJwt.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.DataAccess.Repositories.UserSongRepositories
{
    public class UserSongHistoryRepository : GenericRepository<UserSongHistory>, IUserSongHistoryRepository
    {
        public UserSongHistoryRepository(AppDbContext _context) : base(_context)
        {
        }
    }
}