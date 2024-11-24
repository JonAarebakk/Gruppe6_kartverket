using Gruppe6_Kartverket.Mvc.Models; // Imports models used by the application.
using Microsoft.AspNetCore.Mvc; // Enables MVC features like controllers and views.
using System.Diagnostics; // Provides diagnostic tools for debugging.
using System.Text.Json; // Supports JSON serialization.
using Microsoft.AspNetCore.Authorization; // Enables authorization features.
using Microsoft.AspNetCore.Identity; // Manages user authentication and identity.

namespace Gruppe6_Kartverket.Mvc.Controllers; // Namespace for the controller.

public class HomeController : Controller // Manages homepage-related actions.
{
    private readonly ILogger<HomeController> _logger; // Logs application activities.
    private readonly UserManager<IdentityUser> _userManager; // Manages user identity.

    // Constructor to inject logger and user manager.
    public HomeController(
        ILogger<HomeController> logger,
        UserManager<IdentityUser> userManager)
    {
        _logger = logger; // Assigns the logger.
        _userManager = userManager; // Assigns the user manager.
    }

    public IActionResult Index() // Displays the landing page.
    {
        var model = new HomeViewModel(); // Creates a view model instance.
        model.Message = "It's scuffed, but it works"; // Sets a default message.
        return View("LandingPage", model); // Returns the landing page view.
    }

    // Placeholder for a commented-out POST method handling map data.
/*
    [HttpPost]
    public IActionResult Index(HomeViewModel model)
    {
        if (!ModelState.IsValid) // Validates the model state.
            return View("Index", model); // Returns the index view with the model if invalid.

        if (model.Hidden != null) // Checks if hidden data is present.
        {
            var mapData = JsonSerializer.Deserialize<MapData>(model.Hidden, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }); // Deserializes hidden data.
            mapDataList.Add(mapData); // Adds map data to the list.
        }
        model.Message = model.NewMessage; // Updates the message.
        model.NewMessage = null; // Clears the new message field.
        return View("Index", model); // Returns the updated index view.
    }
*/

    public IActionResult Privacy() // Displays the privacy policy page.
    {
        return View(); // Returns the privacy view.
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] // Disables caching for the error page.
    public IActionResult Error() // Handles errors.
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }); // Returns the error view with request details.
    }
}
