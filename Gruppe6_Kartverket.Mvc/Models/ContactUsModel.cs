using System.Collections.Generic;

namespace Gruppe6_Kartverket.Mvc.Models
{
    public class ContactUsModel
    {
        public string Phone { get; set; }
        public string Email { get; set; }
        public string OrganizationNumber { get; set; }
        public string Address { get; set; }
        public Dictionary<string, string> Departments { get; set; } = new Dictionary<string, string>();
    }
}
