using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Gruppe6_Kartverket.Mvc.Models;
using Gruppe6_Kartverket.Mvc.Models.Database;
using Gruppe6_Kartverket.Mvc.Data;

namespace Gruppe6_Kartverket.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _dbContext;

        // Constructor: Initializes dependencies for UserManager, SignInManager, and ApplicationDbContext.
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
        }

        // Logs the user out and redirects to the landing page.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("LandingPage", "LandingPage");
        }

        // Displays the login page.
        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        // Validates the login credentials and signs the user in if valid.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LogInModel model)
        {
            if (ModelState.IsValid)
            {
                var identityUser = await _userManager.FindByEmailAsync(model.Email);
                if (identityUser != null)
                {
                    var user = await _dbContext.Users
                        .FirstOrDefaultAsync(u => u.UserId == Guid.Parse(identityUser.Id) && u.UserPassword == model.Password);

                    if (user != null)
                    {
                        var result = await _signInManager.PasswordSignInAsync(
                            userName: identityUser.UserName,
                            password: model.Password,
                            isPersistent: false,
                            lockoutOnFailure: false);

                        if (result.Succeeded)
                        {
                            var roles = await _userManager.GetRolesAsync(identityUser);
                            if (user.UserType == "Ad" || user.UserType == "Us")
                            {
                                return RedirectToAction("LandingPage", "LandingPage");
                            }
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

        // Displays the registration page.
        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }

        // Handles the registration form submission, validates input, and creates the user.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationFormModel model)
        {
            // Validates email format and presence.
            if (string.IsNullOrEmpty(model.Email))
            {
                ModelState.AddModelError("Email", "Email is required.");
            }
            else if (!new EmailAddressAttribute().IsValid(model.Email))
            {
                ModelState.AddModelError("Email", "Invalid email format.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Checks if a user with the same email or username exists.
            var existingUser = await _dbContext.UserInfos
                .Include(ui => ui.User)
                .FirstOrDefaultAsync(ui => ui.Email == model.Email || ui.User.UserName == model.Username);

            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "User with this email or username already exists.");
                return View(model);
            }

            // Creates a new IdentityUser for authentication.
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
                // Creates additional user information for the database.
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

                // Creates user record with user type and password.
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

                await _signInManager.SignInAsync(identityUser, isPersistent: false);
                return RedirectToAction("LandingPage", "LandingPage");
            }

            // Adds errors if user creation fails.
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
    }
}
