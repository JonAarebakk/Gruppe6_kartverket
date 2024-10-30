namespace Gruppe6_Kartverket.Mvc.Models
{
    public class RegistrationFormModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateOnly RegistrationDate { get; set; }
        public Boolean UserStatus { get; set; }

    }
}
