using BePopJwt.WebUI.Dtos.ArtistDtos;
using BePopJwt.WebUI.Dtos.BaseDtos;
using BePopJwt.WebUI.Dtos.PackageDtos;
using BePopJwt.WebUI.Dtos.SongDtos;

namespace BePopJwt.WebUI.Services.CatalogServices
{
    public sealed class ApiCatalogService(HttpClient client) : IApiCatalogService
    {
        public async Task<List<PackageDto>> GetPackagesAsync()
            => await GetDataOrDefaultAsync<List<PackageDto>>("api/packages") ?? [];

        public async Task<List<SongWithAlbumDto>> GetSongsWithAlbumAsync()
            => await GetDataOrDefaultAsync<List<SongWithAlbumDto>>("api/songs/withalbum") ?? [];

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
}
