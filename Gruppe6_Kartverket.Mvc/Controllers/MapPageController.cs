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

    [HttpGet]
    public IActionResult MapPage()
    {
        if (User.Identity.IsAuthenticated) { ViewBag.LoggedIn = true; }
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
            return View("MapPage", model);
        }
        else
        {
            ViewBag.ShowSubmittedPopup = true;
            ModelState.Clear();

            var identityUser = await _userManager.GetUserAsync(User);

            if (identityUser != null)
            {
                var newLocationId = (_dbContext.CaseLocations.Max(cl => (int?)cl.LocationId) ?? 0) + 1;

                var caseLocation = new CaseLocation
                {
                    LocationId = newLocationId,     // Set the increment from the database
                    GeoJSON = model.GeoJson,
                    Municipality = "",  // Get via kartverket API
                    County = ""         // Get via kartverket API
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
            return RedirectToAction("LandingPage","LandingPage");
        }
    }
}