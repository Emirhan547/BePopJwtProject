using BePopJwt.WebUI.Dtos.AuthDtos;
using BePopJwt.WebUI.Dtos.BaseDtos;
using BePopJwt.WebUI.Dtos.UserDtos;
using System.Net.Http.Headers;

namespace BePopJwt.WebUI.Services.AccountServices
{
    public class ApiAccountService(HttpClient client) : IApiAccountService
    {
        public async Task<(bool IsSuccess, UserProfileDto? Profile, string? Error)> GetProfileAsync(string jwtToken)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "api/auths/profile");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await client.SendAsync(request);
            var result = await response.Content.ReadFromJsonAsync<BaseResultDto<UserProfileDto>>();

            if (result is null)
            {
                return (false, null, "Profile response could not be parsed.");
            }

            return result.IsSuccess
                ? (true, result.Data, null)
                : (false, null, result.Errors?.FirstOrDefault()?.ErrorMessage ?? "Profile could not be loaded.");
        }

        public async Task<(bool IsSuccess, AuthResponseDto? Response, string? Error)> ChangePackageAsync(string jwtToken, int packageId)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "api/auths/change-package");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            request.Content = JsonContent.Create(new ChangePackageRequestDto { PackageId = packageId });

            var response = await client.SendAsync(request);
            var result = await response.Content.ReadFromJsonAsync<BaseResultDto<AuthResponseDto>>();
            if (result is null)
            {
                return (false, null, "Package change response could not be parsed.");
            }

            return result.IsSuccess
                ? (true, result.Data, null)
                : (false, null, result.Errors?.FirstOrDefault()?.ErrorMessage ?? "Package change failed.");
        }
    }
}