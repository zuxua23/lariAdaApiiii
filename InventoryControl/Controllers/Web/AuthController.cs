using Microsoft.AspNetCore.Mvc;

namespace InventoryControl.Controllers.Web;

public class AuthController : Controller
{
    [HttpGet("/login")]
    public IActionResult Login()
    {
        var user = HttpContext.Session.GetString("UserId");

        if (user != null)
            return Redirect("/home/index");

        return View();
    }
}
