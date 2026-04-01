using BePopJwt.WebUI.Dtos.AuthDtos;
using BePopJwt.WebUI.Services;
using BePopJwt.WebUI.Services.AuthServices;
using BePopJwt.WebUI.Services.CatalogServices;
using BePopJwt.WebUI.Services.UserSessionServices;
using Microsoft.AspNetCore.Mvc;

namespace BePopJwt.WebUI.Controllers;

public class AuthController(IApiAuthService authService, IApiCatalogService catalogService, IUserSessionService userSessionService) : Controller
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

        userSessionService.SignIn(result.Response);
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

        userSessionService.SignIn(result.Response);
        return RedirectToAction("Discover", "Default");
    }

    public IActionResult Logout()
    {
        userSessionService.SignOut();
        return RedirectToAction("Index", "Default");
    }
}