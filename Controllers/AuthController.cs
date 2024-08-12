using Microsoft.AspNetCore.Mvc;

namespace feedBackMvc.Controllers
{
    public class AuthController : Controller
    {
        public object Login()
        {
            return View();
        }
    }
}