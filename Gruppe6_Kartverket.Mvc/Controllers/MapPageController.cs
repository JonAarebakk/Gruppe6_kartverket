using Gruppe6_Kartverket.Mvc.Data;
using Gruppe6_Kartverket.Mvc.Models;
using Gruppe6_Kartverket.Mvc.Models.Database;
using Gruppe6_Kartverket.Mvc.Models.Services;
using Gruppe6_Kartverket.Mvc.Models.ViewModels;
using Gruppe6_Kartverket.Mvc.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Text.Json;

namespace Gruppe6_Kartverket.Mvc.Controllers;

public class MapPageController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _dbContext;


    public MapPageController(UserManager<IdentityUser> userManager, ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult MapPage()
    {
        if ((User.Identity.IsAuthenticated != null) && User.Identity.IsAuthenticated)
        {
            ViewBag.LoggedIn = "loggedIn";
        }

        ViewBag.HideFooter = true;

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MapPage(CaseRegistrationModel model)
    {
        ViewBag.HideFooter = true;


        if (!ModelState.IsValid)
        {
            foreach (var state in ModelState)
            {
                if (state.Value.Errors.Count > 0)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Debug.WriteLine($"Key: {state.Key}, Error: {error.ErrorMessage}");
                    }
                }
            }

            return View(model);
        }
        else
        {
            var identityUser = await _userManager.GetUserAsync(User);

            if (identityUser != null)
            {
                var newLocationId = (_dbContext.CaseLocations.Max(cl => (int?)cl.LocationId) ?? 0) + 1;

                var caseLocation = new CaseLocation
                {
                    LocationId = newLocationId, // Set the increment from the database
                    GeoJSON = model.GeoJson,
                        //"{\"type\":\"FeatureCollection\",\"features\":[{\"type\":\"Feature\",\"geometry\":{\"type\":\"Point\",\"coordinates\":[-122.4194,37.7749]},\"properties\":{\"name\":\"San Francisco\",\"type\":\"City\"}},{\"type\":\"Feature\",\"geometry\":{\"type\":\"LineString\",\"coordinates\":[[-122.4194,37.7749],[-118.2437,34.0522],[-74.0060,40.7128]]},\"properties\":{\"name\":\"Route 1\",\"description\":\"A route connecting SF, LA, and NYC\"}},{\"type\":\"Feature\",\"geometry\":{\"type\":\"Polygon\",\"coordinates\":[[[-123.0,37.0],[-123.0,38.0],[-122.0,38.0],[-122.0,37.0],[-123.0,37.0]]]},\"properties\":{\"name\":\"Area A\",\"description\":\"A polygonal area\"}}]}",
                    Municipality = "", // Get via kartverket API
                    County = "" // Get via kartverket API
                };

                var userId = Guid.Parse(identityUser.Id);

                var caseRecord = new CaseRecord
                {
                    CaseDate = DateTime.UtcNow,
                    CaseTitle = model.CaseTitle,
                    CaseIssueType = model.Kategori,
                    CaseDescription = model.Beskrivelse,
                    CaseStatus = CaseStatus.Open.ToString(),
                    CaseLocation = caseLocation,
                    User = _dbContext.Users.FirstOrDefault(u => u.UserId == userId)
                };

                _dbContext.CaseRecords.Add(caseRecord);
                _dbContext.CaseLocations.Add(caseLocation);
                await _dbContext.SaveChangesAsync();
            }
            ViewBag.ShowSubmittedPopup = true;
            ModelState.Clear();

            return RedirectToAction("MapPage", "MapPage");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCase(int caseId)
    {
        var caseRecord = await _dbContext.CaseRecords.FindAsync(caseId);
        if (caseRecord != null)
        {
            _dbContext.CaseRecords.Remove(caseRecord);
            await _dbContext.SaveChangesAsync();
        }

        return RedirectToAction("Userpage", "UserPage"); // Adjust the redirect action as needed
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditCaseDescription(int caseId, string newDescription)
    {
        var caseRecord = await _dbContext.CaseRecords.FindAsync(caseId);
        if (caseRecord != null)
        {
            caseRecord.CaseDescription = newDescription;
            await _dbContext.SaveChangesAsync();
        }

        return RedirectToAction("UserPage", "UserPage"); // Adjust the redirect action as needed
    }
}