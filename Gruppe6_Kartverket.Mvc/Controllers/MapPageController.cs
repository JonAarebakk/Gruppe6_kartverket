using Gruppe6_Kartverket.Mvc.Data; // Imports application-specific data context.
using Gruppe6_Kartverket.Mvc.Models; // Imports models used in the application.
using Gruppe6_Kartverket.Mvc.Models.Database; // Imports database-related models.
using Microsoft.AspNetCore.Identity; // Provides identity management.
using Microsoft.AspNetCore.Mvc; // Enables MVC features like controllers and views.
using System.Diagnostics; // For debugging purposes.

namespace Gruppe6_Kartverket.Mvc.Controllers; // Namespace for the controller.

public class MapPageController : Controller // Manages map-related pages and actions.
{
    private readonly UserManager<IdentityUser> _userManager; // Manages user authentication.
    private readonly ApplicationDbContext _dbContext; // Provides access to the database.

    // Constructor to inject dependencies.
    public MapPageController(UserManager<IdentityUser> userManager, ApplicationDbContext dbContext)
    {
        _userManager = userManager; // Assigns the user manager.
        _dbContext = dbContext; // Assigns the database context.
    }

    [HttpGet]
    public IActionResult MapPage() // Displays the map page.
    {
        if (User.Identity.IsAuthenticated) // Checks if the user is logged in.
        {
            ViewBag.LoggedIn = true; // Sets a flag to show logged-in status.
        }

        ViewBag.HideFooter = true; // Hides the footer on this page.
        return View(); // Returns the map page view.
    }

    [HttpPost]
    [ValidateAntiForgeryToken] // Protects against CSRF attacks.
    public async Task<IActionResult> MapPage(CaseRegistrationModel model) // Handles case registration.
    {
        ViewBag.HideFooter = true; // Hides the footer on this page.

        if (!ModelState.IsValid) // Validates the form data.
        {
            foreach (var state in ModelState) // Logs validation errors.
            {
                if (state.Value.Errors.Count > 0)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Debug.WriteLine($"Key: {state.Key}, Error: {error.ErrorMessage}");
                    }
                }
            }

            return View(model); // Returns the view with validation messages.
        }
        else
        {
            ViewBag.ShowSubmittedPopup = true; // Shows a success popup.
            ModelState.Clear(); // Clears the form state.

            var identityUser = await _userManager.GetUserAsync(User); // Retrieves the logged-in user.

            if (identityUser != null)
            {
                var newLocationId = (_dbContext.CaseLocations.Max(cl => (int?)cl.LocationId) ?? 0) + 1; // Generates a new location ID.

                var caseLocation = new CaseLocation // Creates a new case location.
                {
                    LocationId = newLocationId, // Sets the new location ID.
                    GeoJSON = model.GeoJson, // Assigns geoJSON data.
                    Municipality = "", // Placeholder for municipality data.
                    County = "" // Placeholder for county data.
                };

                var userId = Guid.Parse(identityUser.Id); // Converts user ID to Guid.

                var caseRecord = new CaseRecord // Creates a new case record.
                {
                    CaseDate = DateTime.UtcNow, // Sets the current date and time.
                    CaseTitle = model.CaseTitle, // Assigns the case title.
                    CaseIssueType = model.Kategori, // Assigns the issue type.
                    CaseDescription = model.Beskrivelse, // Assigns the description.
                    CaseStatus = CaseStatus.Open.ToString(), // Sets the status to "Open."
                    CaseLocation = caseLocation, // Links the location.
                    User = _dbContext.Users.FirstOrDefault(u => u.UserId == userId) // Links the user.
                };

                _dbContext.CaseRecords.Add(caseRecord); // Adds the case record to the database.
                _dbContext.CaseLocations.Add(caseLocation); // Adds the location to the database.
                await _dbContext.SaveChangesAsync(); // Saves the changes.
            }

            return RedirectToAction("LandingPage", "LandingPage"); // Redirects to the landing page.
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken] // Protects against CSRF attacks.
    public async Task<IActionResult> DeleteCase(int caseId) // Handles case deletion.
    {
        var caseRecord = await _dbContext.CaseRecords.FindAsync(caseId); // Finds the case by ID.
        if (caseRecord != null)
        {
            _dbContext.CaseRecords.Remove(caseRecord); // Removes the case from the database.
            await _dbContext.SaveChangesAsync(); // Saves the changes.
        }

        return RedirectToAction("Userpage", "UserPage"); // Redirects to the user page.
    }

    [HttpPost]
    [ValidateAntiForgeryToken] // Protects against CSRF attacks.
    public async Task<IActionResult> EditCaseDescription(int caseId, string newDescription) // Edits the case description.
    {
        var caseRecord = await _dbContext.CaseRecords.FindAsync(caseId); // Finds the case by ID.
        if (caseRecord != null)
        {
            caseRecord.CaseDescription = newDescription; // Updates the description.
            await _dbContext.SaveChangesAsync(); // Saves the changes.
        }

        return RedirectToAction("UserPage", "UserPage"); // Redirects to the user page.
    }
}
