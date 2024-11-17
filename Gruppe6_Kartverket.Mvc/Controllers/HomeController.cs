using Gruppe6_Kartverket.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Gruppe6_Kartverket.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<IdentityUser> _userManager;

    // private static List<MapData> mapDataList = new List<MapData>();

    public HomeController(
        ILogger<HomeController> logger,
        UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    
   
    public IActionResult Index()
    {
        var model = new HomeViewModel();
        model.Message = "It's scuffed, but it works";
        return View("landingpage", model);
    }
    // Var no rart med mapdata
/*
    [HttpPost]
    public IActionResult Index(HomeViewModel model)
    {
        if (!ModelState.IsValid)
            return View("Index", model);

        if (model.Hidden != null)
        {
            var mapData = JsonSerializer.Deserialize<MapData>(model.Hidden, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            mapDataList.Add(mapData);
        }
        model.Message = model.NewMessage;
        model.NewMessage = null;
        return View("Index", model);
    }
*/
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