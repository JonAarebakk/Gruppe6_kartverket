using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gruppe6_Kartverket.Mvc.Models.ViewModels;
using Gruppe6_Kartverket.Mvc.Data; 
using System.Security.Claims;

namespace Gruppe6_Kartverket.Mvc.Controllers;

public class UserPageController : Controller
{
    private readonly ApplicationDbContext _context;

    // Constructor that injects the ApplicationDbContext for database access
    public UserPageController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET
    public async Task<IActionResult> UserPage()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
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
                CaseRecords = caseRecords
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

    public IActionResult Settings()
    {
        // Logic to load settings
        return View("Settings");
    }

    public IActionResult Profile()
    {
        // Logic to load user profile
        return View("Profile");
    }

    public IActionResult Logout()
    {
        // Logic to handle logout
        return RedirectToAction("Index", "Home");
    }

    /*private List<CaseRecord> GetCases()
    {
        return new List<CaseRecord>
        {
            
            new CaseRecord { UserId = "3", CaseTitle = "Case 1", CaseDate = DateTime.Now, CaseStatus = "Open" },
            new CaseRecord { UserId = "8", CaseTitle = "Case 2", CaseDate = DateTime.Now, CaseStatus = "Closed" } 
        };
    }*/
}
