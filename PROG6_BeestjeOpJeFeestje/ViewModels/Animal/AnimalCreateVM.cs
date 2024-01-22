using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PROG6_BeestjeOpJeFeestje.ViewModels;

[ExcludeFromCodeCoverage]

public class AnimalCreateVM
{

    [Required] public required string Name { get; set; }

    [Required] public required string Type { get; set; }

    [Required]
    [DataType(DataType.Currency)]
    [Range(0, 9999999999999999.99)]
    public required decimal Price { get; set; }

    [Microsoft.Build.Framework.Required] public required string ImageUrl { get; set; }

    [Microsoft.Build.Framework.Required] public required bool IsVip { get; set; }
}