namespace Gruppe6_Kartverket.Mvc.Models
{
    public class CaseLocation
    {
        public int LocationId { get; set; }
        public string GeoJSON { get; set; }
        public string Municipality { get; set; }
        public string County { get; set; }

        // Navigation property to list of Cases
        public virtual ICollection<CaseRecord> CaseRecords { get; set; }
    }
}