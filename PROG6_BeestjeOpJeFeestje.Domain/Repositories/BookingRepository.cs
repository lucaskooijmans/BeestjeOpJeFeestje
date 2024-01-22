using System.Diagnostics.CodeAnalysis;
using Domain.Data;
using Domain.Models;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories;
[ExcludeFromCodeCoverage]

public class BookingRepository: IBookingRepository
{
    
    private readonly ApplicationDbContext _context;

    public BookingRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Booking>> GetAllBookings()
    {
        return await _context.Bookings.ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetAllBookingsForUser(string userName)
    {
        return await _context.Bookings.Where(b => b.Email == userName).ToListAsync();
    }

    public async Task<Booking?> GetBookingByIdOrNull(int id)
    {
        return await _context.Bookings.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task AddBooking(Booking booking)
    {
        List<Animal> animals = booking.Animals.ToList();
        booking.Animals.Clear();
        _context.Bookings.Add(booking);
        
        
        await _context.SaveChangesAsync();
        
        foreach (var animal in animals)
        {
            _context.AnimalBookings.Add(new AnimalBooking
            {
                AnimalId = animal.Id,
                BookingId = booking.Id
            });
        }
        
        await _context.SaveChangesAsync();
    }

    public async Task UpdateBooking(Booking booking)
    {
        _context.Entry(booking).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteBooking(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking != null)
        {
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> BookingExists(int id)
    {
        return await _context.Bookings.AnyAsync(a => a.Id == id);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
    
}