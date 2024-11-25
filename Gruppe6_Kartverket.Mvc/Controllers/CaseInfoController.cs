using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Gruppe6_Kartverket.Mvc.Data;
using Gruppe6_Kartverket.Mvc.Models.ViewModels;

namespace Gruppe6_Kartverket.Mvc.Controllers
{
    public class CaseInfoController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor: Initializes the controller with the ApplicationDbContext.
        public CaseInfoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Fetches and displays detailed information about a specific case record.
        public async Task<IActionResult> CaseInfo(int caseRecordId)
        {
            // Retrieves the case record along with related location and user data.
            var caseRecord = await _context.CaseRecords
                .Include(cr => cr.CaseLocation) // Includes associated case location details.
                .Include(cr => cr.User) // Includes associated user details.
                .FirstOrDefaultAsync(cr => cr.CaseRecordId == caseRecordId); // Finds the case by its ID.

            // Returns a 404 if the case record is not found.
            if (caseRecord == null)
            {
                return NotFound();
            }

            // Creates a view model to present the case record details to the view.
            var viewmodel = new CaseDetailsViewModel
            {
                CaseRecordId = caseRecord.CaseRecordId,
                CaseDate = caseRecord.CaseDate,
                CaseTitle = caseRecord.CaseTitle,
                CaseIssueType = caseRecord.CaseIssueType,
                CaseDescription = caseRecord.CaseDescription,
                CaseStatus = caseRecord.CaseStatus,
                LocationId = caseRecord.LocationId,
                GeoJSON = caseRecord.CaseLocation?.GeoJSON, // Optionally include the geoJSON data from the location.
                CaseLocation = caseRecord.CaseLocation,
                User = caseRecord.User
            };

            // Passes the view model to the view to display the case details.
            return View(viewmodel);
        }
    }
}
