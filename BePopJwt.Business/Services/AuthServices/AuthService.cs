using BePopJwt.Business.Base;
using BePopJwt.Business.Dtos.AuthDtos;
using BePopJwt.DataAccess.Context;
using BePopJwt.Entity.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BePopJwt.Business.Services.AuthServices
{
    public class AuthService(
        UserManager<AppUser> userManager,
        IValidator<RegisterDto> registerValidator,
        IValidator<LoginDto> loginValidator,
        IConfiguration configuration,
        AppDbContext context) : IAuthService
    {
        public async Task<BaseResult<AuthResponseDto>> RegisterAsync(RegisterDto dto)
        {
            var validation = await registerValidator.ValidateAsync(dto);
            if (!validation.IsValid)
            {
                return BaseResult<AuthResponseDto>.Fail(validation.Errors);
            }

            var package = await context.Packages
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == dto.PackageId);

            if (package is null)
            {
                return BaseResult<AuthResponseDto>.Fail("Geçersiz paket seçimi.");
            }

            var user = new AppUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                DisplayName = dto.DisplayName,
                ImageUrl=dto.ImageUrl,
                PackageId = dto.PackageId
            };

            var createResult = await userManager.CreateAsync(user, dto.Password);
            if (!createResult.Succeeded)
            {
                return BaseResult<AuthResponseDto>.Fail(createResult.Errors);
            }

            var response = GenerateToken(user, package.Name, package.Level);
            return BaseResult<AuthResponseDto>.Success(response);
        }

        public async Task<BaseResult<AuthResponseDto>> LoginAsync(LoginDto dto)
        {
            var validation = await loginValidator.ValidateAsync(dto);
            if (!validation.IsValid)
            {
                return BaseResult<AuthResponseDto>.Fail(validation.Errors);
            }

            var user = await userManager.Users
                .Include(x => x.Package)
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user is null)
            {
                return BaseResult<AuthResponseDto>.Fail("Kullanıcı bulunamadı.");
            }

            var isValidPassword = await userManager.CheckPasswordAsync(user, dto.Password);
            if (!isValidPassword)
            {
                return BaseResult<AuthResponseDto>.Fail("Email veya şifre hatalı.");
            }

            var response = GenerateToken(user, user.Package.Name, user.Package.Level);
            return BaseResult<AuthResponseDto>.Success(response);
        }

        private AuthResponseDto GenerateToken(AppUser user, string packageName, int packageLevel)
        {
            var jwtSection = configuration.GetSection("Jwt");
            var key = jwtSection["Key"] ?? throw new InvalidOperationException("Jwt:Key missing.");
            var issuer = jwtSection["Issuer"] ?? "BePopJwt";
            var audience = jwtSection["Audience"] ?? "BePopJwtClients";
            var expiryMinutes = int.TryParse(jwtSection["ExpiryMinutes"], out var value) ? value : 120;

            var expiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes);
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName ?? string.Empty),
                new(ClaimTypes.Email, user.Email ?? string.Empty),
                new("packageId", user.PackageId.ToString()),
                new("packageLevel", packageLevel.ToString()),
                new("packageName", packageName)
            };

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: expiresAt,
                signingCredentials: credentials);

            return new AuthResponseDto
            {
                UserId = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                DisplayName = user.DisplayName,
                PackageName = packageName,
                PackageLevel = packageLevel,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAtUtc = expiresAt
            };
        }
    }
}