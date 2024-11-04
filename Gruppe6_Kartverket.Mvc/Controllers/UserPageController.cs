
using Microsoft.AspNetCore.Mvc;
using Gruppe6_Kartverket.Mvc.Models;

namespace Gruppe6_Kartverket.Mvc.Controllers;

public class UserPageController : Controller
{

    // GET
    public IActionResult UserPage()
    {
        var model = new UserPageModel
        {
            UserName = "User Name",
            NewMessagesCount = 5,
            Cases = GetCases() // Assuming GetCases is a method that retrieves cases for the user
        };
        return View(model);
    }

    public IActionResult Settings()
    {
        // Logic to load settings
        return View("Settings");
    }

    public IActionResult Profile()
    {
        // Logic to load user profile
        return View("Profile");
    }

    public IActionResult Logout()
    {
        // Logic to handle logout
        return RedirectToAction("Index", "Home");
    }

    private List<Case> GetCases()
    {
        return new List<Case>
        {
            new Case { Id = 1, Title = "Case 1", Date = DateTime.Now, Status = "Open" },
            new Case { Id = 2, Title = "Case 2", Date = DateTime.Now, Status = "Closed" }
        };
    }
}
