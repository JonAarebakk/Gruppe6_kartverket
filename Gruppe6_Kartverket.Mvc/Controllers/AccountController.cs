using System.ComponentModel.DataAnnotations; // Provides attributes for data validation.
using Microsoft.AspNetCore.Mvc; // Enables MVC features like controllers and views.
using Microsoft.AspNetCore.Identity; // Handles identity and authentication.
using System.Threading.Tasks; // Supports asynchronous programming.
using Microsoft.EntityFrameworkCore; // Provides ORM for database access.
using Gruppe6_Kartverket.Mvc.Models; // Imports custom models.
using Gruppe6_Kartverket.Mvc.Models.Database; // Imports database models.
using Gruppe6_Kartverket.Mvc.Data; // Imports database context.

namespace Gruppe6_Kartverket.Mvc.Controllers // Namespace for the controller.
{
    public class AccountController : Controller // Handles account-related actions.
    {
        private readonly UserManager<IdentityUser> _userManager; // Manages user operations.
        private readonly SignInManager<IdentityUser> _signInManager; // Handles sign-in operations.
        private readonly ApplicationDbContext _dbContext; // Represents the database context.

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            ApplicationDbContext dbContext)
        {
            _userManager = userManager; // Inject UserManager dependency.
            _signInManager = signInManager; // Inject SignInManager dependency.
            _dbContext = dbContext; // Inject ApplicationDbContext dependency.
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Protects against CSRF attacks.
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync(); // Signs the user out.
            return RedirectToAction("LandingPage", "LandingPage"); // Redirects to the landing page.
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View(); // Displays the login page.
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Protects against CSRF attacks.
        public async Task<IActionResult> LogIn(LogInModel model)
        {
            if (ModelState.IsValid) // Checks if the form data is valid.
            {
                var identityUser = await _userManager.FindByEmailAsync(model.Email); // Finds user by email.
                if (identityUser != null)
                {
                    // Verifies user credentials in the database.
                    var user = await _dbContext.Users
                        .FirstOrDefaultAsync(u => u.UserId == Guid.Parse(identityUser.Id) && u.UserPassword == model.Password);

                    if (user != null) // If user is found and password matches.
                    {
                        var result = await _signInManager.PasswordSignInAsync(
                            userName: identityUser.UserName,
                            password: model.Password,
                            isPersistent: false,
                            lockoutOnFailure: false);

                        if (result.Succeeded) // If sign-in is successful.
                        {
                            var roles = await _userManager.GetRolesAsync(identityUser); // Fetches user roles.
                            if (user.UserType == "Ad") // Redirects based on user type.
                            {
                                return RedirectToAction("LandingPage", "LandingPage");
                            }
                            else if (user.UserType == "Us")
                            {
                                return RedirectToAction("LandingPage", "LandingPage");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid login attempt."); // Error for failed login.
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid password."); // Error for wrong password.
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found."); // Error for missing user.
                }
            }

            return View(model); // Returns to the login page with errors.
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View("Register"); // Displays the registration page.
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Protects against CSRF attacks.
        public async Task<IActionResult> Register(RegistrationFormModel model)
        {
            if (string.IsNullOrEmpty(model.Email)) // Validates email presence.
            {
                ModelState.AddModelError("Email", "Email is required.");
            }
            else if (!new EmailAddressAttribute().IsValid(model.Email)) // Validates email format.
            {
                ModelState.AddModelError("Email", "Invalid email format.");
            }

            if (!ModelState.IsValid) // Returns errors if form is invalid.
            {
                return View(model);
            }

            // Checks for existing user by email or username.
            var existingUser = await _dbContext.UserInfos
                .Include(ui => ui.User)
                .FirstOrDefaultAsync(ui => ui.Email == model.Email || ui.User.UserName == model.Username);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "User with this email or username already exists.");
                return View(model);
            }

            var identityUser = new IdentityUser // Creates a new identity user.
            {
                UserName = model.Username,
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                NormalizedUserName = model.Username.ToUpper()
            };

            var result = await _userManager.CreateAsync(identityUser, model.Password); // Adds user to Identity.

            if (result.Succeeded) // If user creation succeeds.
            {
                var userInfo = new UserInfo // Creates UserInfo record.
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

                var users = new User // Creates User record.
                {
                    UserName = model.Username,
                    UserPassword = model.Password,
                    UserId = Guid.Parse(identityUser.Id),
                    UserType = model.UserType
                };
                _dbContext.Users.Add(users); // Adds User to the database.
                _dbContext.UserInfos.Add(userInfo); // Adds UserInfo to the database.
                await _dbContext.SaveChangesAsync(); // Saves changes to the database.

                await _signInManager.SignInAsync(identityUser, isPersistent: false); // Signs in the new user.
                return RedirectToAction("LandingPage", "LandingPage"); // Redirects to the landing page.
            }

            foreach (var error in result.Errors) // Adds any creation errors to the model state.
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model); // Returns to registration page with errors.
        }
    }
}
