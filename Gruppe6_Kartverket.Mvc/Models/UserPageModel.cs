namespace Gruppe6_Kartverket.Mvc.Models;

public class UserPageModel
{
    public string UserName { get; set; }
    public string UserId { get; set; }
    public DateTime RegistrationDate { get; set; }
    public List<Case> Cases { get; set; } = new List<Case>();
    public int NewMessagesCount { get; set; }
    public List<Notification> Notifications { get; set; } = new List<Notification>();
}

public class Case
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; } // Open, Closed, etc.
    public string IssueType { get; set; }
    public string Description { get; set; }
}

public class Notification
{
    public string Message { get; set; }
    public DateTime Date { get; set; }
}