using System.Diagnostics.CodeAnalysis;

namespace PROG6_BeestjeOpJeFeestje.ViewModels;
[ExcludeFromCodeCoverage]

public class BookingContactInfoVM
{
    public IEnumerable<AnimalVM> Animals { get; set; } = new List<AnimalVM>();
    
    public DateTime Date { get; set; }
    public string FirstName { get; set; }
    
    public string? InBetween { get; set; }
    
    public string LastName { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    
    public BookingSummaryVM? BookingSummary { get; set; }
}
