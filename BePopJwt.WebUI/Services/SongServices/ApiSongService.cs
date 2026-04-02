using BePopJwt.WebUI.Dtos.Common;
using BePopJwt.WebUI.Dtos.SongDtos;

namespace BePopJwt.WebUI.Services.SongServices;

public class ApiSongService(HttpClient client) : ISongService
{
    public async Task<List<SongWithAlbumDto>> GetSongsWithAlbumAsync()
        => await GetDataOrDefaultAsync<List<SongWithAlbumDto>>("api/songs/withalbum") ?? [];

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