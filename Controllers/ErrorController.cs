using Microsoft.AspNetCore.Mvc;

public class ErrorController : Controller
{
    public IActionResult PageNotFound()
    {
        return View();
    }
}
