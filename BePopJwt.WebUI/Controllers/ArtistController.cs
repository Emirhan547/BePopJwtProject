using BePopJwt.WebUI.Services.ArtistServices;
using BePopJwt.WebUI.Services.SongServices;
using BePopJwt.WebUI.Services.UserSessionServices;
using Microsoft.AspNetCore.Mvc;

namespace BePopJwt.WebUI.Controllers;

public class ArtistController(
    IArtistService artistService,
    ISongService songService,
    IUserSessionService userSessionService) : Controller
{
    public async Task<IActionResult> Artists()
    {
        ViewBag.Session = userSessionService.GetCurrent();
        return View("Artists", await artistService.GetArtistsWithAlbumsAsync());

    }

    public async Task<IActionResult> ArtistDetail(int id)
    {
        var artists = await artistService.GetArtistsWithAlbumsAsync();
        var artist = artists.FirstOrDefault(x => x.Id == id);
        if (artist is null)
        {
            return RedirectToAction(nameof(Artists));
        }

        var songs = await songService.GetSongsWithAlbumAsync();
        ViewBag.ArtistSongs = songs.Where(x => artist.Albums.Any(a => a.Id == (x.Album?.Id ?? 0))).ToList();
        ViewBag.Session = userSessionService.GetCurrent();
        return View("ArtistDetail", artist);
    }
}