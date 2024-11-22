using System.ComponentModel.DataAnnotations;

namespace Gruppe6_Kartverket.Mvc.Models

{
    public class LogInModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}