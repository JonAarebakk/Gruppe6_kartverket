using Microsoft.AspNetCore.Mvc;
using Gruppe6_Kartverket.Mvc.Models;

namespace Gruppe6_Kartverket.Mvc.Controllers;

public class CaseController : Controller
{
    // GET
    public IActionResult CaseView()
    {
        var model = new CaseInfoModel();
        return View("CaseView", model);
    }
    
    public IActionResult CaseViewWithModel(CaseInfoModel model)
    {
        if(!ModelState.IsValid)
        {
            return View("CaseView", model);
        }
        
        return View("CaseView", model);
    }
}