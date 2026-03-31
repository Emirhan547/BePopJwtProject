using BePopJwt.DataAccess.Context;
using BePopJwt.DataAccess.Repositories.GenericRepositories;
using BePopJwt.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.DataAccess.Repositories.SongRepositories
{
    public class SongRepository : GenericRepository<Song>, ISongRepository
    {
        public SongRepository(AppDbContext _context) : base(_context)
        {
        }
        public async Task<List<Song>> GetSongsWithAlbumAsync()
        {
            return await _context.Songs
                .AsNoTracking()
                .Include(x => x.Album)
                .ToListAsync();
        }

        public async Task<Song?> GetSongWithAlbumByIdAsync(int id)
        {
            return await _context.Songs
                .AsNoTracking()
                .Include(x => x.Album)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
