using BePopJwt.DataAccess.Context;
using BePopJwt.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace BePopJwt.DataAccess.Repositories.UserRepositories
{
    public class AppUserRepository(AppDbContext context) : IAppUserRepository
    {
        public Task<AppUser?> GetByIdWithPackageAsync(int userId)
            => context.Users
                .Include(x => x.Package)
                .FirstOrDefaultAsync(x => x.Id == userId);

        public Task<AppUser?> GetByEmailWithPackageAsync(string email)
            => context.Users
                .Include(x => x.Package)
                .FirstOrDefaultAsync(x => x.Email == email);

        public Task<bool> ExistsByUserNameAsync(string userName, int excludedUserId)
            => context.Users
                .AsNoTracking()
                .AnyAsync(x => x.UserName == userName && x.Id != excludedUserId);

        public Task<bool> ExistsByEmailAsync(string email, int excludedUserId)
            => context.Users
                .AsNoTracking()
                .AnyAsync(x => x.Email == email && x.Id != excludedUserId);
    }
}