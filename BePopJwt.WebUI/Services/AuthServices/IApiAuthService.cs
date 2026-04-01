using BePopJwt.WebUI.Dtos.AuthDtos;

namespace BePopJwt.WebUI.Services.AuthServices
{
    public interface IApiAuthService
    {
        Task<(bool IsSuccess, AuthResponseDto? Response, string? Error)> LoginAsync(LoginRequestDto request);
        Task<(bool IsSuccess, AuthResponseDto? Response, string? Error)> RegisterAsync(RegisterRequestDto request);
       
    }
}
