using BePopJwt.Entity.Entities;

namespace BePopJwt.DataAccess.Repositories.UserRepositories
{
    public interface IAppUserRepository
    {
        Task<AppUser?> GetByIdWithPackageAsync(int userId);
        Task<AppUser?> GetByEmailWithPackageAsync(string email);
        Task<bool> ExistsByUserNameAsync(string userName, int excludedUserId);
        Task<bool> ExistsByEmailAsync(string email, int excludedUserId);
    }
}