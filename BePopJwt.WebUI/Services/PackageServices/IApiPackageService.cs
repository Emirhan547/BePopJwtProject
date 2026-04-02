using BePopJwt.WebUI.Dtos.PackageDtos;

namespace BePopJwt.WebUI.Services.PackageServices
{
    public interface IApiPackageService
    {
        Task<List<PackageDto>> GetPackagesAsync();
    }
}
