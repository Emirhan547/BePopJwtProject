using BePopJwt.Business.Base;
using BePopJwt.Business.Dtos.AuthDtos;
using BePopJwt.Business.Dtos.UserDtos;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace BePopJwt.Business.Services.AuthServices
{
    public interface IAuthService
    {
        Task<BaseResult<AuthResponseDto>> RegisterAsync(RegisterDto dto);
        Task<BaseResult<AuthResponseDto>> LoginAsync(LoginDto dto);
        Task<BaseResult<AuthResponseDto>> ChangePackageAsync(int userId, int packageId);
        Task<BaseResult<UserProfileDto>> GetProfileAsync(int userId);
        Task<BaseResult<UserProfileDto>> UpdateProfileAsync(int userId, UpdateProfileDto dto);
    }
}
