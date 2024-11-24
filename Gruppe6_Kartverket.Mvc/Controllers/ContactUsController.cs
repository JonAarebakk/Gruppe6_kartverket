using Gruppe6_Kartverket.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Gruppe6_Kartverket.Mvc.Controllers
{
    /// <summary>
    /// Displays the contact us form.
    /// </summary>
    /// <returns>Returns the contact us view.</returns>
    public class ContactUsController : Controller
    {
        public ActionResult ContactUs()
        {
            return View();
        }
    }
}