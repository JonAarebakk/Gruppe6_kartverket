using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Gruppe6_Kartverket.Mvc.Models.Database
{
 
    public class UserInfo

    {
        [Key]
        [BindNever]
        [ForeignKey("User")]
        public string UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(30)]
        public string FirstName { get; set; }

        [StringLength(30)]
        public string LastName { get; set; }

        [Required]
        [StringLength(15)]
        public string PhoneNumber { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        [Required]
        public UserStatus UserStatus { get; set; }

        // Navigation property to User
        public User User { get; set; }
    }
    public enum Gender
    {
        Man, 
        Woman, 
        Others
    }

    public enum UserStatus
    {
        Active, 
        Inactive,
        Suspended
    }

}