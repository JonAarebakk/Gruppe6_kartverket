using Gruppe6_Kartverket.Mvc.Controllers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Gruppe6_Kartverket.Mvc.Models;

public class HomeViewModel
{
    public string? Message { get; set; }
    [Required]
    [DisplayName("Ny melding")]
    public string? NewMessage { get; set; }


    public string Hidden { get; set; }

    public string? MapData { get; set; }
}
