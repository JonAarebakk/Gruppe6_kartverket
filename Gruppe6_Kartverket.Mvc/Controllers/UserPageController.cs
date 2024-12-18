using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Gruppe6_Kartverket.Mvc.Data;
using Gruppe6_Kartverket.Mvc.Models.ViewModels;

namespace Gruppe6_Kartverket.Mvc.Controllers
{
    [Authorize]
    public class UserPageController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public UserPageController(UserManager<IdentityUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        // Displays the user's page with a list of their case records.
        // Redirects to login if the user is not authenticated.
        public async Task<IActionResult> UserPage()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("LogIn", "Account");
            }

            var userId = Guid.Parse(user.Id);
            var caseRecords = await _dbContext.CaseRecords
                .Where(cr => cr.UserId == userId)
                .ToListAsync();

            var viewModel = new UserPageViewModel
            {
                CaseRecords = caseRecords
            };

            return View(viewModel);
        }
    }
}