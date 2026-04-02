using BePopJwt.WebUI.Dtos.ArtistDtos;
using BePopJwt.WebUI.Dtos.Common;

namespace BePopJwt.WebUI.Services.ArtistServices;

public class ApiArtistService(HttpClient client) : IArtistService
{
    public async Task<List<ArtistWithAlbumsDto>> GetArtistsWithAlbumsAsync()
        => await GetDataOrDefaultAsync<List<ArtistWithAlbumsDto>>("api/artists/withalbums") ?? [];

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