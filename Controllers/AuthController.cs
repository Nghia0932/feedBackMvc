using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using feedBackMvc.Models; // Ensure the namespace is correct
using feedBackMvc.Helpers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class AuthController : Controller
{
    private readonly AppDbContext _context;
    private readonly JwtTokenHelper _jwtTokenHelper;

    public AuthController(AppDbContext context, JwtTokenHelper jwtTokenHelper)
    {
        _context = context;
        _jwtTokenHelper = jwtTokenHelper;
    }

    public IActionResult Login()
    {
        return View();
    }

    public class LoginRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool RememberMe { get; set; }
    }

    [HttpPost]
    public async Task<IActionResult> LoginAdmin(LoginRequest loginRequest)
    {
        if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Password))
        {
            ViewBag.ErrorMessage = "Password cannot be null or empty.";
            return View("Login");
        }

        var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == loginRequest.Email);

        if (admin != null && admin.VerifyPassword(loginRequest.Password))
        {
            // Generate the access token using the admin's ID
            var token = _jwtTokenHelper.GenerateAccessToken(admin.idAdmin);

            // Store the token in session
            HttpContext.Session.SetString("AccessToken", token);

            if (loginRequest.RememberMe)
            {
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(30),
                    HttpOnly = true,
                    Secure = true
                };
                // Store the token in a cookie if "Remember Me" is checked
                Response.Cookies.Append("AccessToken", token, cookieOptions);
            }

            TempData["SuccessMessage"] = "Đăng nhập thành công";
            return RedirectToAction("AdminManager", "AdminManager");
        }
        else
        {
            ViewBag.ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng";
            return View("Login");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] Admins model)
    {
        var existingAdmin = await _context.Admins.SingleOrDefaultAsync(a => a.Email == model.Email);
        if (existingAdmin != null)
        {
            ViewBag.ErrorMessage = "Email đã tồn tại.";
            return View();
        }

        var admin = new Admins
        {
            Email = model.Email,
            Ten = model.Ten,
            MatKhau = "123", // Default password, should be securely generated
        };
        admin.SetPassword(model.MatKhau);

        _context.Admins.Add(admin);
        await _context.SaveChangesAsync();

        // Generate and store the access token in session after registration
        var token = _jwtTokenHelper.GenerateAccessToken(admin.idAdmin);
        HttpContext.Session.SetString("AccessToken", token);

        return RedirectToAction("AdminManager", "AdminManager");
    }
     public IActionResult Logout()
    {
        // Clear the session
        HttpContext.Session.Remove("AccessToken");
        
        // Clear the cookie
        Response.Cookies.Delete("AccessToken");
        
        // Redirect to home page
        return RedirectToAction("Index", "Home");
    }
}
