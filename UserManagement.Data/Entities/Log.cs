namespace UserManagement.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Log
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public string Action { get; set; } = null!;
    public long TargetUserId { get; set; }
    public string? Details { get; set; }
    public DateTime Timestamp { get; set; }
}