using BePopJwt.WebUI.Dtos.HistoryDtos;
using BePopJwt.WebUI.Dtos.HomeDtos;
using BePopJwt.WebUI.Dtos.SongDtos;
using BePopJwt.WebUI.Services;
using BePopJwt.WebUI.Services.CatalogServices;
using BePopJwt.WebUI.Services.PlayerServices;
using BePopJwt.WebUI.Services.UserSessionServices;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxTokenParser;

namespace BePopJwt.WebUI.Controllers;

public class DefaultController(IApiCatalogService catalogService, IApiPlayerService playerService, IUserSessionService userSessionService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var vm = new HomeViewModel
        {
            Songs = await catalogService.GetSongsWithAlbumAsync(),
            Artists = await catalogService.GetArtistsWithAlbumsAsync(),
            Packages = await catalogService.GetPackagesAsync(),
            Session = userSessionService.GetCurrent()
        };

        return View(vm);
    }

    public async Task<IActionResult> Artists()
    {
        var artists = await catalogService.GetArtistsWithAlbumsAsync();
        ViewBag.Session = userSessionService.GetCurrent();
        return View(artists);
    }

    public async Task<IActionResult> Charts()
    {
        var session = userSessionService.GetCurrent();
        var songs = await catalogService.GetSongsWithAlbumAsync();
        ViewBag.Session = session;
        return View(songs.OrderBy(s => s.Level).ThenBy(s => s.Name).ToList());
    }

    public async Task<IActionResult> Discover()
    {
        var session = userSessionService.GetCurrent();
        if (!session.IsAuthenticated || string.IsNullOrWhiteSpace(session.Token))
        {
            return View(new DiscoverViewModel
            {
                Session = session,
                ErrorMessage = "Bu sayfada paket yetkine uygun içerikleri görmek için giriş yapmalısın."
            });
        }

        var songsResult = await playerService.GetAccessibleSongsAsync(session.Token);
        var historyResult = await playerService.GetHistoryAsync(session.Token);

        var recommendationsResult = await playerService.GetRecommendationsAsync(session.Token, 8);
        return View(new DiscoverViewModel
        {
            Session = session,
            AccessibleSongs = songsResult.Songs,
            History = historyResult.History,
            RecommendedSongs = recommendationsResult.Songs,
            ErrorMessage = songsResult.Error ?? historyResult.Error ?? recommendationsResult.Error
        });
    }

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
        return View(result.History.OrderByDescending(x => x.PlayedAt).ToList());
    }

    [HttpPost]
    public async Task<IActionResult> Play([FromBody] PlaySongWebRequest request)
    {
        var session = userSessionService.GetCurrent();
        if (!session.IsAuthenticated || string.IsNullOrWhiteSpace(session.Token))
        {
            return Unauthorized(new { message = "Login required." });
        }

        var result = await playerService.PlayAsync(session.Token, request.SongId, request.PlayDuration);
        return result.IsSuccess ? Ok(new { ok = true }) : BadRequest(new { ok = false, message = result.Error });
    }

   
}
