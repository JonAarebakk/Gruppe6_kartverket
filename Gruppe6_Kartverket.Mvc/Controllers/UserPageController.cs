using Microsoft.AspNetCore.Mvc;

namespace Gruppe6_Kartverket.Mvc.Controllers;

public class UserPageController : Controller
{
    // GET
    public IActionResult UserPage()
    {
        return View();
    }
}