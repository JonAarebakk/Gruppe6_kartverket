using Gruppe6_Kartverket.Mvc.Models.Database;

namespace Gruppe6_Kartverket.Mvc.Models.ViewModels
{
    public class UserPageViewModel
    {
        public List<CaseRecord> CaseRecords { get; set; }
        public UserInfo UserInfo { get; set; }
        public User User { get; set; }

    }
}