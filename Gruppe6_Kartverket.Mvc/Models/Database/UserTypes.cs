namespace Gruppe6_Kartverket.Mvc.Models
{
    public class UserTypes
    {
        public string UserType { get; set; }
        public string UserTypeDescription { get; set; }



        // Navigation property to list of Users
        public virtual ICollection<User> Users { get; set; }
    }
}