using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using feedBackMvc.Models;
using feedBackMvc.Helpers;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from .env file
LoadEnvironmentVariables();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers(); // Add controllers for API

// Add DbContext for PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    //options.EnableSensitiveDataLogging(); // Enable sensitive data logging
});

// Register JwtTokenHelper as a singleton
builder.Services.AddSingleton<JwtTokenHelper>();

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

app.UseStatusCodePages( context =>
{
    var response = context.HttpContext.Response;

    if (response.StatusCode == 404) // Handle 404 Not Found
    {
        response.Redirect("/Error/PageNotFound");
    }
    // Handle other status codes as needed
    return Task.CompletedTask; // Return a completed task since no await is used
});

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
