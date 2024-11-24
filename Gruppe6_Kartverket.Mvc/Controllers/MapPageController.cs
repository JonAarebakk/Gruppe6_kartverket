using Gruppe6_Kartverket.Mvc.Data;
using Gruppe6_Kartverket.Mvc.Models;
using Gruppe6_Kartverket.Mvc.Models.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
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

    // Displays the map page view with footer hidden if authenticated
    [HttpGet]
    public IActionResult MapPage()
    {
        if (User.Identity.IsAuthenticated)
        {
            ViewBag.LoggedIn = true;
        }

        ViewBag.HideFooter = true;

        return View();
    }

    // Handles the form submission to register a new case
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MapPage(CaseRegistrationModel model)
    {
        ViewBag.HideFooter = true;

        if (!ModelState.IsValid)
        {
            // Logs validation errors
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
            ViewBag.ShowSubmittedPopup = true;
            ModelState.Clear();

            var identityUser = await _userManager.GetUserAsync(User);

            if (identityUser != null)
            {
                // Generate a new Location ID and add CaseLocation and CaseRecord to the database
                var newLocationId = (_dbContext.CaseLocations.Max(cl => (int?)cl.LocationId) ?? 0) + 1;

                var caseLocation = new CaseLocation
                {
                    LocationId = newLocationId,
                    GeoJSON = model.GeoJson,
                    Municipality = "", // Placeholder, could be fetched via an API
                    County = "" // Placeholder, could be fetched via an API
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

            return RedirectToAction("LandingPage", "LandingPage");
        }
    }

    // Deletes a case record based on the provided caseId
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

        return RedirectToAction("Userpage", "UserPage");
    }

    // Edits the description of a case record
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

        return RedirectToAction("UserPage", "UserPage");
    }
}
