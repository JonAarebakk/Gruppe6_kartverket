namespace Gruppe6_Kartverket.Mvc.Models
{
    public class CaseRecord
    {
        public int CaseId { get; set; }
        public DateTime CaseDate { get; set; }
        public string CaseDescription { get; set; }
        public string CaseStatus { get; set; }
        public int LocationId { get; set; }
        public int UserId { get; set; }

        // Navigation property to CaseLocation
        public virtual CaseLocation CaseLocation { get; set; }

        // Navigation property to User
        public virtual User User { get; set; }
    }
}