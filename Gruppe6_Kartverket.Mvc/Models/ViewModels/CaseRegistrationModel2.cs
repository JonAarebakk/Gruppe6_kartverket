namespace Gruppe6_Kartverket.Mvc.Models;

public class CaseRegistrationModel2
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; } // Open, Closed, etc.
    public string IssueType { get; set; }
    public string Description { get; set; }
}
