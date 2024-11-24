using Gruppe6_Kartverket.Mvc.Models.Database;
namespace Gruppe6_Kartverket.Mvc.Models.ViewModels
{
    public class CaseDetailsViewModel
    {
        public int CaseRecordId { get; set; }
        public DateTime CaseDate { get; set; }
        public string CaseTitle { get; set; }
        public string CaseIssueType { get; set; }
        public string CaseDescription { get; set; }
        public string CaseStatus { get; set; }
        public int? LocationId { get; set; }
        public CaseLocation CaseLocation { get; set; }
        public User User { get; set; }
    }
}