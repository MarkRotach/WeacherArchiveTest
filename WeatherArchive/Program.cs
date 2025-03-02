using Microsoft.EntityFrameworkCore;
using WeatherArchive.Database;
using WeatherArchive.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.AddControllersWithViews();

services.AddDbContextPool<WeatherArchiveDbContext>(
    options => options.UseSqlServer(
        configuration.GetConnectionString("DefaultConnection")
    )
);

services.AddScoped<ArchiveUploader>()
    .AddScoped<ArchiveService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();