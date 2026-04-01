using BePopJwt.WebUI.Const;
using BePopJwt.WebUI.Dtos.AuthDtos;
using BePopJwt.WebUI.Services;
using BePopJwt.WebUI.Services.AuthServices;
using BePopJwt.WebUI.Services.CatalogServices;
using Microsoft.AspNetCore.Mvc;

namespace BePopJwt.WebUI.Controllers;

public class AuthController(IApiAuthService authService, IApiCatalogService catalogService) : Controller
{
    [HttpGet]
    public IActionResult SignIn() => View(new LoginDto());

    [HttpPost]
    public async Task<IActionResult> SignIn(LoginDto vm)
    {
        var result = await authService.LoginAsync(vm.Request);
        if (!result.IsSuccess || result.Response is null)
        {
            vm.ErrorMessage = result.Error;
            return View(vm);
        }

        SetTokenCookie(result.Response.Token, result.Response.ExpiresAtUtc);
        return RedirectToAction("Discover", "Default");
    }

    [HttpGet]
    public async Task<IActionResult> SignUp()
    {
        var vm = new RegisterDto { Packages = await catalogService.GetPackagesAsync() };
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(RegisterDto vm)
    {
        vm.Packages = await catalogService.GetPackagesAsync();
        var result = await authService.RegisterAsync(vm.Request);
        if (!result.IsSuccess || result.Response is null)
        {
            vm.ErrorMessage = result.Error;
            return View(vm);
        }

        SetTokenCookie(result.Response.Token, result.Response.ExpiresAtUtc);
        return RedirectToAction("Discover", "Default");
    }

    public IActionResult Logout()
    {
        Response.Cookies.Delete(AuthCookieNames.JwtToken);
        return RedirectToAction("Index", "Default");
    }

    private void SetTokenCookie(string token, DateTime expiresAtUtc)
    {
        Response.Cookies.Append(AuthCookieNames.JwtToken, token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = new DateTimeOffset(expiresAtUtc)
        });
    }
}