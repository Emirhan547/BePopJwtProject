using BePopJwt.WebUI.Services.PlayerServices;
using BePopJwt.WebUI.Services.UserSessionServices;
using Microsoft.AspNetCore.Mvc;

namespace BePopJwt.WebUI.Controllers;

public class HistoryController(IApiPlayerService playerService, IUserSessionService userSessionService) : Controller
{
    public async Task<IActionResult> History()
    {
        var session = userSessionService.GetCurrent();
        if (!session.IsAuthenticated || string.IsNullOrWhiteSpace(session.Token))
        {
            return RedirectToAction("SignIn", "Auth");
        }

        var result = await playerService.GetHistoryAsync(session.Token);
        ViewBag.Session = session;
        ViewBag.Error = result.Error;
        return View("History", result.History.OrderByDescending(x => x.PlayedAt).ToList());
    }
}