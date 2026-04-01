using BePopJwt.WebUI.Dtos.HistoryDtos;
using BePopJwt.WebUI.Dtos.SongDtos;

namespace BePopJwt.WebUI.Services.PlayerServices
{
    public interface IApiPlayerService
    {
        Task<(bool IsSuccess, List<SongWithAlbumDto> Songs, string? Error)> GetAccessibleSongsAsync(string jwtToken);
        Task<(bool IsSuccess, List<UserHistoryDto> History, string? Error)> GetHistoryAsync(string jwtToken);
        Task<(bool IsSuccess, string? Error)> PlayAsync(string jwtToken, int songId, int playDuration);
        Task<(bool IsSuccess, List<SongWithAlbumDto> Songs, string? Error)> GetRecommendationsAsync(string jwtToken, int take = 6);
        Task<(bool IsSuccess, string? Source, string? Error)> GetSongSourceAsync(string jwtToken, int songId);
    }
}
