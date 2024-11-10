namespace Gruppe6_Kartverket.Mvc.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserType { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }

        // Navigation property to UserType
        public virtual UserTypes UserTypeNavigation { get; set; }


        // Navigation property to UserInfo
        public virtual UserInfo UserInfo { get; set; }
    }
}