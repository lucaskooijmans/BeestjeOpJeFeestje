using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PROG6_BeestjeOpJeFeestje.ViewModels;
[ExcludeFromCodeCoverage]

public class AnimalVM
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Type { get; set; }
    
    public decimal Price { get; set; }

    public string ImageUrl { get; set; }

    public bool IsVip { get; set; }
    
    public bool Selected { get; set; }
}