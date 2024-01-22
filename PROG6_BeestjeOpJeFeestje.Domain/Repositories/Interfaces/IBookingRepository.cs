using Domain.Models;

namespace Domain.Repositories.Interfaces;

public interface IBookingRepository
{
    public Task<IEnumerable<Booking>> GetAllBookings();
    
    public Task<IEnumerable<Booking>> GetAllBookingsForUser(string userName);

    public Task<Booking?> GetBookingByIdOrNull(int id);

    public Task AddBooking(Booking booking);

    public Task UpdateBooking(Booking booking);

    public Task DeleteBooking(int id);

    public Task<bool> BookingExists(int id);

    public void Dispose();
    
}