using BePopJwt.WebUI.Const;
using BePopJwt.WebUI.Dtos.AuthDtos;
using BePopJwt.WebUI.Services.UserSessionServices;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace BePopJwt.WebUI.Services;

public sealed class UserSessionService(IHttpContextAccessor accessor) : IUserSessionService
{
    private const string PackageNameCookie = "bepop.package.name";
    private const string PackageLevelCookie = "bepop.package.level";
    private const string UserNameCookie = "bepop.username";
    

    public UserSessionViewModel GetCurrent()
    {
        var http = accessor.HttpContext;
        if (http is null)
        {
            return new UserSessionViewModel();
        }

        var token = http.Request.Cookies[AuthCookieNames.JwtToken];
        if (string.IsNullOrWhiteSpace(token))
        {
            return new UserSessionViewModel();
        }

        var model = new UserSessionViewModel
        {
            IsAuthenticated = true,
            Token = token,
            UserName = http.Request.Cookies[UserNameCookie],
            PackageName = http.Request.Cookies[PackageNameCookie]
            
        };

        if (int.TryParse(http.Request.Cookies[PackageLevelCookie], out var level))
        {
            model.PackageLevel = level;
        }

        if (model.PackageLevel is null || string.IsNullOrWhiteSpace(model.PackageName) || string.IsNullOrWhiteSpace(model.UserName))
            
        {
            FillFromToken(model, token);
            
        }

        return model;
        
    }
    public void UpdateDisplayName(string displayName)
    {
        var http = accessor.HttpContext;
        if (http is null)
        {
            return;
        }

        var existingToken = http.Request.Cookies[AuthCookieNames.JwtToken];
        if (string.IsNullOrWhiteSpace(existingToken))
        {
            return;
        }

        var current = GetCurrent();
        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        };

        http.Response.Cookies.Append(UserNameCookie, displayName, options);
        if (!string.IsNullOrWhiteSpace(current.PackageName))
        {
            http.Response.Cookies.Append(PackageNameCookie, current.PackageName, options);
        }
        if (current.PackageLevel is not null)
        {
            http.Response.Cookies.Append(PackageLevelCookie, current.PackageLevel.Value.ToString(), options);
        }
    }
    public void SignIn(AuthResponseDto response)
    {
        var http = accessor.HttpContext;
        if (http is null)
        {
            return;
        }

        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = new DateTimeOffset(response.ExpiresAtUtc)
        };

        http.Response.Cookies.Append(AuthCookieNames.JwtToken, response.Token, options);
        http.Response.Cookies.Append(UserNameCookie, response.DisplayName, options);
        http.Response.Cookies.Append(PackageNameCookie, response.PackageName, options);
        http.Response.Cookies.Append(PackageLevelCookie, response.PackageLevel.ToString(), options);
    }

    public void SignOut()
    {
        var response = accessor.HttpContext?.Response;
        if (response is null)
        {
            return;
        }

        response.Cookies.Delete(AuthCookieNames.JwtToken);
        response.Cookies.Delete(UserNameCookie);
        response.Cookies.Delete(PackageNameCookie);
        response.Cookies.Delete(PackageLevelCookie);
    }

    private static void FillFromToken(UserSessionViewModel model, string token)
    {
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        model.UserName ??= jwt.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        model.PackageName ??= jwt.Claims.FirstOrDefault(x => x.Type == "packageName")?.Value;

        if (model.PackageLevel is null)
        {
            var val = jwt.Claims.FirstOrDefault(x => x.Type == "packageLevel")?.Value;
            if (int.TryParse(val, out var parsed))
            {
                model.PackageLevel = parsed;
            }
        }

       
    }
}
