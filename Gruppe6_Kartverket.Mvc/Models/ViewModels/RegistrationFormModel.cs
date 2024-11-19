using System;
using System.ComponentModel.DataAnnotations;

namespace Gruppe6_Kartverket.Mvc.Models
{
    public class RegistrationFormModel
    {
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(35, ErrorMessage = "First Name must be less than 35 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(35, ErrorMessage = "Last Name must be less than 35 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }

        public string? Gender { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(30, ErrorMessage = "Username cannot be longer than 30 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Provided passwords must be identical")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        /*
        [Required(ErrorMessage = "Type of user is required")]
        [StringLength(20, ErrorMessage = "UserType must be less than 20 characters")]
        public string UserType { get; set; }
*/
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
        public bool UserStatus { get; set; } = true; // Assuming default active status for new registrations
    }
}