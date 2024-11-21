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
        [StringLength(50)]
        public string CaseTitle { get; set; }

        [Required]
        [StringLength(30)]
        public string CaseIssueType { get; set; }    //IssueType - add Enum of what kind of types there are

        [Required]
       [StringLength(300)]
        public string CaseDescription { get; set; }

       [Required]
        public string CaseStatus { get; set; }


        public int? LocationId { get; set; }

        [ForeignKey("LocationId")]
        public virtual CaseLocation CaseLocation { get; set; }
        
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        
    }
   
    public enum CaseStatus
    {
       Open,
        Closed,
        InProgress,
        Resolved
    }

     public enum CaseIssueType
     {
         IssueType1,
         IssueType2,
         IssueType3,
         IssueType4
     }
}