//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.EntityFrameworkCore;
//using feedBackMvc.Models; // Đảm bảo namespace đúng
//using Dotenv;

//var builder = WebApplication.CreateBuilder(args);
//DotEnv.Load();
//// Add services to the container.
//builder.Services.AddControllersWithViews();
//builder.Services.AddControllers(); // Add controllers for API

//// Add DbContext for PostgreSQL
//builder.Services.AddDbContext<AppDbContext>(options =>
//{
//    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
//    options.EnableSensitiveDataLogging(); // Enable sensitive data logging
//});



//// Add session services
//builder.Services.AddDistributedMemoryCache();
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//// Use session before authentication
//app.UseSession();

//app.UseAuthorization();

//app.MapControllers(); // Map API controllers

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.Run();

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using feedBackMvc.Models; // Đảm bảo namespace đúng
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Tải biến môi trường từ tệp .env
LoadEnvironmentVariables();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers(); // Add controllers for API

// Add DbContext for PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.EnableSensitiveDataLogging(); // Enable sensitive data logging
});

// Add session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Use session before authentication
app.UseSession();

app.UseAuthorization();

app.MapControllers(); // Map API controllers

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

void LoadEnvironmentVariables()
{
    var envFilePath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
    if (File.Exists(envFilePath))
    {
        var envVariables = File.ReadAllLines(envFilePath);
        foreach (var line in envVariables)
        {
            var parts = line.Split('=', 2);
            if (parts.Length == 2)
            {
                Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }
        }
    }
}
