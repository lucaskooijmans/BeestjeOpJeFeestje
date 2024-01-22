using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories.Interfaces;

public interface IUserRepository
{
    public Task<IEnumerable<User>> GetAllUsers();

    public Task<User?> GetUserByIdOrNull(string id);

    public Task AddUser(User user);

    public Task UpdateUser(User user);

    public Task DeleteUser(string id);

    public Task<bool> UserExists(string id);
    
    public Task<User?> GetUserByEmailOrNull(string email);

    public void Dispose();
}