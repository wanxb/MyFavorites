using MyFavorites.Core.Models;
using MyFavorites.Core.Services;
using MyFavorites.Core.Services.Favorites;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddMyFavorites(builder.Configuration);

builder.Services.Configure<DataBaseSettings>(
    builder.Configuration.GetSection("Database"));

using IHost host = Host.CreateApplicationBuilder(args).Build();
IConfiguration config = host.Services.GetRequiredService<IConfiguration>();
string? databaseType = config.GetValue<string>("Database:DatabaseType");

if (databaseType == DatabaseType.MongoDB.ToString())
    builder.Services.AddTransient<IFavoritesService, MongoDBService>();
else if (databaseType == DatabaseType.MySQL.ToString())
    builder.Services.AddTransient<IFavoritesService, MySQLService>();
else if (databaseType == DatabaseType.File.ToString())
    builder.Services.AddTransient<IFavoritesService, FileService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Favorites}/{action=Index}/{id?}");

app.UseStatusCodePagesWithRedirects("/NotFound"); // 设置自定义404页面的路径

app.Run();