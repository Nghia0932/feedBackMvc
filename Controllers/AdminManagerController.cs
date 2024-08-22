using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

public class AdminManagerController : Controller
{
    public IActionResult  AdminManager()
    {
        // Kiểm tra Access Token trong Session hoặc Cookie
        if (HttpContext.Session.GetString("AccessToken") == null)
        {
            // Nếu không có Access Token, chuyển hướng đến trang đăng nhập
            return RedirectToAction("Login", "Auth");
        }
       
        // Nếu có Access Token, hiển thị trang quản trị
        return View();
    }
}
