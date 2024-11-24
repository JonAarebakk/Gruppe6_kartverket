using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gruppe6_Kartverket.Mvc.Models.Database;
using Gruppe6_Kartverket.Mvc.Models.ViewModels;
using System.Threading.Tasks;
using System.Linq;
using Gruppe6_Kartverket.Mvc.Data;
using System;
using System.Collections.Generic;

namespace Gruppe6_Kartverket.Mvc.Controllers
{
    public class CaseWorkerController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor that injects the ApplicationDbContext for database access
        public CaseWorkerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Action method to fetch case records and pass them to the view
        public async Task<IActionResult> CaseWorkerPageV2()
        {
            try
            {
                // Fetches all case records along with their associated case location and user
                var caseRecords = await _context.CaseRecords
                    .Include(c => c.CaseLocation) // Includes case location data
                    .Include(c => c.User) // Includes user data
                    .ToListAsync(); // Executes the query asynchronously

                // Logs a message if no case records are found
                if (caseRecords == null || !caseRecords.Any())
                {
                    Console.WriteLine("No case records found.");
                }

                // Creates a ViewModel with the case records to pass to the view
                var viewModel = new CaseWorkerPageV2ViewModel
                {
                    CaseRecords = caseRecords,
                };

                return View(viewModel); // Returns the view with the populated ViewModel
            }
            catch (Exception ex)
            {
                // Logs the exception (e.g., to console or file) if an error occurs
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error. Please try again later."); // Returns a 500 status code on error
            }
        }

        // Action method to fetch the details of a specific case record
        public IActionResult CaseDetails(int caseRecordId)
        {
            // Retrieves a specific case record by its ID, along with associated location and user
            var caseRecord = _context.CaseRecords
                .Include(c => c.CaseLocation) // Includes case location data
                .Include(c => c.User) // Includes user data
                .FirstOrDefault(c => c.CaseRecordId == caseRecordId); // Finds the case record by its ID

            // Returns a 404 if the case record was not found
            if (caseRecord == null)
            {
                return NotFound();
            }

            // Creates a ViewModel to pass detailed case information to the view
            var viewModel = new CaseDetailsViewModel
            {
                CaseRecordId = caseRecord.CaseRecordId,
                CaseDate = caseRecord.CaseDate,
                CaseTitle = caseRecord.CaseTitle,
                CaseIssueType = caseRecord.CaseIssueType,
                CaseDescription = caseRecord.CaseDescription,
                CaseStatus = caseRecord.CaseStatus,
                LocationId = caseRecord.LocationId,
                GeoJSON = caseRecord.CaseLocation.GeoJSON, // Includes the GeoJSON data from the location
                CaseLocation = caseRecord.CaseLocation, // Includes location data
                User = caseRecord.User // Includes user data
            };

            return View(viewModel); // Returns the view with the populated ViewModel
        }
        [HttpPost]
        public IActionResult UpdateStatus(int caseRecordId, string caseStatus)
        {
            var caseRecord = _context.CaseRecords.Find(caseRecordId);
            if (caseRecord != null)
            {
                caseRecord.CaseStatus = caseStatus;
                _context.SaveChanges();
            }

            return RedirectToAction("CaseDetails", new { caseRecordId = caseRecordId });        }
        
    }
}
