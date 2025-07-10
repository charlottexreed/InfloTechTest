using System;

namespace UserManagement.Web.Models.Users;
public class EditUserViewModel
{
    public long Id { get; set; }

    public string Forename { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public bool IsActive { get; set; }
}