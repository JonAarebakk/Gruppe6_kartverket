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
        public async Task<IActionResult> CaseWorkerPageV2(string filter)
        {
            var caseRecords = _context.CaseRecords.AsQueryable();

            switch (filter)
            {
                case "asc":
                    caseRecords = caseRecords.OrderBy(c => c.CaseDate);
                    break;
                case "desc":
                    caseRecords = caseRecords.OrderByDescending(c => c.CaseDate);
                    break;
                case "open":
                    caseRecords = caseRecords.Where(c => c.CaseStatus == CaseStatus.Open.ToString());
                    break;
                case "closed":
                    caseRecords = caseRecords.Where(c => c.CaseStatus == CaseStatus.Closed.ToString());
                    break;
                case "inprogress":
                    caseRecords = caseRecords.Where(c => c.CaseStatus == CaseStatus.InProgress.ToString());
                    break;
                case "resolved":
                    caseRecords = caseRecords.Where(c => c.CaseStatus == CaseStatus.Resolved.ToString());
                    break;
            }

            var viewModel = new CaseWorkerPageV2ViewModel
            {
                CaseRecords = await caseRecords
                    .Include(c => c.CaseLocation)
                    .Include(c => c.User)
                    .ToListAsync()
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CaseRecordsTable", viewModel);
            }

            return View(viewModel);
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
                Municipality = caseRecord.CaseLocation.Municipality,
                County = caseRecord.CaseLocation.County,
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

            return RedirectToAction("CaseDetails", new { caseRecordId = caseRecordId });
        }
    }
}