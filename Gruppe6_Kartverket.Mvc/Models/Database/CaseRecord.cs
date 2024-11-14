using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gruppe6_Kartverket.Mvc.Models.Database
{
    public class CaseRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CaseRecordId { get; set; }

        [Required]
        public DateTime CaseDate { get; set; }

        [Required]
        [StringLength(300)]
        public string CaseDescription { get; set; }

        [Required]
        public string CaseStatus { get; set; }

        public int? LocationId { get; set; }


        [ForeignKey("LocationId")]
        public virtual CaseLocation CaseLocation { get; set; }

        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}