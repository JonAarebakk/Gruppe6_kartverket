using Microsoft.AspNetCore.Mvc;

namespace Gruppe6_Kartverket.Mvc.Controllers;

public class CaseWorkerControllerV2 : Controller
{
    // GET
    public IActionResult CaseWorkerPageV2()
    {
        return View("CaseWorkerPage-V2");
    }
}