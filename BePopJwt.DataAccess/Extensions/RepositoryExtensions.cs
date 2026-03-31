using BePopJwt.DataAccess.Context;
using BePopJwt.DataAccess.Interceptors;
using BePopJwt.DataAccess.Repositories.AlbumRepositories;
using BePopJwt.DataAccess.Repositories.ArtistRepositories;
using BePopJwt.DataAccess.Repositories.BannerRepositories;
using BePopJwt.DataAccess.Repositories.PackageRepositories;
using BePopJwt.DataAccess.Repositories.PlayerRepositories;
using BePopJwt.DataAccess.Repositories.SongRepositories;
using BePopJwt.DataAccess.Repositories.UserSongRepositories;
using BePopJwt.DataAccess.Uow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.DataAccess.Extensions
{
    public static class RepositoryExtensions
    {
        public static void AddRepositoriesExt(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped<IAlbumRepository, AlbumRepository>();
            services.AddScoped<IArtistRepository, ArtistRepository>();
            services.AddScoped<IBannerRepository, BannerRepository>();
            services.AddScoped<IPackageRepository, PackageRepository>();
            services.AddScoped<ISongRepository, SongRepository>();
            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserSongHistoryRepository, UserSongHistoryRepository>();
            services.AddScoped<AuditInterceptor>();
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
                options.AddInterceptors(new AuditInterceptor());
            });
        }
    }
}
