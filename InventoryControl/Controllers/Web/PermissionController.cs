namespace InventoryControl.Controllers.Web;

using Microsoft.AspNetCore.Mvc;


public class PermissionController : Controller
{
    [HttpGet("/permission")]
    public IActionResult Index()
    {
        var user = HttpContext.Session.GetString("UserId");

        if (user == null)
            return Redirect("/");

        ViewData["pages"] = "permission";
        ViewData["parent"] = "master";

        return View();
    }
    //[HttpGet("/user/detail")]
    //public IActionResult Detail(string id)
    //{
    //    ViewData["id"] = id;
    //    return View();
    //}
}