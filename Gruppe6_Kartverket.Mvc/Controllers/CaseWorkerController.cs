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

        // Action method to fetch the data and pass it to the view
        public async Task<IActionResult> CaseWorkerPageV2()
        {
            try
            {
                var caseRecords = await _context.CaseRecords
                    .Include(c => c.CaseLocation)
                    .Include(c => c.User)
                    .ToListAsync();

                if (caseRecords == null || !caseRecords.Any())
                {
                    Console.WriteLine("No case records found.");
                }

                var viewModel = new CaseWorkerPageV2ViewModel
                {
                    CaseRecords = caseRecords,
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Log the exception to the console or a file
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        // Action method to fetch the details of a specific case record
        public IActionResult CaseDetails(int caseRecordId)
        {
            var caseRecord = _context.CaseRecords
                .Include(c => c.CaseLocation)
                .Include(c => c.User)
                .FirstOrDefault(c => c.CaseRecordId == caseRecordId);

            if (caseRecord == null)
            {
                return NotFound();
            }

            var viewModel = new CaseDetailsViewModel
            {
                CaseRecordId = caseRecord.CaseRecordId,
                CaseDate = caseRecord.CaseDate,
                CaseTitle = caseRecord.CaseTitle,
                CaseIssueType = caseRecord.CaseIssueType,
                CaseDescription = caseRecord.CaseDescription,
                CaseStatus = caseRecord.CaseStatus,
                LocationId = caseRecord.LocationId,
                GeoJSON = caseRecord.CaseLocation.GeoJSON,
                CaseLocation = caseRecord.CaseLocation,
                User = caseRecord.User
            };

            return View(viewModel);
        }
    }
}