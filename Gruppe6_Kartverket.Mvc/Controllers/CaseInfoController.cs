using Microsoft.AspNetCore.Mvc; // Enables MVC features like controllers and views.
using Microsoft.EntityFrameworkCore; // Provides ORM for database access.
using System.Threading.Tasks; // Supports asynchronous programming.
using Gruppe6_Kartverket.Mvc.Data; // Imports database context.
using Gruppe6_Kartverket.Mvc.Models.ViewModels; // Imports custom view models.

namespace Gruppe6_Kartverket.Mvc.Controllers // Namespace for the controller.
{
    public class CaseInfoController : Controller // Handles case-related actions.
    {
        private readonly ApplicationDbContext _context; // Represents the database context.

        public CaseInfoController(ApplicationDbContext context) // Constructor with dependency injection.
        {
            _context = context; // Injects the database context.
        }

        public async Task<IActionResult> CaseInfo(int caseRecordId) // Retrieves detailed case information.
        {
            // Queries the database for a case record, including related location and user data.
            var caseRecord = await _context.CaseRecords
                .Include(cr => cr.CaseLocation) // Includes location details.
                .Include(cr => cr.User) // Includes associated user details.
                .FirstOrDefaultAsync(cr => cr.CaseRecordId == caseRecordId); // Finds the case by ID.

            if (caseRecord == null) // Checks if the case record is found.
            {
                return NotFound(); // Returns a 404 error if not found.
            }

            // Maps database data to a view model for display.
            var viewmodel = new CaseDetailsViewModel
            {
                CaseRecordId = caseRecord.CaseRecordId, // Case ID.
                CaseDate = caseRecord.CaseDate, // Date of the case.
                CaseTitle = caseRecord.CaseTitle, // Title of the case.
                CaseIssueType = caseRecord.CaseIssueType, // Type of issue.
                CaseDescription = caseRecord.CaseDescription, // Description of the case.
                CaseStatus = caseRecord.CaseStatus, // Status of the case.
                LocationId = caseRecord.LocationId, // Location ID.
                GeoJSON = caseRecord.CaseLocation?.GeoJSON, // GeoJSON data for mapping.
                CaseLocation = caseRecord.CaseLocation, // Location details.
                User = caseRecord.User // Associated user details.
            };

            return View(viewmodel); // Passes the view model to the view.
        }
    }
}
