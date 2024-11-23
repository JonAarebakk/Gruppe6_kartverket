using Gruppe6_Kartverket.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Text.Json;

namespace Gruppe6_Kartverket.Mvc.Controllers;

public class MapPageController : Controller
{
    // GET 
    public IActionResult MapPage()
    {
        ViewBag.HideFooter = true;
        ViewBag.LoggedIn = true;

        return View();
    }

    [HttpPost]
    public IActionResult MapPage(CaseRegistrationModel model)
    {
        ViewBag.HideFooter = true;

        if (!ModelState.IsValid)
        {
            Console.WriteLine(model.CaseTitle);
            Console.WriteLine(model.Kategori);
            Console.WriteLine(model.Beskrivelse);
            Console.WriteLine(model.GeoJson);
            Console.WriteLine(model.CaseTitle);
            return View("MapPage", model);
        } else {
            ViewBag.ShowSubmittedPopup = true;
            ModelState.Clear();

            //TODO: Save to database
            //Make the model into a CaseRecord object, and send to database

            return View("MapPage");
        }
    }
}

