using Microsoft.AspNetCore.Mvc;
using Gruppe6_Kartverket.Mvc.Models.Database;

namespace Gruppe6_Kartverket.Mvc.Controllers;

public class CaseInfoController : Controller
{
    public IActionResult CaseInfoViewWithModel(CaseRecord model)
    {
        if(!ModelState.IsValid)
        {
            return View("CaseInfo", model);
        }
        
        return View("CaseInfo", model);
    }
}