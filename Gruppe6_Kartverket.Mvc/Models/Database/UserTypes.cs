
using System.ComponentModel.DataAnnotations;

namespace Gruppe6_Kartverket.Mvc.Models.Database
{
    public class UserTypes
    {
        [Key]
        [StringLength(2)]
        public string UserType { get; set; }

        [Required]
        [StringLength(20)]
        public string UserTypeDescription { get; set; }



        // Navigation property to list of Users
        public virtual ICollection<User> Users { get; set; }
    }


    public enum UserTypeDescription
    {
        CaseWorker,
        CommonUser,
        Prioritised,
        Administrator
    }
}