using Microsoft.AspNetCore.Mvc;

namespace Gruppe6_Kartverket.Mvc.Controllers;

public class MapPageController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}