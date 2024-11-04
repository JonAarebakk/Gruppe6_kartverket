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
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _context;


        public DataBaseController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

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




    }
}