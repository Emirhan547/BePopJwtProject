using BePopJwt.WebUI.Dtos.HomeDtos;
using BePopJwt.WebUI.Models;
using BePopJwt.WebUI.Services.ArtistServices;
using BePopJwt.WebUI.Services.PackageServices;
using BePopJwt.WebUI.Services.SongServices;
using BePopJwt.WebUI.Services.UserSessionServices;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BePopJwt.WebUI.Controllers;

public class HomeController(
    ISongService songService,
    IArtistService artistService,
    IApiPackageService packageService,
    IUserSessionService userSessionService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var vm = new HomeViewModel
        {
           
            Songs = await songService.GetSongsWithAlbumAsync(),
            Artists = await artistService.GetArtistsWithAlbumsAsync(),
            Packages = await packageService.GetPackagesAsync(),
            Session = userSessionService.GetCurrent()
};


        return View(vm);
}


public IActionResult Privacy()
{
    return View();
}

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
public IActionResult Error()
{
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
}