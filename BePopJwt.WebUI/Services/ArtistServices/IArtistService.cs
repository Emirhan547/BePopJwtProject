using BePopJwt.WebUI.Dtos.ArtistDtos;

namespace BePopJwt.WebUI.Services.ArtistServices
{
    public interface IArtistService
    {
        Task<List<ArtistWithAlbumsDto>> GetArtistsWithAlbumsAsync();
    }
}
