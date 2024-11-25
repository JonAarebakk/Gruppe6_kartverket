using Gruppe6_Kartverket.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Gruppe6_Kartverket.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<IdentityUser> _userManager;

    // Constructor that injects the logger and UserManager for managing user data
    public HomeController(
        ILogger<HomeController> logger,
        UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    // Action method that returns the landing page view with a model containing a message
    public IActionResult Index()
    {
        var model = new HomeViewModel();
        model.Message = "It's scuffed, but it works"; // Setting a test message
        return View("LandingPage", model); // Passes the model to the "LandingPage" view
    }

    // Privacy page action that returns the privacy view (no additional logic)
    public IActionResult Privacy()
    {
        return View(); // Displays the privacy page
    }

    // Error handling action that returns the error view with request information
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        // Returns an error view with the current request identifier for error tracking
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}