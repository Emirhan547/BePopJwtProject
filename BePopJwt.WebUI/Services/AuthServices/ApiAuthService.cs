using BePopJwt.WebUI.Dtos.AuthDtos;
using BePopJwt.WebUI.Dtos.BaseDtos;

namespace BePopJwt.WebUI.Services.AuthServices
{
    public  class ApiAuthService(HttpClient client) : IApiAuthService
    {
        public async Task<(bool IsSuccess, AuthResponseDto? Response, string? Error)> LoginAsync(LoginRequestDto request)
        {
            var response = await client.PostAsJsonAsync("api/auths/login", request);
            return await MapAuthResponseAsync(response);
        }

        public async Task<(bool IsSuccess, AuthResponseDto? Response, string? Error)> RegisterAsync(RegisterRequestDto request)
        {
            var response = await client.PostAsJsonAsync("api/auths/register", request);
            return await MapAuthResponseAsync(response);
        }

        private static async Task<(bool IsSuccess, AuthResponseDto? Response, string? Error)> MapAuthResponseAsync(HttpResponseMessage response)
        {
            var result = await response.Content.ReadFromJsonAsync<BaseResultDto<AuthResponseDto>>();
            if (result is null)
            {
                return (false, null, "API response could not be parsed.");
            }

            return result.IsSuccess
                ? (true, result.Data, null)
                : (false, null, result.Errors?.FirstOrDefault()?.ErrorMessage ?? "Request failed.");
        }
    }
}
