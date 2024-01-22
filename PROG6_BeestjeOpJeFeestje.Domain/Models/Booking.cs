using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Models;

[ExcludeFromCodeCoverage]
public class Booking
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required] public DateTime Date { get; set; }

    [Required]
    [DataType(DataType.Currency)]
    [Range(0, 9999999999999999.99)]
    public decimal Price { get; set; }
    
    public string? UserId { get; set; }

    public virtual User? User { get; set; }
    
    public string? FirstName { get; set; }
    public string? InBetween { get; set; }
    public string? LastName { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public ICollection<Animal> Animals { get; set; }
}