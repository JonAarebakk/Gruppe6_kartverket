using Gruppe6_Kartverket.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Gruppe6_Kartverket.Mvc.Controllers
{
    public class LandingPageController : Controller
    {
        // Displays the LandingPage view
        [HttpGet]
        public IActionResult LandingPage()
        {
            return View();
        }

        // Displays the LinkPage view
        public IActionResult LinkPage()
        {
            return View();
        }

        // Returns the error view with request information
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}