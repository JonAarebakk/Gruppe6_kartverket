using Microsoft.AspNetCore.Mvc; // Enables MVC features.
using Gruppe6_Kartverket.Mvc.Models; // Imports models for the application.

namespace Gruppe6_Kartverket.Mvc.Controllers; // Namespace for the controller.

public class SettingsController : Controller // Manages settings-related actions.
{
    // GET: Renders the settings page.
    public IActionResult Settings()
    {
        var model = new SettingsModel(); // Initializes the settings model.
        return View(model); // Returns the settings view with the model.
    }

    [HttpPost]
    public IActionResult Save(SettingsModel model) // Handles saving settings.
    {
        if (ModelState.IsValid) // Validates the input data.
        {
            // Add save logic here (e.g., save to database or configuration).
            return RedirectToAction("Settings"); // Redirects back to the settings page.
        }

        return View("Settings", model); // Returns the view with validation errors.
    }
}