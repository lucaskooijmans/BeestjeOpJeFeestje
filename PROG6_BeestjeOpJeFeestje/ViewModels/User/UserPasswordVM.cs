using System.Diagnostics.CodeAnalysis;

namespace PROG6_BeestjeOpJeFeestje.ViewModels;
[ExcludeFromCodeCoverage]

public class UserPasswordVM
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string InBetween { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string MemberCard { get; set; }
    public string Password { get; set; }
    
}