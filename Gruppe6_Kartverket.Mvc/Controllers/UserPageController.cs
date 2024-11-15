
using Microsoft.AspNetCore.Mvc;
using Gruppe6_Kartverket.Mvc.Models.Database;

namespace Gruppe6_Kartverket.Mvc.Controllers;

public class UserPageController : Controller
{

    // GET
    public IActionResult UserPage()
    {
        var model = new User
        {
            UserName = "User Name",
            //NewMessagesCount = 5,
            CaseRecords = GetCases() // Assuming GetCases is a method that retrieves cases for the user
        };
        ViewBag.HideFooter = true; // Hide footer in this view
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

    private List<CaseRecord> GetCases()
    {
        return new List<CaseRecord>
        {
            new CaseRecord { UserId = 1, CaseTitle = "Case 1", CaseDate = DateTime.Now, CaseStatus = "Open" },
            new CaseRecord { UserId = 2, CaseTitle = "Case 2", CaseDate = DateTime.Now, CaseStatus = "Closed" }
        };
    }
}
