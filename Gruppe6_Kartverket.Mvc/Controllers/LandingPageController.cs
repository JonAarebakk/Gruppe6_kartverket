using Gruppe6_Kartverket.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Gruppe6_Kartverket.Mvc.Controllers
{
    public class LandingPageController : Controller
    {
        // GET: LandingPage
        [HttpGet]
        public IActionResult LandingPage()
        {
            return View();
        }
        
        
        public IActionResult LinkPage()
        {
            return View();
        }
        
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}