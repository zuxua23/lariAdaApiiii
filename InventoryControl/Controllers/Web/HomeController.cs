using Microsoft.AspNetCore.Mvc;

namespace InventoryControl.Controllers.Web;

public class HomeController : Controller
{
    [HttpGet("/home/index")]
    public IActionResult Index()
    {
        return View();
    }
}