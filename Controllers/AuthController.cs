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

        if (admin != null && admin.VerifyPassword(loginRequest.Password) && admin.Xoa == false)
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
            var Role = admin.Role;

            TempData["SuccessMessage"] = "Đăng nhập thành công";
            return RedirectToAction("AdminManager", "AdminManager");
        }
        else
        {
            ViewBag.ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng";
            return View("Login");
        }
    }
    public class RegisterRequest
    {
        public string? Ten { get; set; }
        public string? Email { get; set; }
        public string? Matkhau { get; set; }
        public int? Role { get; set; }
    }
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterRequest data)
    {
        var existingAdmin = await _context.Admins.SingleOrDefaultAsync(a => a.Email == data.Email);
        if (existingAdmin != null)
        {
            ViewBag.ErrorMessage = "Email đã tồn tại.";
            return View();
        }

        var admin = new Admins
        {
            Email = data.Email,
            Ten = data.Ten,
            Role = data.Role,
            MatKhau = "123", // Default password, should be securely generated
            Xoa = false,
        };
        admin.SetPassword(data.Matkhau);

        _context.Admins.Add(admin);
        await _context.SaveChangesAsync();

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
    public class UpdateAccountModel
    {
        public int? Id { get; set; }
        public string? Ten { get; set; }
        public string? Email { get; set; }
        public string? Matkhau { get; set; }
        public int? Role { get; set; }
    }
    [HttpPost]
    public async Task<IActionResult> CapNhatTaiKhoan([FromBody] List<UpdateAccountModel> updates)
    {
        if (updates == null || !updates.Any())
        {
            return BadRequest("Không có dữ liệu để cập nhật.");
        }

        foreach (var update in updates)
        {
            // Tìm admin theo Id
            var admin = await _context.Admins.FindAsync(update.Id);
            if (admin != null)
            {
                // Cập nhật thông tin
                admin.Ten = update.Ten;
                admin.Email = update.Email;

                // Cập nhật mật khẩu nếu có giá trị mới
                if (!string.IsNullOrWhiteSpace(update.Matkhau))
                {
                    admin.SetPassword(update.Matkhau);
                }

                admin.Role = update.Role;

                // Cập nhật vào cơ sở dữ liệu
                _context.Admins.Update(admin);
            }
        }

        // Lưu thay đổi
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Cập nhật tài khoản thành công!" });
    }
    public class IdRequest
    {
        public int Id { get; set; }
    }
    [HttpPost]
    public async Task<IActionResult> DanhDauXoaTaiKhoan([FromBody] IdRequest data)
    {
        if (data == null)
        {
            return BadRequest("Không có dữ liệu để cập nhật.");
        }

        // Tìm admin theo Id
        var admin = await _context.Admins.FindAsync(data.Id);
        if (admin != null)
        {
            admin.Xoa = true;
        }

        // Lưu thay đổi
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Vô hiệu tài khoản thành công." });
    }
    public class IdReload
    {
        public int Id { get; set; }
    }
    [HttpPost]
    public async Task<IActionResult> KhoiPhucTaiKhoan([FromBody] IdReload data)
    {
        if (data == null)
        {
            return BadRequest("Không có dữ liệu để cập nhật.");
        }

        // Tìm admin theo Id
        var admin = await _context.Admins.FindAsync(data.Id);
        if (admin != null)
        {
            admin.Xoa = false;
        }

        // Lưu thay đổi
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Khôi phục tài khoản thành công." });
    }



}
