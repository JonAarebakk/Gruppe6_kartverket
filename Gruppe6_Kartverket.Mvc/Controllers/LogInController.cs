using Microsoft.AspNetCore.Mvc;
using Gruppe6_Kartverket.Mvc.Models;
using System.Diagnostics;

namespace Gruppe6_Kartverket.Mvc.Controllers;

public class LogInController : Controller
{
    [HttpGet]
    public IActionResult LogIn()
    {
        return View();
    }

    [HttpGet]
    public IActionResult RegistrationForm()
    {
        return View();
    }
    
    [HttpPost]
    public ViewResult RegistrationForm(RegistrationFormModel userData)
    {
        return View("Overview", userData);
    }
    

    //[HttpPost]
    //public IActionResult RegistrationForm(UserInfo userInfo)
    //{
    //    if (!ModelState.IsValid)
    //        return View(userInfo);
    //    return RedirectToAction("Index", "Home");
    //}
}