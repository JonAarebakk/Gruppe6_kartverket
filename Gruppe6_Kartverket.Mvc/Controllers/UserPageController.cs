using Microsoft.AspNetCore.Authorization; // Enables authorization policies.
using Microsoft.AspNetCore.Identity; // Manages identity and authentication.
using Microsoft.AspNetCore.Mvc; // Provides MVC functionality.
using Microsoft.EntityFrameworkCore; // Enables database operations.
using System.Linq; // Provides LINQ functionality.
using System.Threading.Tasks; // Allows async programming.
using Gruppe6_Kartverket.Mvc.Data; // Imports application data context.
using Gruppe6_Kartverket.Mvc.Models.ViewModels; // Imports view models.

namespace Gruppe6_Kartverket.Mvc.Controllers
{
    [Authorize] // Ensures only authorized users can access this controller.
    public class UserPageController : Controller // Manages user-specific pages.
    {
        private readonly UserManager<IdentityUser> _userManager; // Manages user identity.
        private readonly ApplicationDbContext _dbContext; // Interacts with the database.

        // Constructor to inject dependencies.
        public UserPageController(UserManager<IdentityUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        // Displays the user's page with their case records.
        public async Task<IActionResult> UserPage()
        {
            var user = await _userManager.GetUserAsync(User); // Gets the currently logged-in user.
            if (user == null) // If user is not logged in.
            {
                return RedirectToAction("LogIn", "Account"); // Redirect to login.
            }

            var userId = Guid.Parse(user.Id); // Parse the user's ID.
            var caseRecords = await _dbContext.CaseRecords
                .Where(cr => cr.UserId == userId) // Fetch case records for the user.
                .ToListAsync();

            var viewModel = new CaseWorkerPageV2ViewModel // Prepare the view model.
            {
                CaseRecords = caseRecords
            };

            return View(viewModel); // Render the view with the data.
        }
    }
}
