using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;

namespace UserManagement.Models;
public class User
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    [Display(Name = "Forename")]
    [StringLength(20)]
    public string Forename { get; set; } = default!;
    [Display(Name = "Surname")]
    [StringLength(20)]
    public string Surname { get; set; } = default!;
    [Display(Name = "Email")]
    [EmailAddress]
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    [Display(Name = "Date of Birth")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }
    [Display(Name = "Account Active")]
    public bool IsActive { get; set; }
}
