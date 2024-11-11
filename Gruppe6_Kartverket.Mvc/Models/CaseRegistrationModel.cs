using System.ComponentModel.DataAnnotations;

namespace Gruppe6_Kartverket.Mvc.Models
{
    public class CaseRegistrationModel
    {
        [Required(ErrorMessage = "Case Title is required")]
        [StringLength(35, ErrorMessage = "Case Title must be less than 35 characters")]
        public required string CaseTitle { get; set; }

        public required string Kategori { get; set; }

        [Required(ErrorMessage = "Beskrivelse is required")]
        [StringLength(300, ErrorMessage = "Beskrivelse must be less than 300 characters")]
        public required string Beskrivelse { get; set; }
    }
}
