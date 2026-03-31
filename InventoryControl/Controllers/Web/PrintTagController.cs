using Microsoft.AspNetCore.Mvc;

namespace InventoryControl.Controllers.Web;

public class PrintTagController : Controller
{
    [HttpGet("/printtag")]
    public IActionResult Index()
    {
        var user = HttpContext.Session.GetString("UserId");
        if (user == null)
            return Redirect("/login");
        ViewData["pages"] = "print-tag";
        ViewData["parent"] = "";
        return View();
    }
}
