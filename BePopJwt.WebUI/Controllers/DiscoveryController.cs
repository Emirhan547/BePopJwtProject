using BePopJwt.WebUI.Dtos.HomeDtos;
using BePopJwt.WebUI.Services.PlayerServices;
using BePopJwt.WebUI.Services.SongServices;
using BePopJwt.WebUI.Services.UserSessionServices;
using Microsoft.AspNetCore.Mvc;

namespace BePopJwt.WebUI.Controllers;

public class DiscoveryController(
    ISongService songService,
    IApiPlayerService playerService,
    IUserSessionService userSessionService) : Controller
{
    public async Task<IActionResult> Charts()
    {
        ViewBag.Session = userSessionService.GetCurrent();
        var songs = await songService.GetSongsWithAlbumAsync();
        return View("Charts", songs.OrderBy(s => s.Level).ThenBy(s => s.Name).ToList());
    }

    public async Task<IActionResult> Discover(string mood = "mutlu")
    {
        var session = userSessionService.GetCurrent();
        if (!session.IsAuthenticated || string.IsNullOrWhiteSpace(session.Token))
        {
            return View("Discover", new DiscoverViewModel
            {
                Session = session,
                ErrorMessage = "Bu sayfada paket yetkine uygun içerikleri görmek için giriş yapmalısın."
            });
        }

        var songsResult = await playerService.GetAccessibleSongsAsync(session.Token);
        var historyResult = await playerService.GetHistoryAsync(session.Token);
        var recommendationsResult = await playerService.GetMoodRecommendationsAsync(session.Token, mood, 8);

        return View("Discover", new DiscoverViewModel
        {
            Session = session,
            AccessibleSongs = songsResult.Songs,
            History = historyResult.History,
            RecommendedSongs = recommendationsResult.Songs,
            SelectedMood = mood,
            ErrorMessage = songsResult.Error ?? historyResult.Error ?? recommendationsResult.Error
        });
    }

    [HttpGet]
    public async Task<IActionResult> SongSource(int songId)
    {
        var session = userSessionService.GetCurrent();
        if (!session.IsAuthenticated || string.IsNullOrWhiteSpace(session.Token))
        {
            return Unauthorized(new { message = "Login required." });
        }

        var result = await playerService.GetSongSourceAsync(session.Token, songId);
        return result.IsSuccess ? Ok(new { ok = true, source = result.Source }) : BadRequest(new { ok = false, message = result.Error });
    }

    [HttpGet]
    public async Task<IActionResult> Recommendations(int take = 8)
    {
        var session = userSessionService.GetCurrent();
        if (!session.IsAuthenticated || string.IsNullOrWhiteSpace(session.Token))
        {
            return Unauthorized(new { ok = false, message = "Login required." });
        }

        var result = await playerService.GetRecommendationsAsync(session.Token, Math.Clamp(take, 1, 20));
        return result.IsSuccess
            ? Ok(new { ok = true, songs = result.Songs })
            : BadRequest(new { ok = false, message = result.Error });
    }

    [HttpGet]
    public async Task<IActionResult> PromptRecommendations(string prompt, int take = 8)
    {
        var session = userSessionService.GetCurrent();
        if (!session.IsAuthenticated || string.IsNullOrWhiteSpace(session.Token))
        {
            return Unauthorized(new { ok = false, message = "Login required." });
        }

        var result = await playerService.GetPromptRecommendationsAsync(session.Token, prompt, Math.Clamp(take, 1, 20));
        return result.IsSuccess
            ? Ok(new { ok = true, songs = result.Songs })
            : BadRequest(new { ok = false, message = result.Error });
    }

    public async Task<IActionResult> SongDetail(int id, bool autoPlay = false)
    {
        var songs = await songService.GetSongsWithAlbumAsync();
        var song = songs.FirstOrDefault(x => x.Id == id);
        if (song is null)
        {
            return RedirectToAction(nameof(Discover));
        }

        ViewBag.Session = userSessionService.GetCurrent();
        ViewBag.RelatedSongs = songs.Where(x => x.Id != id && x.Album?.Id == song.Album?.Id).Take(6).ToList();
        ViewBag.AutoPlay = autoPlay;
        return View("SongDetail", song);
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