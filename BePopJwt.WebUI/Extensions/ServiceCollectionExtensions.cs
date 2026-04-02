using BePopJwt.WebUI.Services;
using BePopJwt.WebUI.Services.AccountServices;
using BePopJwt.WebUI.Services.ArtistServices;
using BePopJwt.WebUI.Services.AuthServices;
using BePopJwt.WebUI.Services.PackageServices;
using BePopJwt.WebUI.Services.PlayerServices;
using BePopJwt.WebUI.Services.SongServices;
using BePopJwt.WebUI.Services.UserSessionServices;

namespace BePopJwt.WebUI.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebUiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllersWithViews();
        services.AddHttpContextAccessor();
        services.AddScoped<IUserSessionService, UserSessionService>();

        var apiBaseUrl = configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7068/";

        services.AddHttpClient<IApiAuthService, ApiAuthService>(c => c.BaseAddress = new Uri(apiBaseUrl));
        services.AddHttpClient<IApiAccountService, ApiAccountService>(c => c.BaseAddress = new Uri(apiBaseUrl));
        services.AddHttpClient<IArtistService, ApiArtistService>(c => c.BaseAddress = new Uri(apiBaseUrl));
        services.AddHttpClient<IApiPackageService, ApiPackageService>(c => c.BaseAddress = new Uri(apiBaseUrl));
        services.AddHttpClient<ISongService, ApiSongService>(c => c.BaseAddress = new Uri(apiBaseUrl));
        services.AddHttpClient<IApiPlayerService, ApiPlayerService>(c => c.BaseAddress = new Uri(apiBaseUrl));

        return services;
    }
}