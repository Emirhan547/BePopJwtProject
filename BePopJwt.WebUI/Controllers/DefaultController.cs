using BePopJwt.WebUI.Const;
using BePopJwt.WebUI.Dtos.HomeDtos;
using BePopJwt.WebUI.Services.CatalogServices;
using BePopJwt.WebUI.Services.PlayerServices;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Utilities;

namespace BePopJwt.WebUI.Controllers;

public class DefaultController(IApiCatalogService catalogService, IApiPlayerService playerService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var vm = new HomeViewModel
        {
            Songs = await catalogService.GetSongsWithAlbumAsync(),
            Artists = await catalogService.GetArtistsWithAlbumsAsync(),
            Packages = await catalogService.GetPackagesAsync()
        };

        return View(vm);
    }

    public async Task<IActionResult> Artists()
    {
        var artists = await catalogService.GetArtistsWithAlbumsAsync();
        return View(artists);
    }

    public async Task<IActionResult> Charts()
    {
        var songs = await catalogService.GetSongsWithAlbumAsync();
        return View(songs.OrderBy(s => s.Level).ThenBy(s => s.Name).ToList());
    }

    public async Task<IActionResult> Discover()
    {
        var token = Request.Cookies[AuthCookieNames.JwtToken];
        if (string.IsNullOrWhiteSpace(token))
        {
            return View(new DiscoverViewModel
            {
                IsAuthenticated = false,
                ErrorMessage = "Bu sayfada paket yetkine uygun içerikleri görmek için giriş yapmalısın."
            });
        }

        var result = await playerService.GetAccessibleSongsAsync(token);
        return View(new DiscoverViewModel
        {
            IsAuthenticated = true,
            AccessibleSongs = result.Songs,
            ErrorMessage = result.Error
        });
    }
}