using BePopJwt.WebUI.Dtos.AuthDtos;
using BePopJwt.WebUI.Dtos.UserDtos;

namespace BePopJwt.WebUI.Services.AccountServices
{
    public interface IApiAccountService
    {
        Task<(bool IsSuccess, UserProfileDto? Profile, string? Error)> GetProfileAsync(string jwtToken);
        Task<(bool IsSuccess, AuthResponseDto? Response, string? Error)> ChangePackageAsync(string jwtToken, int packageId);
        Task<(bool IsSuccess, UserProfileDto? Profile, string? Error)> UpdateProfileAsync(string jwtToken, UpdateProfileRequestDto request);
    }
}
