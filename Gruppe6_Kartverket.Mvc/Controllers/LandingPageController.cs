using Gruppe6_Kartverket.Mvc.Models; // Imports application-specific models.
using Microsoft.AspNetCore.Mvc; // Provides MVC features like controllers and views.
using System.Diagnostics; // Provides tools for debugging.

namespace Gruppe6_Kartverket.Mvc.Controllers // Namespace for the controller.
{
    public class LandingPageController : Controller // Controller for landing page-related actions.
    {
        // Handles GET requests for the landing page.
        [HttpGet]
        public IActionResult LandingPage()
        {
            return View(); // Returns the landing page view.
        }
        
        // Handles requests for a linked page.
        public IActionResult LinkPage()
        {
            return View(); // Returns the link page view.
        }
        
        // Handles error cases and provides error details.
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }); // Returns the error view with the request ID.
        }
    }
}