using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using PROG6_BeestjeOpJeFeestje.ViewModels.Validation;

namespace PROG6_BeestjeOpJeFeestje.ViewModels;
[ExcludeFromCodeCoverage]

public class UserCreateVM
{
    [Required]
    public string FirstName { get; set; }
    public string? InBetween { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    [EmailAddress]
    [EmailUserUnique]
    [Display(Name = "Email Address")]
    public string Email { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public string MemberCard { get; set; }
    
}