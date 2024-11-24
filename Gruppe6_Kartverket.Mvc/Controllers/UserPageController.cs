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
    /// Controller for handling user page related actions.
    [Authorize]
    public class UserPageController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        /// Initializes a new instance of the <see cref="UserPageController"/> class.
        /// <param name="userManager">The user manager to handle user operations.</param>
        /// <param name="dbContext">The database context to interact with the database.</param>
        public UserPageController(UserManager<IdentityUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Displays the user page with case records.
        /// </summary>
        /// <returns>The user page view with the user's case records.</returns>
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

        /// <summary>
        /// Updates the user's information.
        /// </summary>
        /// <param name="model">The view model containing the user's updated information.</param>
        /// <returns>Redirects to the user page if successful, otherwise returns the view with validation errors.</returns>



        [HttpPost]
        public async Task<IActionResult> UpdateUserInfo(UserPageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("LogIn", "Account");
                }

                // Update user information
                user.UserName = model.User.UserName;
                user.Email = model.UserInfo.Email;

                // Update other fields as necessary
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    // Update additional user info in the database if necessary
                    var userInfo =
                        await _dbContext.UserInfos.FirstOrDefaultAsync(ui => ui.UserId == Guid.Parse(user.Id));
                    if (userInfo != null)
                    {
                        userInfo.FirstName = model.UserInfo.FirstName;
                        userInfo.LastName = model.UserInfo.LastName;
                        userInfo.PhoneNumber = model.UserInfo.PhoneNumber;
                        userInfo.Gender = model.UserInfo.Gender;
                        _dbContext.Update(userInfo);
                        await _dbContext.SaveChangesAsync();
                    }

                    return RedirectToAction("UserPage");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View("UserPage", model);
        }
    }
} 