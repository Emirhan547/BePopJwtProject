using BePopJwt.WebUI.Dtos.BaseDtos;
using BePopJwt.WebUI.Dtos.HistoryDtos;
using BePopJwt.WebUI.Dtos.SongDtos;
using System.Net.Http.Headers;

namespace BePopJwt.WebUI.Services.PlayerServices
{
    public sealed class ApiPlayerService(HttpClient client) : IApiPlayerService
    {
        public async Task<(bool IsSuccess, List<SongWithAlbumDto> Songs, string? Error)> GetAccessibleSongsAsync(string jwtToken)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "api/player/songs");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var response = await client.SendAsync(request);
            var result = await response.Content.ReadFromJsonAsync<BaseResultDto<List<SongWithAlbumDto>>>();

            if (result is null)
            {
                return (false, [], "Player response could not be parsed.");
            }

            return result.IsSuccess
                ? (true, result.Data ?? [], null)
                : (false, [], result.Errors?.FirstOrDefault()?.ErrorMessage ?? "Authorized song list could not be loaded.");
        }

        public async Task<(bool IsSuccess, List<UserHistoryDto> History, string? Error)> GetHistoryAsync(string jwtToken)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "api/player/history");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var response = await client.SendAsync(request);
            var result = await response.Content.ReadFromJsonAsync<BaseResultDto<List<UserHistoryDto>>>();

            if (result is null)
            {
                return (false, [], "History response could not be parsed.");
            }

            return result.IsSuccess
                ? (true, result.Data ?? [], null)
                : (false, [], result.Errors?.FirstOrDefault()?.ErrorMessage ?? "History could not be loaded.");
        }
    }
}
