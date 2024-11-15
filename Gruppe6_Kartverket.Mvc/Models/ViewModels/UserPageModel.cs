namespace Gruppe6_Kartverket.Mvc.Models;

public class UserPageModel
{
    public string UserName { get; set; }
    public string UserId { get; set; }
    public DateTime RegistrationDate { get; set; }
    
    public string IssueType { get; set; }
    
    public string Title { get; set; }

    public string Description { get; set; }
    
    public List<CaseRecord> CaseRecords { get; set; } = new List<CaseRecord>();
    public int NewMessagesCount { get; set; }
    
    public List<Notification> Notifications { get; set; } = new List<Notification>();
}

