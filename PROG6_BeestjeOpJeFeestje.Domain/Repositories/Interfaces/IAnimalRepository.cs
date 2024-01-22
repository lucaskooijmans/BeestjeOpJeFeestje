using Domain.Models;

namespace Domain.Repositories.Interfaces;

public interface IAnimalRepository
{
    
    bool ExcludePenguin { get; set; }
    bool ExcludeDesert { get; set; }
    bool ExcludeSnow { get; set; }
    bool ExcludeUnavailable { get; set; }
    
    bool ExcludeVip { get; set; }
    public void SetFiltersToDefault();

    
    public Task<IEnumerable<Animal>> GetAllAnimals();

    public Task<Animal?> GetAnimalByIdOrNull(int id);

    public Task<IEnumerable<Animal>> GetAllAvailableAnimals(DateTime date);


    public Task AddAnimal(Animal animal);

    public Task UpdateAnimal(Animal animal);

    public Task DeleteAnimal(int id);

    public Task<bool> AnimalExists(int id);

    public void Dispose();
}