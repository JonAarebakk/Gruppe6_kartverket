using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // Add this namespace
using Gruppe6_Kartverket.Mvc.Models;
using Gruppe6_Kartverket.Mvc.Models.Database;
using Gruppe6_Kartverket.Mvc.Data;

namespace Gruppe6_Kartverket.Mvc.Controllers
{
    public class LogInController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _dbContext;

        public LogInController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("LandingPage", "LandingPage");
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LogInModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the user by email in the AspNetUsers table
                var identityUser = await _userManager.FindByEmailAsync(model.Email);
                if (identityUser != null)
                {
                    // Find the user by UserId in the Users table
                    var user = await _dbContext.Users
                        .FirstOrDefaultAsync(u => u.UserId == Guid.Parse(identityUser.Id) && u.UserPassword == model.Password);

                    if (user != null)
                    {
                        // Sign in the user
                        var result = await _signInManager.PasswordSignInAsync(
                            userName: identityUser.UserName,
                            password: model.Password,
                            isPersistent: false,
                            lockoutOnFailure: false);

                        if (result.Succeeded)
                        {
                            return RedirectToAction("LandingPage", "LandingPage");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid password.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationFormModel model)
        {
            // Validate email
            if (string.IsNullOrEmpty(model.Email))
            {
                ModelState.AddModelError("Email", "Email is required.");
            }
            else if (!new EmailAddressAttribute().IsValid(model.Email))
            {
                ModelState.AddModelError("Email", "Invalid email format.");
            }

            // Check if the model state is valid after all validations
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check if user already exists
            var existingUser = await _dbContext.UserInfos
                .Include(ui => ui.User)
                .FirstOrDefaultAsync(ui => ui.Email == model.Email || ui.User.UserName == model.Username);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "User with this email or username already exists.");
                return View(model);
            }

            // Create Identity user
            var identityUser = new IdentityUser
            {
                UserName = model.Username,
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                NormalizedUserName = model.Username.ToUpper()
            };

            var result = await _userManager.CreateAsync(identityUser, model.Password);

            if (result.Succeeded)
            {
                // Create UserInfo entry
                var userInfo = new UserInfo
                {
                    UserId = Guid.Parse(identityUser.Id),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Gender = Enum.Parse<Gender>(model.Gender),
                    RegistrationDate = DateTime.UtcNow,
                    UserStatus = UserStatus.Active,
                    Email = model.Email
                };
                // Create Users entry
                var users = new User
                {
                    UserName = model.Username,
                    UserPassword = model.Password,
                    UserId = Guid.Parse(identityUser.Id),
                    UserType = "Us"
                };
                _dbContext.Users.Add(users);
                _dbContext.UserInfos.Add(userInfo);
                await _dbContext.SaveChangesAsync();

                // Sign in the user
                await _signInManager.SignInAsync(identityUser, isPersistent: false);
                return RedirectToAction("LandingPage", "LandingPage");
            }

            // Add any errors from the Identity user creation
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
    }
}
