using Gruppe6_Kartverket.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Gruppe6_Kartverket.Mvc.Controllers
{
    public class ContactUsController : Controller
    {
        public ActionResult ContactUs()
        {
            // Create an example of contact info
            var model = new ContactUsModel
            {
                Phone = "32 11 80 00",
                Email = "post@kartverket.no",
                OrganizationNumber = "971 040 238",
                Address = "Kartverksveien 21, 3511 Hønefoss",
                Departments = new Dictionary<string, string>
                {
                    { "Kundesenteret", "09.00-15.00" },
                    { "Tinglysing", "Kartverket Tinglysing\nPostboks 600 Sentrum\n3507 Hønefoss\nFaks: 32 11 88 01" },
                    { "Sjødivisjonen", "Prof. Olav Hanssens vei 10, 4021 Stavanger" },
                }
            };

            return View(model);
        }
    }
}