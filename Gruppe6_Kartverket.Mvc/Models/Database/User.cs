using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Gruppe6_Kartverket.Mvc.Models.Database
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [StringLength(2)]
        [Required]
        public string UserType { get; set; }

        [ForeignKey("UserType")]
        public virtual UserTypes UserTypeNavigation { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(100)]
        public string UserPassword { get; set; }

        public virtual UserInfo UserInfo { get; set; }
        public virtual ICollection<CaseRecord> CaseRecords { get; set; }
    }
}