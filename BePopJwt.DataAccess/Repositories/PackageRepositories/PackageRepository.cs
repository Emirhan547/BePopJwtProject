using BePopJwt.DataAccess.Context;
using BePopJwt.DataAccess.Repositories.GenericRepositories;
using BePopJwt.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.DataAccess.Repositories.PackageRepositories
{
    public class PackageRepository : GenericRepository<Package>, IPackageRepository
    {
        public PackageRepository(AppDbContext _context) : base(_context)
        {
        }
        public async Task<List<Package>> GetPackagesWithUsersAsync()
        {
            return await _context.Packages
                .AsNoTracking()
                .Include(x => x.Users)
                .ToListAsync();
        }

        public async Task<Package?> GetPackageWithUsersByIdAsync(int id)
        {
            return await _context.Packages
                .AsNoTracking()
                .Include(x => x.Users)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
