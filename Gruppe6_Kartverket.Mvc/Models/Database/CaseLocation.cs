using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gruppe6_Kartverket.Mvc.Models.Database
{
    public class CaseLocation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LocationId { get; set; }

        [Required]
        public string GeoJSON { get; set; }

        [Required]
        [StringLength(40)]
        public string Municipality { get; set; }

        [Required]
        [StringLength(40)]
        public string County { get; set; }

        // Navigation property to list of Cases
        public virtual ICollection<CaseRecord> CaseRecords { get; set; }
    }
}