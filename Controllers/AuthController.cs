using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using feedBackMvc.Models; // Ensure the namespace is correct
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

public class AuthController : Controller
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Login()
    {
        return View();
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
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
            HttpContext.Session.SetString("AccessToken", GenerateAccessToken(admin.Email));
            if (loginRequest.RememberMe)
            {
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(30),
                    HttpOnly = true,
                    Secure = true
                };
                Response.Cookies.Append("AccessToken", GenerateAccessToken(admin.Email), cookieOptions);
            }
           // Set success message in TempData
        TempData["SuccessMessage"] = "Đăng nhập thành công";
        return RedirectToAction("AdminManager", "AdminManager", new { adminName = admin.Ten });

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
        // Check if email already exists
        var existingAdmin = await _context.Admins.SingleOrDefaultAsync(a => a.Email == model.Email);
        if (existingAdmin != null)
        {
            ViewBag.ErrorMessage = "Email đã tồn tại.";
            return View();
        }

        // Create new account
        var admin = new Admins
        {
            Email = model.Email,
            Ten = model.Ten,
            MatKhau = "123", // Default password, should be securely generated
        };
        admin.SetPassword(model.MatKhau);

        _context.Admins.Add(admin);
        await _context.SaveChangesAsync();

        // Log in immediately after successful registration
        HttpContext.Session.SetString("AccessToken", GenerateAccessToken(admin.Email));
        return RedirectToAction("AdminManager", "AdminManager");
    }

    private string GenerateAccessToken(string email)
    {
        var secretKey = Environment.GetEnvironmentVariable("SECRET_KEY");
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "feedBack",
            audience: "Admin",
            claims: new[] { new Claim("Email", email) },
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
