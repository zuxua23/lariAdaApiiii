using Microsoft.AspNetCore.Mvc;

namespace InventoryControl.Controllers.Web;

public class HomeController : Controller
{

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}