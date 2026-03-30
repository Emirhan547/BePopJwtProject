using BePopJwt.DataAccess.Context;
using BePopJwt.DataAccess.Repositories.GenericRepositories;
using BePopJwt.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.DataAccess.Repositories.AlbumRepositories
{
    public class AlbumRepository : GenericRepository<Album>, IAlbumRepository
    {
        public AlbumRepository(AppDbContext _context) : base(_context)
        {
        }
        public async Task<List<Album>> GetAllWithArtistAsync()
        {
            return await _context.Albums
                .Include(x => x.Artist)
                .Include(x => x.Songs)
                .ToListAsync();
        }
    }
}
