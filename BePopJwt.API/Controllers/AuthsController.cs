using BePopJwt.Business.Dtos.AuthDtos;
using BePopJwt.Business.Dtos.UserDtos;
using BePopJwt.Business.Services.AuthServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BePopJwt.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController(IAuthService _authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
           
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            var userId = GetUserId();
            if (userId is null)
            {
                return Unauthorized();
            }

            var result = await _authService.GetProfileAsync(userId.Value);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto)
        {
            var userId = GetUserId();
            if (userId is null)
            {
                return Unauthorized();
            }

            var result = await _authService.UpdateProfileAsync(userId.Value, dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [Authorize]
        [HttpPost("change-package")]
        public async Task<IActionResult> ChangePackage(ChangePackageDto dto)
        {
            var userId = GetUserId();
            if (userId is null)
            {
                return Unauthorized();
            }

            var result = await _authService.ChangePackageAsync(userId.Value, dto.PackageId);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        private int? GetUserId()
        {
            var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(value, out var userId) ? userId : null;
        }
    }
}