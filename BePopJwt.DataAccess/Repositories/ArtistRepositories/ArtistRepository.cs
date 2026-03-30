using BePopJwt.DataAccess.Context;
using BePopJwt.DataAccess.Repositories.GenericRepositories;
using BePopJwt.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.DataAccess.Repositories.ArtistRepositories
{
    public class ArtistRepository : GenericRepository<Artist>,IArtistRepository
    {
        public ArtistRepository(AppDbContext _context) : base(_context)
        {
        }

        public async Task<List<Artist>> GetAllWithAlbumsAsync()
        {
            return await _context.Artists
         .Include(x => x.Albums)
             .ThenInclude(a => a.Songs)
         .ToListAsync();
        }
    }
}
