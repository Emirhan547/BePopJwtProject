using BePopJwt.WebUI.Dtos.UserDtos;
using BePopJwt.WebUI.Services.AccountServices;
using BePopJwt.WebUI.Services.UserSessionServices;
using Microsoft.AspNetCore.Mvc;

namespace BePopJwt.WebUI.Controllers;

public class ProfileController(IApiAccountService accountService, IUserSessionService userSessionService) : Controller
{
    public async Task<IActionResult> Profile()
    {
        var session = userSessionService.GetCurrent();
        if (!session.IsAuthenticated || string.IsNullOrWhiteSpace(session.Token))
        {
            return RedirectToAction("SignIn", "Auth");
        }

        var result = await accountService.GetProfileAsync(session.Token);
        if (!result.IsSuccess || result.Profile is null)
        {
            ViewBag.Error = result.Error ?? "Profil bilgileri yüklenemedi.";
            return View("~/Views/Default/Profile.cshtml", new UserProfileDto());
        }

        return View("~/Views/Default/Profile.cshtml", result.Profile);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(UpdateProfileRequestDto request)
    {
        var session = userSessionService.GetCurrent();
        if (!session.IsAuthenticated || string.IsNullOrWhiteSpace(session.Token))
        {
            return RedirectToAction("SignIn", "Auth");
        }

        var result = await accountService.UpdateProfileAsync(session.Token, request);
        if (!result.IsSuccess || result.Profile is null)
        {
            ViewBag.Error = result.Error ?? "Profil güncellenemedi.";
            return View("~/Views/Default/Profile.cshtml", new UserProfileDto
            {
                UserName = request.UserName,
                Email = request.Email,
                DisplayName = request.DisplayName,
                ImageUrl = request.ImageUrl
            });
        }

        userSessionService.UpdateDisplayName(result.Profile.DisplayName);
        ViewBag.Success = "Profilin başarıyla güncellendi.";
        return View("~/Views/Default/Profile.cshtml", result.Profile);
    }
}