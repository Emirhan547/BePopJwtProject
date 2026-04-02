using BePopJwt.WebUI.Dtos.SongDtos;

namespace BePopJwt.WebUI.Services.SongServices;

public interface ISongService
{
    Task<List<SongWithAlbumDto>> GetSongsWithAlbumAsync();
}