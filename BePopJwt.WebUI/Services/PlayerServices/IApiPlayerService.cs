using BePopJwt.WebUI.Dtos.HistoryDtos;
using BePopJwt.WebUI.Dtos.SongDtos;

namespace BePopJwt.WebUI.Services.PlayerServices
{
    public interface IApiPlayerService
    {
        Task<(bool IsSuccess, List<SongWithAlbumDto> Songs, string? Error)> GetAccessibleSongsAsync(string jwtToken);
        Task<(bool IsSuccess, List<UserHistoryDto> History, string? Error)> GetHistoryAsync(string jwtToken);
        Task<(bool IsSuccess, string? Error)> PlayAsync(string jwtToken, int songId, int playDuration);
    }
}
