using BePopJwt.WebUI.Dtos.AuthDtos;
using BePopJwt.WebUI.Services;
using BePopJwt.WebUI.Services.AuthServices;

using BePopJwt.WebUI.Services.PackageServices;
using BePopJwt.WebUI.Services.UserSessionServices;
using Microsoft.AspNetCore.Mvc;

namespace BePopJwt.WebUI.Controllers;

public class AuthController(IApiAuthService authService, IApiPackageService packageService, IUserSessionService userSessionService) : Controller
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
        return RedirectToAction("Discover", "Discovery");
    }

    [HttpGet]
    public async Task<IActionResult> SignUp()
    {
        var vm = new RegisterDto { Packages = await packageService.GetPackagesAsync() }; 
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(RegisterDto vm)
    {
        vm.Packages = await packageService.GetPackagesAsync();
        var result = await authService.RegisterAsync(vm.Request);
        if (!result.IsSuccess || result.Response is null)
        {
            vm.ErrorMessage = result.Error;
            return View(vm);
        }

        userSessionService.SignIn(result.Response);
        return RedirectToAction("Discover", "Discovery");
    }

    public IActionResult Logout()
    {
        userSessionService.SignOut();
        return RedirectToAction("Index", "Default");
    }
}