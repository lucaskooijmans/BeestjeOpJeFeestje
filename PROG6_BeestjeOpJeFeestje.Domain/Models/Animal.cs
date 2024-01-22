using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Models;

[ExcludeFromCodeCoverage]
public class Animal
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required int Id { get; set; }

    [Required] public required string Name { get; set; }

    [Required] public required string Type { get; set; }

    [Required]
    [DataType(DataType.Currency)]
    [Range(0, 9999999999999999.99)]
    public required decimal Price { get; set; }

    [Required] public required string ImageUrl { get; set; }

    [Required] public required bool IsVip { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}