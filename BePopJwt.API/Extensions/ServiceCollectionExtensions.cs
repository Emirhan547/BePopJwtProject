using Amazon;
using Amazon.S3;
using BePopJwt.API.Options;
using BePopJwt.API.Services.Storage;
using BePopJwt.Business.Services.StorageServices;
using BePopJwt.Entity.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BePopJwt.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() ?? new JwtOptions();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                    ClockSkew = TimeSpan.Zero
                };
            });

        return services;
    }

    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, IdentityRole<int>>()
            .AddEntityFrameworkStores<BePopJwt.DataAccess.Context.AppDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    public static IServiceCollection AddAwsStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AwsS3Options>(configuration.GetSection(AwsS3Options.SectionName));
        var awsOptions = configuration.GetSection(AwsS3Options.SectionName).Get<AwsS3Options>() ?? new AwsS3Options();

        services.AddSingleton<IAmazonS3>(_ => new AmazonS3Client(
            awsOptions.AccessKey,
            awsOptions.SecretKey,
            RegionEndpoint.GetBySystemName(awsOptions.Region)));

        services.AddScoped<IAudioStorageService, S3AudioStorageService>();
        return services;
    }
}