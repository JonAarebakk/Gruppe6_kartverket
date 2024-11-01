using Microsoft.AspNetCore.Mvc;

namespace Gruppe6_Kartverket.Mvc.Controllers;

public class CaseWorkerController : Controller
{
    // GET
    public IActionResult CaseWorkerPage()
    {
        return View();
    }
}