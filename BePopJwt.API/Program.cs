using Amazon;
using Amazon.S3;
using BePopJwt.API.Extensions;
using BePopJwt.API.Services.Storage;
using BePopJwt.Business.Extensions;
using BePopJwt.Business.Services.StorageServices;
using BePopJwt.DataAccess.Context;
using BePopJwt.DataAccess.Extensions;
using BePopJwt.DataAccess.Interceptors;
using BePopJwt.Entity.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRepositoriesExt(builder.Configuration);
builder.Services.AddServiceRegistrationExt();


builder.Services.AddIdentityServices();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddAwsStorage(builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();