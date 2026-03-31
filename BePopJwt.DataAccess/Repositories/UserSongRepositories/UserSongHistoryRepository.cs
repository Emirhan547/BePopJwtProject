using BePopJwt.DataAccess.Context;
using BePopJwt.DataAccess.Repositories.GenericRepositories;
using BePopJwt.Entity.Entities;
using Microsoft.EntityFrameworkCore;
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
        public async Task<List<UserSongHistory>> GetHistoriesWithSongAndUserAsync()
        {
            return await _context.UserSongHistories
                .AsNoTracking()
                .Include(x => x.Song)
                .Include(x => x.User)
                .ToListAsync();
        }

        public async Task<UserSongHistory?> GetHistoryWithSongAndUserByIdAsync(int id)
        {
            return await _context.UserSongHistories
                .AsNoTracking()
                .Include(x => x.Song)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}