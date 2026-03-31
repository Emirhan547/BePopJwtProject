using BePopJwt.Business.Base;
using BePopJwt.Business.Dtos.PackageDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Services.PackageServices
{
    public interface IPackageService
    {
        Task<BaseResult<List<ResultPackageDto>>> GetAllAsync();
        Task<BaseResult<List<ResultPackageWithUsersDto>>> GetPackagesWithUsersAsync();
        Task<BaseResult<ResultPackageDto>> GetByIdAsync(int id);
        Task<BaseResult<ResultPackageWithUsersDto>> GetPackageWithUsersByIdAsync(int id);
        Task<BaseResult<ResultPackageDto>> CreateAsync(CreatePackageDto createPackageDto);
        Task<BaseResult<ResultPackageDto>> UpdateAsync(UpdatePackageDto updatePackageDto);
        Task<BaseResult<bool>>DeleteAsync(int id);
    }
}
