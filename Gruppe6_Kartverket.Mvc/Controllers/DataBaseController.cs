using Gruppe6_Kartverket.Mvc.Models.Database; // Imports database models.
using Gruppe6_Kartverket.Mvc.Data; // Imports database context.
using Microsoft.AspNetCore.Mvc; // Enables MVC features like controllers and views.
using System.Diagnostics; // Provides diagnostics for debugging.
using System.Text.Json; // Supports JSON serialization.
using Microsoft.EntityFrameworkCore; // Provides ORM for database access.

namespace Gruppe6_Kartverket.Mvc.Controllers // Namespace for the controller.
{
    public class DataBaseController : Controller // Manages database-related actions.
    {
        private readonly ILogger<DataBaseController> _logger; // Logs application activities.
        private readonly ApplicationDbContext _context; // Represents the database context.

        public DataBaseController(ILogger<DataBaseController> logger, ApplicationDbContext context) // Constructor.
        {
            _logger = logger; // Injects the logger.
            _context = context; // Injects the database context.
        }

        [HttpPost]
        public IActionResult RegisterUserInfo(UserInfo userInfo) // Registers new user information.
        {
            try
            {
                if (userInfo == null) // Validates the input.
                {
                    return BadRequest("Invalid user information."); // Returns bad request if invalid.
                }

                _context.UserInfos.Add(userInfo); // Adds user information to the database.
                _context.SaveChanges(); // Saves changes to the database.
                return RedirectToAction("UserInfoOverview"); // Redirects to the overview page.
            }
            catch (Exception ex) // Catches errors.
            {
                Console.WriteLine($"Error: {ex.Message}, Inner Exception: {ex.InnerException?.Message}"); // Logs error details.
                throw; // Rethrows the exception.
            }
        }

        [HttpGet]
        public IActionResult UserInfoOverview() // Displays all user information.
        {
            var userInfos = _context.UserInfos.ToList(); // Retrieves all user information from the database.
            return View(userInfos); // Passes data to the view.
        }

        [HttpPost]
        public IActionResult RegisterUser(User user) // Registers a new user.
        {
            try
            {
                if (user == null) // Validates the input.
                {
                    return BadRequest("Invalid user."); // Returns bad request if invalid.
                }

                _context.Users.Add(user); // Adds the user to the database.
                _context.SaveChanges(); // Saves changes to the database.
                return RedirectToAction("UserOverview"); // Redirects to the overview page.
            }
            catch (Exception ex) // Catches errors.
            {
                Console.WriteLine($"Error: {ex.Message}, Inner Exception: {ex.InnerException?.Message}"); // Logs error details.
                throw; // Rethrows the exception.
            }
        }

        [HttpGet]
        public IActionResult UserOverview() // Displays all users.
        {
            var users = _context.Users.ToList(); // Retrieves all users from the database.
            return View(users); // Passes data to the view.
        }

        [HttpPost]
        public IActionResult RegisterUserType(UserTypes userType) // Registers a new user type.
        {
            try
            {
                if (userType == null) // Validates the input.
                {
                    return BadRequest("Invalid user type."); // Returns bad request if invalid.
                }

                _context.UserTypes.Add(userType); // Adds the user type to the database.
                _context.SaveChanges(); // Saves changes to the database.
                return RedirectToAction("UserTypeOverview"); // Redirects to the overview page.
            }
            catch (Exception ex) // Catches errors.
            {
                Console.WriteLine($"Error: {ex.Message}, Inner Exception: {ex.InnerException?.Message}"); // Logs error details.
                throw; // Rethrows the exception.
            }
        }

        [HttpGet]
        public IActionResult UserTypeOverview() // Displays all user types.
        {
            var userTypes = _context.UserTypes.ToList(); // Retrieves all user types from the database.
            return View(userTypes); // Passes data to the view.
        }

        [HttpPost]
        public IActionResult RegisterCaseLocation(CaseLocation caseLocation) // Registers a new case location.
        {
            try
            {
                if (caseLocation == null) // Validates the input.
                {
                    return BadRequest("Invalid case location."); // Returns bad request if invalid.
                }

                _context.CaseLocations.Add(caseLocation); // Adds the case location to the database.
                _context.SaveChanges(); // Saves changes to the database.
                return RedirectToAction("CaseLocationOverview"); // Redirects to the overview page.
            }
            catch (Exception ex) // Catches errors.
            {
                Console.WriteLine($"Error: {ex.Message}, Inner Exception: {ex.InnerException?.Message}"); // Logs error details.
                throw; // Rethrows the exception.
            }
        }

        [HttpGet]
        public IActionResult CaseLocationOverview() // Displays all case locations.
        {
            var caseLocations = _context.CaseLocations.ToList(); // Retrieves all case locations from the database.
            return View(caseLocations); // Passes data to the view.
        }

        [HttpPost]
        public IActionResult RegisterCase(CaseRecord caseRecord) // Registers a new case.
        {
            try
            {
                if (caseRecord == null) // Validates the input.
                {
                    return BadRequest("Invalid case."); // Returns bad request if invalid.
                }

                _context.CaseRecords.Add(caseRecord); // Adds the case to the database.
                _context.SaveChanges(); // Saves changes to the database.
                return RedirectToAction("CaseOverview"); // Redirects to the overview page.
            }
            catch (Exception ex) // Catches errors.
            {
                Console.WriteLine($"Error: {ex.Message}, Inner Exception: {ex.InnerException?.Message}"); // Logs error details.
                throw; // Rethrows the exception.
            }
        }

        [HttpGet]
        public IActionResult CaseOverview() // Displays all cases.
        {
            var cases = _context.CaseRecords.ToList(); // Retrieves all cases from the database.
            return View(cases); // Passes data to the view.
        }
    }
}
