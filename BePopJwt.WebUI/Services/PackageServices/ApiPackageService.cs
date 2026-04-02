using BePopJwt.WebUI.Dtos.Common;
using BePopJwt.WebUI.Dtos.PackageDtos;

namespace BePopJwt.WebUI.Services.PackageServices;

public class ApiPackageService(HttpClient client) : IApiPackageService
{
    public async Task<List<PackageDto>> GetPackagesAsync()
        => await GetDataOrDefaultAsync<List<PackageDto>>("api/packages") ?? [];

    private async Task<T?> GetDataOrDefaultAsync<T>(string endpoint)
    {
        var response = await client.GetAsync(endpoint);
        if (!response.IsSuccessStatusCode)
        {
            return default;
        }

        var result = await response.Content.ReadFromJsonAsync<BaseResultDto<T>>();
        return result.Data;
    }
}