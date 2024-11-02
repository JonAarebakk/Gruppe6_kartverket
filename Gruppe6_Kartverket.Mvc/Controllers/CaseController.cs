using Microsoft.AspNetCore.Mvc;

namespace Gruppe6_Kartverket.Mvc.Controllers;

public class CaseController : Controller
{
    // GET
    public IActionResult CaseView()
    {
        return View();
    }
}