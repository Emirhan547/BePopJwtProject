using BePopJwt.Business.Services.AlbumServices;
using BePopJwt.Business.Services.ArtistServices;
using BePopJwt.Business.Services.BannerServices;
using BePopJwt.Business.Services.PackageServices;
using BePopJwt.Business.Services.SongServices;
using BePopJwt.Business.Services.UserSongHistoryServices;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BePopJwt.Business.Extensions
{
    public static class ServiceRegistration
    {
        public static void AddServiceRegistrationExt(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<IArtistService, ArtistService>();
            services.AddScoped<IBannerService, BannerService>();
            services.AddScoped<IPackageService, PackageService>();
            services.AddScoped<ISongService, SongService>();
            services.AddScoped<IUserSongHistoryService, UserSongHistoryService>();

        }
    }
}
