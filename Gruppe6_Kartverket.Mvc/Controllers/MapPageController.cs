using Gruppe6_Kartverket.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace Gruppe6_Kartverket.Mvc.Controllers;

public class MapPageController : Controller
{
    // GET
    public IActionResult MapPage()
    {
        return View();
    }
}

