namespace UserManagement.Web.Models.Logs;

using System;

public class LogListViewModel
{
    public List<LogListItemViewModel> Items { get; set; } = new();
    public string? Forename { get; set; }
    public string? Surname { get; set; }
}

public class LogListItemViewModel
{
    public long Id { get; set; }
    public string Action { get; set; } = null!;
    public long TargetUserId { get; set; }
    public string? Details { get; set; }
    public DateTime Timestamp { get; set; }
}
