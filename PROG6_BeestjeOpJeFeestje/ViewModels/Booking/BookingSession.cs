using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using BeestjeOpJeFeestje.Domain;
using Domain.Models;

namespace PROG6_BeestjeOpJeFeestje.ViewModels;
[ExcludeFromCodeCoverage]

public class BookingSession
{
    public DateTime Date { get; set; }
    public decimal? Price { get; set; }
    public string? FirstName { get; set; }
    public string? InBetween { get; set; }
    public string? LastName { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string? UserId { get; set; }
    public ICollection<Animal> Animals { get; set; } = new List<Animal>();
    
    public ICollection<Discount> Discounts { get; set; } = new List<Discount>();
   
}
[ExcludeFromCodeCoverage]
public static class SessionExtensions
{
    public static void Set<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonSerializer.Serialize(value));
    }

    public static T? Get<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default : JsonSerializer.Deserialize<T>(value);
    }
}