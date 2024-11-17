using Gruppe6_Kartverket.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gruppe6_Kartverket.Mvc.Models.Database;
using Gruppe6_Kartverket.Mvc.Data;
using Microsoft.AspNetCore.Identity;

namespace Gruppe6_Kartverket.Mvc.Controllers
{
    public class AccoutController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccoutController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationFormModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Username, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    TempData["Success"] = "User created successfully";
                    return RedirectToAction("RegistrationSuccess");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        public IActionResult RegistrationSuccess()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}
