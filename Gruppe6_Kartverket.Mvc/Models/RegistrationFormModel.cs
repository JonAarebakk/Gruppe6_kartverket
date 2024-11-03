using System;
using System.ComponentModel.DataAnnotations;

namespace Gruppe6_Kartverket.Mvc.Models;

public class RegistrationFormModel
{
    [Required(ErrorMessage = "First Name is required")]
    [StringLength(35, ErrorMessage = "First Name must be less than 35 characters")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]
    [StringLength(35, ErrorMessage = "Last Name must be less than 35 characters")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "Phone Number is required")]
    [Phone(ErrorMessage = "Invalid Phone Number")]
    public required string PhoneNumber { get; set; }

    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string? Email { get; set; }

    public string? Gender { get; set; }

    [Required(ErrorMessage = "Username is required")]
    [StringLength(30, ErrorMessage = "Username cannot be longer than 30 characters")]
    public required string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
    public required string Password { get; set; }

    [Required(ErrorMessage = "Confirm Password is required")]
    [Compare("Password", ErrorMessage = "Provided passwords must be identical")]
    [DataType(DataType.Password)]
    public required string ConfirmPassword { get; set; }

    public DateOnly RegistrationDate { get; set; }
    public bool UserStatus { get; set; }
}

//[Required]               // Ensures the field is not empty
//[EmailAddress]           // Checks for a valid email format
//[Phone]                  // Checks for a valid phone number format
//[StringLength]           // Limits the number of characters
//[Compare]                // Used for comparing fields (e.g., password confirmation)