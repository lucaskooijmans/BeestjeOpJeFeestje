using System.Diagnostics.CodeAnalysis;
using Domain.Data;
using Domain.Models;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories;
[ExcludeFromCodeCoverage]

public class AnimalRepository : IAnimalRepository
{
    private readonly ApplicationDbContext _context;
    
    public bool ExcludePenguin { get; set; }
    public bool ExcludeDesert { get; set; }
    public bool ExcludeSnow { get; set; }
    public bool ExcludeVip { get; set; }

    public bool ExcludeUnavailable { get; set; } = false;

    public AnimalRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Animal>> GetAllAnimals()
    {
        return await _context.Animals.ToListAsync();
    }
    
    private IEnumerable<Animal> GetUnavailableAnimals(DateTime date)
    {
        List<Animal> list = (from item in _context.AnimalBookings.Include(animalBooking => animalBooking.Booking)
                .Include(animalBooking => animalBooking.Animal)
            where item.Booking != null && item.Booking.Date.Date == date.Date
            where item.Animal != null
            select item.Animal).ToList();
        {
           
        }
        return list;
    }
    
    private IEnumerable<Animal> GetVipAnimals()
    {
        return _context.Animals.Where(a => a.IsVip).ToList();
    }

    public async Task<IEnumerable<Animal>> GetAllAvailableAnimals(DateTime date)
    {
        
        List<Animal> list = await _context.Animals.ToListAsync();
        
        if (ExcludeUnavailable)
        {
            var unavailable = GetUnavailableAnimals(date.Date).ToList();


            foreach (var item in unavailable)
            {
                list.Remove(item);
            }
                
        }
        if (ExcludeVip)
        {
            var vip = GetVipAnimals();
            
            foreach (var item in vip)
            {
                list.Remove(item);
            }
                
        }
        if (ExcludePenguin)
        {
            var pin = list.SingleOrDefault(beast => beast.Name == "Pinguin");
            if (pin != null)
            {
                list.Remove(pin);
            }

        }
        if(ExcludeSnow)
        {
            for (int i = list.Count-1; i >= 0; i--)
            {
                if (list[i].Type == "Sneeuw")
                {
                    list.RemoveAt(i);
                }
            }
        }
        if(ExcludeDesert)
        {
            for (int i = list.Count-1; i >= 0; i--)
            {
                if (list[i].Type == "Woestijn")
                {
                    list.RemoveAt(i);
                }
            }
        }
        return list;
    }
    

    public async Task<Animal?> GetAnimalByIdOrNull(int id)
    {
        return await _context.Animals.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task AddAnimal(Animal animal)
    {
        _context.Animals.Add(animal);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAnimal(Animal animal)
    {
        _context.Entry(animal).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAnimal(int id)
    {
        var animal = await _context.Animals.FindAsync(id);
        if (animal != null)
        {
            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> AnimalExists(int id)
    {
        return await _context.Animals.AnyAsync(a => a.Id == id);
    }
    
    public void SetFiltersToDefault()
    {
        ExcludePenguin = false;
        ExcludeDesert = false;
        ExcludeSnow = false;
        ExcludeVip = false;
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}