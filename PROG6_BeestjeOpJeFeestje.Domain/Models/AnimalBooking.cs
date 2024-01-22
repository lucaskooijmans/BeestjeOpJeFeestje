using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[ExcludeFromCodeCoverage]
public class AnimalBooking
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int AnimalId { get; set; }
    public Animal? Animal { get; set; }
    public int BookingId { get; set; }
    public Booking? Booking { get; set; }
}