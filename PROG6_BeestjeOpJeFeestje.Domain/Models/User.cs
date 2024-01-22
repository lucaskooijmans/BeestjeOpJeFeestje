using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

[ExcludeFromCodeCoverage]
public class User : IdentityUser
{
    [Required] public required string FirstName { get; set; }

    public string? InBetween { get; set; }

    [Required] public required string LastName { get; set; }

    [Required] public required string Address { get; set; }

    [Required] public required MemberCard MemberCard { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}