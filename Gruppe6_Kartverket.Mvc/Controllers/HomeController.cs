using Gruppe6_Kartverket.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace Gruppe6_Kartverket.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var model = new HomeViewModel();
        model.Message = "It's scuffed, but it works";
        return View("Index", model);
    }

    [HttpPost]
    public IActionResult Index(HomeViewModel model)
    {
        if (!ModelState.IsValid)
            return View("Index", model);

        if (model.Hidden != null)
        {
            var mapData = JsonSerializer.Deserialize<MapData>(model.Hidden, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        model.Message = model.NewMessage;
        model.NewMessage = null;
        return View("Index", model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

public class MapData
{
    public List<LatLng> Point { get; set; }
    public List<List<LatLng>> LineString { get; set; }
    public List<List<List<LatLng>>> Polygon { get; set; }
}

public class LatLng
{
    public double Lat { get; set; }
    public double Lng { get; set; }
}