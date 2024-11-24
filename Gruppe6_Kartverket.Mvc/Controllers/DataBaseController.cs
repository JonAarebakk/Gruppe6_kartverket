using Gruppe6_Kartverket.Mvc.Models.Database;
using Gruppe6_Kartverket.Mvc.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Gruppe6_Kartverket.Mvc.Controllers
{
    public class DataBaseController : Controller
    {
        private readonly ILogger<DataBaseController> _logger;
        private readonly ApplicationDbContext _context;

        // Constructor that injects the logger and ApplicationDbContext for database access
        public DataBaseController(ILogger<DataBaseController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Action method to register user information and save it to the database
        [HttpPost]
        public IActionResult RegisterUserInfo(UserInfo userInfo)
        {
            try
            {
                if (userInfo == null)
                {
                    return BadRequest("Invalid user information.");
                }

                // Adds the new userInfo to the database and saves the changes
                _context.UserInfos.Add(userInfo);
                _context.SaveChanges();
                return RedirectToAction("UserInfoOverview");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}, Inner Exception: {ex.InnerException?.Message}");
                throw; // Rethrows the exception for further handling
            }
        }

        // Action method to fetch and display a list of all user information
        [HttpGet]
        public IActionResult UserInfoOverview()
        {
            var userInfos = _context.UserInfos.ToList(); // Fetches all userInfos from the database
            return View(userInfos); // Passes the list to the view
        }

        // Action method to register a new user and save it to the database
        [HttpPost]
        public IActionResult RegisterUser(User user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("Invalid user.");
                }

                // Adds the new user to the database and saves the changes
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("UserOverview");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}, Inner Exception: {ex.InnerException?.Message}");
                throw; // Rethrows the exception for further handling
            }
        }

        // Action method to fetch and display a list of all users
        [HttpGet]
        public IActionResult UserOverview()
        {
            var users = _context.Users.ToList(); // Fetches all users from the database
            return View(users); // Passes the list to the view
        }

        // Action method to register a user type and save it to the database
        [HttpPost]
        public IActionResult RegisterUserType(UserTypes userType)
        {
            try
            {
                if (userType == null)
                {
                    return BadRequest("Invalid user type.");
                }

                // Adds the new userType to the database and saves the changes
                _context.UserTypes.Add(userType);
                _context.SaveChanges();
                return RedirectToAction("UserTypeOverview");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}, Inner Exception: {ex.InnerException?.Message}");
                throw; // Rethrows the exception for further handling
            }
        }

        // Action method to fetch and display a list of all user types
        [HttpGet]
        public IActionResult UserTypeOverview()
        {
            var userTypes = _context.UserTypes.ToList(); // Fetches all userTypes from the database
            return View(userTypes); // Passes the list to the view
        }

        // Action method to register a case location and save it to the database
        [HttpPost]
        public IActionResult RegisterCaseLocation(CaseLocation caseLocation)
        {
            try
            {
                if (caseLocation == null)
                {
                    return BadRequest("Invalid case location.");
                }

                // Adds the new caseLocation to the database and saves the changes
                _context.CaseLocations.Add(caseLocation);
                _context.SaveChanges();
                return RedirectToAction("CaseLocationOverview");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}, Inner Exception: {ex.InnerException?.Message}");
                throw; // Rethrows the exception for further handling
            }
        }

        // Action method to fetch and display a list of all case locations
        [HttpGet]
        public IActionResult CaseLocationOverview()
        {
            var caseLocations = _context.CaseLocations.ToList(); // Fetches all case locations from the database
            return View(caseLocations); // Passes the list to the view
        }

        // Action method to register a case record and save it to the database
        [HttpPost]
        public IActionResult RegisterCase(CaseRecord caseRecord)
        {
            try
            {
                if (caseRecord == null)
                {
                    return BadRequest("Invalid case.");
                }

                // Adds the new caseRecord to the database and saves the changes
                _context.CaseRecords.Add(caseRecord);
                _context.SaveChanges();
                return RedirectToAction("CaseOverview");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}, Inner Exception: {ex.InnerException?.Message}");
                throw; // Rethrows the exception for further handling
            }
        }

        // Action method to fetch and display a list of all case records
        [HttpGet]
        public IActionResult CaseOverview()
        {
            var cases = _context.CaseRecords.ToList(); // Fetches all case records from the database
            return View(cases); // Passes the list to the view
        }
    }
}
