using Microsoft.AspNetCore.Mvc;

namespace feedBackMvc.Controllers
{
    public class AuthController : Controller
    {
        public ViewResult Login()
        {
            return View();
        }
    }
}