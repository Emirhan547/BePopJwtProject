using BePopJwt.WebUI.Dtos.BaseDtos;
using BePopJwt.WebUI.Dtos.HistoryDtos;
using BePopJwt.WebUI.Dtos.SongDtos;
using System.Net.Http.Headers;
using static Microsoft.CodeAnalysis.CSharp.SyntaxTokenParser;

namespace BePopJwt.WebUI.Services.PlayerServices
{
    public class ApiPlayerService(HttpClient client) : IApiPlayerService
    {
        public async Task<(bool IsSuccess, List<SongWithAlbumDto> Songs, string? Error)> GetAccessibleSongsAsync(string jwtToken)
        {
            var response = await SendWithAuthAsync(jwtToken, HttpMethod.Get, "api/player/songs");
             var result = await response.Content.ReadFromJsonAsync<BaseResultDto<List<SongWithAlbumDto>>>();

            if (result is null)
            {
                return (false, [], "Player response could not be parsed.");
            }

            return result.IsSuccess
                ? (true, result.Data ?? [], null)
                : (false, [], result.Errors?.FirstOrDefault()?.ErrorMessage ?? "Authorized song list could not be loaded.");
        }
        public async Task<(bool IsSuccess, List<SongWithAlbumDto> Songs, string? Error)> GetRecommendationsAsync(string jwtToken, int take = 6)
        {
            var response = await SendWithAuthAsync(jwtToken, HttpMethod.Get, $"api/player/recommendations?take={Math.Clamp(take, 1, 20)}");
            var result = await response.Content.ReadFromJsonAsync<BaseResultDto<List<SongWithAlbumDto>>>();

            if (result is null)
            {
                return (false, [], "Recommendations response could not be parsed.");
            }

            return result.IsSuccess
                ? (true, result.Data ?? [], null)
                : (false, [], result.Errors?.FirstOrDefault()?.ErrorMessage ?? "Recommendations could not be loaded.");
        }
        public async Task<(bool IsSuccess, string? Source, string? Error)> GetSongSourceAsync(string jwtToken, int songId)
        {
            var response = await SendWithAuthAsync(jwtToken, HttpMethod.Get, $"api/player/song-source/{songId}");
            var result = await response.Content.ReadFromJsonAsync<BaseResultDto<string>>();

            if (result is null)
            {
                return (false, null, "Song source response could not be parsed.");
            }

            return result.IsSuccess
                ? (true, result.Data, null)
                : (false, null, result.Errors?.FirstOrDefault()?.ErrorMessage ?? "Şarkı kaynağına erişilemedi.");
        }
        public async Task<(bool IsSuccess, List<SongWithAlbumDto> Songs, string? Error)> GetMoodRecommendationsAsync(string jwtToken, string mood, int take = 6)
        {
            var encodedMood = Uri.EscapeDataString(mood ?? string.Empty);
            var response = await SendWithAuthAsync(jwtToken, HttpMethod.Get, $"api/player/recommendations-by-mood?mood={encodedMood}&take={Math.Clamp(take, 1, 20)}");
            var result = await response.Content.ReadFromJsonAsync<BaseResultDto<List<SongWithAlbumDto>>>();

            if (result is null)
            {
                return (false, [], "Mood recommendations response could not be parsed.");
            }

            return result.IsSuccess
                ? (true, result.Data ?? [], null)
                : (false, [], result.Errors?.FirstOrDefault()?.ErrorMessage ?? "Ruh haline göre öneriler yüklenemedi.");
        }
        public async Task<(bool IsSuccess, List<SongWithAlbumDto> Songs, string? Error)> GetPromptRecommendationsAsync(string jwtToken, string prompt, int take = 8)
        {
            var encodedPrompt = Uri.EscapeDataString(prompt ?? string.Empty);
            var response = await SendWithAuthAsync(jwtToken, HttpMethod.Get, $"api/player/recommendations-by-prompt?prompt={encodedPrompt}&take={Math.Clamp(take, 1, 20)}");
            var result = await response.Content.ReadFromJsonAsync<BaseResultDto<List<SongWithAlbumDto>>>();

            if (result is null)
            {
                return (false, [], "Prompt recommendations response could not be parsed.");
            }

            return result.IsSuccess
                ? (true, result.Data ?? [], null)
                : (false, [], result.Errors?.FirstOrDefault()?.ErrorMessage ?? "Prompta göre öneriler yüklenemedi.");
        }
        public async Task<(bool IsSuccess, List<UserHistoryDto> History, string? Error)> GetHistoryAsync(string jwtToken)
        {
            var response = await SendWithAuthAsync(jwtToken, HttpMethod.Get, "api/player/history");
            var result = await response.Content.ReadFromJsonAsync<BaseResultDto<List<UserHistoryDto>>>();

            if (result is null)
            {
                return (false, [], "History response could not be parsed.");
            }

            return result.IsSuccess
                ? (true, result.Data ?? [], null)
                : (false, [], result.Errors?.FirstOrDefault()?.ErrorMessage ?? "History could not be loaded.");
        }

        public async Task<(bool IsSuccess, string? Error)> PlayAsync(string jwtToken, int songId, int playDuration)
        {
            var response = await SendWithAuthAsync(jwtToken, HttpMethod.Post, "api/player/play", new
            {
                SongId = songId,
                PlayDuration = playDuration
            });
        

            var result = await response.Content.ReadFromJsonAsync<BaseResultDto<object>>();
            if (result is null)
            {
                return (false, "Play response could not be parsed.");
            }

            return result.IsSuccess
                ? (true, null)
                : (false, result.Errors?.FirstOrDefault()?.ErrorMessage ?? "Şarkı dinleme kaydı atılamadı.");
        }

private async Task<HttpResponseMessage> SendWithAuthAsync(string jwtToken, HttpMethod method, string url, object? payload = null)
{
    using var request = new HttpRequestMessage(method, url);
    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
    
    if (payload is not null)
    {
        request.Content = JsonContent.Create(payload);
    }

    return await client.SendAsync(request);
}
    }
}