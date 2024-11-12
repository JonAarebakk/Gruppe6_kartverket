using Gruppe6_Kartverket.Mvc.Models;
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

            public DataBaseController(ILogger<DataBaseController> logger, ApplicationDbContext context)
            {
                _logger = logger;
                _context = context;
            }

            [HttpPost]
            public IActionResult RegisterUserInfo(UserInfo userInfo)
            {
                try
                {
                    if (userInfo == null)
                    {
                        return BadRequest("Invalid user information.");
                    }

                    _context.UserInfos.Add(userInfo);
                    _context.SaveChanges();
                    return RedirectToAction("UserInfoOverview");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}, Inner Exception: {ex.InnerException?.Message}");
                    throw;
                }
            }

            [HttpGet]
            public IActionResult UserInfoOverview()
            {
                var userInfos = _context.UserInfos.ToList();
                return View(userInfos);
            }


            // Handles the registration of a new geographical area change by storing the geoJson and description in the database

            [HttpPost]
            public IActionResult RegisterAreaChange(string geoJson, string description)
            {
                try
                {
                    if (string.IsNullOrEmpty(geoJson) || string.IsNullOrEmpty(description))
                    {
                        return BadRequest("Invalid data.");
                    }

                    var newGeoChange = new GeoChange
                    {
                        GeoJson = geoJson,
                        Description = description
                    };
                    _context.GeoChanges.Add(newGeoChange);
                    _context.SaveChanges();
                    return RedirectToAction("AreaChangeOverview");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}, Inner Exception: {ex.InnerException?.Message}");
                    throw;
                }

            }



            [HttpGet]
            public IActionResult AreaChangeOverview()
            {
                var geoChanges = _context.GeoChanges.ToList();
                return View(geoChanges);
            }


            [HttpPost]
            public IActionResult RegisterUser(User user)
            {
                try
                {
                    if (user == null)
                    {
                        return BadRequest("Invalid user.");
                    }

                    _context.Users.Add(user);
                    _context.SaveChanges();
                    return RedirectToAction("UserOverview");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}, Inner Exception: {ex.InnerException?.Message}");
                    throw;
                }
            }

            [HttpGet]
            public IActionResult UserOverview()
            {
                var users = _context.Users.ToList();
                return View(users);
            }

            [HttpPost]
            public IActionResult RegisterUserType(UserTypes userType)
            {
                try
                {
                    if (userType == null)
                    {
                        return BadRequest("Invalid user type.");
                    }

                    _context.UserTypes.Add(userType);
                    _context.SaveChanges();
                    return RedirectToAction("UserTypeOverview");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}, Inner Exception: {ex.InnerException?.Message}");
                    throw;
                }
            }

            [HttpGet]
            public IActionResult UserTypeOverview()
            {
                var userTypes = _context.UserTypes.ToList();
                return View(userTypes);
            }

            [HttpPost]
            public IActionResult RegisterCaseLocation(CaseLocation caseLocation)
            {
                try
                {
                    if (caseLocation == null)
                    {
                        return BadRequest("Invalid case location.");
                    }

                    _context.CaseLocations.Add(caseLocation);
                    _context.SaveChanges();
                    return RedirectToAction("CaseLocationOverview");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}, Inner Exception: {ex.InnerException?.Message}");
                    throw;
                }
            }

            [HttpGet]
            public IActionResult CaseLocationOverview()
            {
                var caseLocations = _context.CaseLocations.ToList();
                return View(caseLocations);
            }

            [HttpPost]
            public IActionResult RegisterCase(CaseRecord caseRecord)
            {
                try
                {
                    if (caseRecord == null)
                    {
                        return BadRequest("Invalid case.");
                    }

                    _context.CaseRecords.Add(caseRecord);
                    _context.SaveChanges();
                    return RedirectToAction("CaseOverview");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}, Inner Exception: {ex.InnerException?.Message}");
                    throw;
                }
            }

            [HttpGet]
            public IActionResult CaseOverview()
            {
                var cases = _context.CaseRecords.ToList();
                return View(cases);
            }
        }
    }



