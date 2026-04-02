using BePopJwt.WebUI.Dtos.AccountDtos;
using BePopJwt.WebUI.Services.AccountServices;
using BePopJwt.WebUI.Services.PackageServices;
using BePopJwt.WebUI.Services.UserSessionServices;
using Microsoft.AspNetCore.Mvc;

namespace BePopJwt.WebUI.Controllers;

public class PackageController(
    IApiPackageService packageService,
    IApiAccountService accountService,
    IUserSessionService userSessionService) : Controller
{
    public async Task<IActionResult> Packages(string? success = null)
    {
        var session = userSessionService.GetCurrent();
        if (!session.IsAuthenticated || string.IsNullOrWhiteSpace(session.Token))
        {
            return RedirectToAction("SignIn", "Auth");
        }

        return View("Packages", new PackageManagementViewModel
        {
            Session = session,
            Packages = await packageService.GetPackagesAsync(),
            SuccessMessage = success
        });
    }

    [HttpPost]
    public async Task<IActionResult> ChangePackage(int packageId)
    {
        var session = userSessionService.GetCurrent();
        if (!session.IsAuthenticated || string.IsNullOrWhiteSpace(session.Token))
        {
            return RedirectToAction("SignIn", "Auth");
        }

        var result = await accountService.ChangePackageAsync(session.Token, packageId);
        if (!result.IsSuccess || result.Response is null)
        {
            return View("Packages", new PackageManagementViewModel
            {
                Session = session,
                Packages = await packageService.GetPackagesAsync(),
                ErrorMessage = result.Error ?? "Paket geçişi sırasında bir hata oluştu."
            });
        }

        userSessionService.SignIn(result.Response);
        return RedirectToAction(nameof(Packages), new { success = "Paketiniz başarıyla güncellendi." });
    }
}