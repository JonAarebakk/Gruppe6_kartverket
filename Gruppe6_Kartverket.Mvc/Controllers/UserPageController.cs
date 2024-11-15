
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
            //CaseRecords = GetCases() // Assuming GetCases is a method that retrieves cases for the user
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
    /*
    private List<CaseRecord> GetCases()
    {
        return new List<CaseRecord>
        {
            new CaseRecord { CaseId = 1, Title = "Case 1", Date = DateTime.Now, Status = "Open" },
            new CaseRecord { CaseId = 2, Title = "Case 2", Date = DateTime.Now, Status = "Closed" }
        };
    }*/
}
