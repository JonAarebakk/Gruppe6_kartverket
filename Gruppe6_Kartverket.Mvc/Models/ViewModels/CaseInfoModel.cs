namespace Gruppe6_Kartverket.Mvc.Models;

public class CaseInfoModel
{
    public int CaseId { get; set; } = 0;
    public string Title { get; set; } = "";
    public string Location { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime Date { get; set; } = DateTime.Now;
    public string Status { get; set; } = ""; // Open, Closed, etc.
}
