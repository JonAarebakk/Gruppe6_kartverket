using System.ComponentModel.DataAnnotations;

namespace Gruppe6_Kartverket.Mvc.Models.ViewModels
{
    public class CaseRegistrationModel
    {
        [Required(ErrorMessage = "Case title is required.")]
        [StringLength(100, ErrorMessage = "Case title cannot be longer than 100 characters.")]
        public string CaseTitle { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public string Kategori { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Beskrivelse { get; set; }

        [Required(ErrorMessage = "GeoJson is required.")]
         public string GeoJson { get; set; }

        public double CenterLongitude { get; set; }
        public double CenterLatitude { get; set; }
    }
}