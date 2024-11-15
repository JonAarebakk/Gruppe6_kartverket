using Gruppe6_Kartverket.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Gruppe6_Kartverket.Mvc.Controllers
{
    public class ContactUsController : Controller
    {
        public ActionResult ContactUs()
        {
            return View();
        }
    }
}