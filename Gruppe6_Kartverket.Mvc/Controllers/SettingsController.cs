using Microsoft.AspNetCore.Mvc;

namespace Gruppe6_Kartverket.Mvc.Controllers;

public class SettingsController : Controller
{
    // GET
    public IActionResult Settings()
    {
        return View();
    }
}