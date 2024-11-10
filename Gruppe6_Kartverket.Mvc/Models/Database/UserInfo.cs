namespace Gruppe6_Kartverket.Mvc.Models
{
    public enum Gender
    {
        M, F, O
    }

    public enum UserStatus
    {
        Active, Inactive
    }


    public class UserInfo

    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Gender? Gender { get; set; }
        public UserStatus? UserStatus { get; set; }


        //Navigation property to User
        public virtual User User { get; set; }
    }
}