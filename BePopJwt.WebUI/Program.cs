using BePopJwt.WebUI.Services;
using BePopJwt.WebUI.Services.AccountServices;
using BePopJwt.WebUI.Services.AuthServices;
using BePopJwt.WebUI.Services.CatalogServices;
using BePopJwt.WebUI.Services.PlayerServices;
using BePopJwt.WebUI.Services.UserSessionServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserSessionService, UserSessionService>();

var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7068/";
builder.Services.AddHttpClient<IApiAuthService, ApiAuthService>(c => c.BaseAddress = new Uri(apiBaseUrl));
builder.Services.AddHttpClient<IApiAccountService, ApiAccountService>(c => c.BaseAddress = new Uri(apiBaseUrl));
builder.Services.AddHttpClient<IApiCatalogService, ApiCatalogService>(c => c.BaseAddress = new Uri(apiBaseUrl));
builder.Services.AddHttpClient<IApiPlayerService, ApiPlayerService>(c => c.BaseAddress = new Uri(apiBaseUrl));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
   pattern: "{controller=Default}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
