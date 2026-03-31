using BePopJwt.DataAccess.Repositories.GenericRepositories;
using BePopJwt.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.DataAccess.Repositories.PackageRepositories
{
    public interface IPackageRepository:IRepository<Package>
    {
        Task<List<Package>> GetPackagesWithUsersAsync();
        Task<Package?> GetPackageWithUsersByIdAsync(int id);
    }
}
