using BePopJwt.WebUI.Dtos.ArtistDtos;
using BePopJwt.WebUI.Dtos.PackageDtos;
using BePopJwt.WebUI.Dtos.SongDtos;

namespace BePopJwt.WebUI.Services.CatalogServices
{
    public interface IApiCatalogService
    {
        Task<List<PackageDto>> GetPackagesAsync();
        Task<List<SongWithAlbumDto>> GetSongsWithAlbumAsync();
        Task<List<ArtistWithAlbumsDto>> GetArtistsWithAlbumsAsync();
    }
}
