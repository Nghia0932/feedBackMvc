using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using feedBackMvc.Models; // Ensure this namespace is correct
using feedBackMvc.Helpers;

using Microsoft.Extensions.Logging;

public class AdminManagerController : Controller
{
    private readonly AppDbContext _appDbContext;
    private readonly JwtTokenHelper _jwtTokenHelper;
    private readonly ILogger<AdminManagerController> _logger;

    public AdminManagerController(AppDbContext appDbContext, JwtTokenHelper jwtTokenHelper, ILogger<AdminManagerController> logger)
    {
        _appDbContext = appDbContext;
        _jwtTokenHelper = jwtTokenHelper;
        _logger = logger;
    }

    public async Task<IActionResult> AdminManager()
    {
        //// Check Access Token in Cookie
        //if (!Request.Cookies.TryGetValue("AccessToken", out var token))

        var token = HttpContext.Session.GetString("AccessToken");
        if (string.IsNullOrEmpty(token))
        {
            _logger.LogWarning("AccessToken not found in session.");
            return RedirectToAction("Login", "Auth");
        }

        // Try to decrypt the token and get the admin ID
        if (!_jwtTokenHelper.TryParseToken(token, out var adminId))
        {
            _logger.LogWarning("Failed to parse token.");
            return RedirectToAction("Login", "Auth");
        }

        // Fetch Admin by ID
        var admin = await _appDbContext.Admins.FindAsync(adminId);

        if (admin == null)
        {
            _logger.LogWarning($"Admin with ID {adminId} not found.");
            return NotFound(); // Return a 404 Not Found if the admin is not found
        }

        // Pass the admin object to the view
        return View(admin);
    }
    public async Task<IActionResult> AdminInfo()
    {
        var token = HttpContext.Session.GetString("AccessToken");
        if (string.IsNullOrEmpty(token))
        {
            _logger.LogWarning("AccessToken not found in session.");
            return RedirectToAction("Login", "Auth");
        }

        // Try to decrypt the token and get the admin ID
        if (!_jwtTokenHelper.TryParseToken(token, out var adminId))
        {
            _logger.LogWarning("Failed to parse token.");
            return RedirectToAction("Login", "Auth");
        }

        // Fetch Admin by ID
        var admin = await _appDbContext.Admins.FindAsync(adminId);

        if (admin == null)
        {
            _logger.LogWarning($"Admin with ID {adminId} not found.");
            return NotFound(); // Return a 404 Not Found if the admin is not found
        }

        ViewData["PartialView"] = "_AdminInfo";
        return PartialView("AdminManager",admin); // Hoặc trả về PartialView("TênPartialView") nếu không sử dụng ViewData
    }


}
