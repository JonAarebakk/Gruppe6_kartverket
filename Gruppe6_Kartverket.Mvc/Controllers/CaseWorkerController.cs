using Microsoft.AspNetCore.Mvc; // Enables MVC features like controllers and views.
using Microsoft.EntityFrameworkCore; // Provides ORM for database access.
using Gruppe6_Kartverket.Mvc.Models.Database; // Imports database models.
using Gruppe6_Kartverket.Mvc.Models.ViewModels; // Imports custom view models.
using System.Threading.Tasks; // Supports asynchronous programming.
using System.Linq; // Provides LINQ functionality.
using Gruppe6_Kartverket.Mvc.Data; // Imports database context.
using System; // Provides base system functionalities.
using System.Collections.Generic; // Enables use of collections.

namespace Gruppe6_Kartverket.Mvc.Controllers // Namespace for the controller.
{
    public class CaseWorkerController : Controller // Manages case worker-related actions.
    {
        private readonly ApplicationDbContext _context; // Represents the database context.

        // Constructor that injects the ApplicationDbContext for database access.
        public CaseWorkerController(ApplicationDbContext context)
        {
            _context = context; // Injects the database context.
        }

        // Action method to fetch all case records and pass them to the view.
        public async Task<IActionResult> CaseWorkerPageV2()
        {
            try
            {
                // Queries the database for case records, including related location and user data.
                var caseRecords = await _context.CaseRecords
                    .Include(c => c.CaseLocation) // Includes location details.
                    .Include(c => c.User) // Includes associated user details.
                    .ToListAsync(); // Executes the query and converts results to a list.

                if (caseRecords == null || !caseRecords.Any()) // Checks if any case records exist.
                {
                    Console.WriteLine("No case records found."); // Logs a message if no records are found.
                }

                // Prepares the view model with the retrieved case records.
                var viewModel = new CaseWorkerPageV2ViewModel
                {
                    CaseRecords = caseRecords, // Assigns case records to the view model.
                };

                return View(viewModel); // Passes the view model to the view.
            }
            catch (Exception ex) // Catches any exceptions that occur.
            {
                Console.WriteLine($"Error: {ex.Message}"); // Logs the error message.
                return StatusCode(500, "Internal server error. Please try again later."); // Returns a server error response.
            }
        }

        // Action method to fetch details of a specific case record.
        public IActionResult CaseDetails(int caseRecordId)
        {
            // Queries the database for a specific case record by ID, including related data.
            var caseRecord = _context.CaseRecords
                .Include(c => c.CaseLocation) // Includes location details.
                .Include(c => c.User) // Includes associated user details.
                .FirstOrDefault(c => c.CaseRecordId == caseRecordId); // Finds the record by ID.

            if (caseRecord == null) // Checks if the case record is found.
            {
                return NotFound(); // Returns a 404 error if not found.
            }

            // Maps the case record data to a view model for display.
            var viewModel = new CaseDetailsViewModel
            {
                CaseRecordId = caseRecord.CaseRecordId, // Case ID.
                CaseDate = caseRecord.CaseDate, // Date of the case.
                CaseTitle = caseRecord.CaseTitle, // Title of the case.
                CaseIssueType = caseRecord.CaseIssueType, // Type of issue.
                CaseDescription = caseRecord.CaseDescription, // Description of the case.
                CaseStatus = caseRecord.CaseStatus, // Status of the case.
                LocationId = caseRecord.LocationId, // Location ID.
                GeoJSON = caseRecord.CaseLocation.GeoJSON, // GeoJSON data for mapping.
                CaseLocation = caseRecord.CaseLocation, // Location details.
                User = caseRecord.User // Associated user details.
            };

            return View(viewModel); // Passes the view model to the view.
        }
    }
}
