using BeestjeOpJeFeestje.Domain;
using Domain.Models;
using System.Diagnostics.CodeAnalysis;

namespace PROG6_BeestjeOpJeFeestje.ViewModels;
[ExcludeFromCodeCoverage]

public class BookingSummaryVM
{
    public DateTime Date { get; set; }
    public string FirstName { get; set; }
    public string InBetween { get; set; }
    public string LastName { get; set; }
    
    public string Address { get; set; }
    public string Email { get; set; }
    
    public decimal? Price { get; set; }
    
    public ICollection<Animal> Animals { get; set; } = new List<Animal>();
    
    public ICollection<Discount> Discounts { get; set; } = new List<Discount>();
    
    
    public List<AnimalVM> SelectedAnimals { get; set; }

}