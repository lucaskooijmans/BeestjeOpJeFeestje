using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Domain.Models;

namespace PROG6_BeestjeOpJeFeestje.ViewModels;
[ExcludeFromCodeCoverage]

public class BookingAnimalsVM
{
    
    public DateTime Date { get; set; }
    
    public List<AnimalVM> Animals { get; set; }
    
    public BookingSummaryVM? BookingSummary { get; set; }
}