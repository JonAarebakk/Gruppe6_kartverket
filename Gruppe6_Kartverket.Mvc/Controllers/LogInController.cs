using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
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

        public LogInController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LogInModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Create Identity user
            var identityUser = new IdentityUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(identityUser, model.Password);

            if (result.Succeeded)
            {
                // Create UserInfo entry
                var userInfo = new UserInfo
                {
                    UserId = int.Parse(identityUser.Id), // Convert string Id to int
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Gender = Enum.Parse<Gender>(model.Gender),
                    RegistrationDate = DateTime.UtcNow,
                    UserStatus = UserStatus.Active
                };

                _dbContext.UserInfos.Add(userInfo);
                await _dbContext.SaveChangesAsync();

                await _signInManager.SignInAsync(identityUser, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult RegistrationSuccess()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegistrationForm(RegistrationFormModel userData)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", userData);
            }

            // Process the registration form data (e.g., save to database)

            return RedirectToAction("RegistrationSuccess");
        }
    }
}