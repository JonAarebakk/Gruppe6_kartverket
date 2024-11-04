using Microsoft.AspNetCore.Mvc;
using Gruppe6_Kartverket.Mvc.Models;

namespace Gruppe6_Kartverket.Mvc.Controllers;

public class SettingsController : Controller
{
    // GET
    public IActionResult Settings()
    {
        var model = new SettingsModel();
        return View(model);
    }

    [HttpPost]
    public IActionResult Save(SettingsModel model)
    {
        if (ModelState.IsValid)
        {
            // Save logic here
            return RedirectToAction("Settings");
        }
        return View("Settings", model);
    }
}