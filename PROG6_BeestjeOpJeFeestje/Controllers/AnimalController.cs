using AutoMapper;
using Domain.Models;
using Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PROG6_BeestjeOpJeFeestje.ViewModels;

namespace PROG6_BeestjeOpJeFeestje.Controllers;

public class AnimalController : Controller
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IMapper _mapper;

    public AnimalController(IAnimalRepository animalRepository, IMapper mapper)
    {
        _animalRepository = animalRepository;
        _mapper = mapper;
    }

    // GET: Animal
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Index()
    {
        var animals = await _animalRepository.GetAllAnimals();
        return View(animals);
    }

    // GET: Animal/Details/5
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Details(int id)
    {
        var animal = await _animalRepository.GetAnimalByIdOrNull(id);
        if (animal == null) return NotFound();

        return View(animal);
    }

    // GET: Animal/Create
    [Authorize(Policy = "AdminOnly")]
    public ActionResult Create()
    {
        return View();
    }

    // POST: Animal/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create([Bind("Name,Type,Price,ImageUrl,IsVip")] AnimalCreateVM animal)
    {
        if (ModelState.IsValid)
        {
            Animal model = _mapper.Map<Animal>(animal);
            await _animalRepository.AddAnimal(model);
            return RedirectToAction(nameof(Index));
        }

        return View(animal);
    }

    // GET: Animal/Edit/5
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Edit(int id)
    {
        var animal = await _animalRepository.GetAnimalByIdOrNull(id);
        if (animal == null) return NotFound();
        
        
        return View(animal);
    }

    // POST: Animal/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type,Price,ImageUrl,IsVip")] Animal model)
    {
        if (id != model.Id) return NotFound();

        if (!ModelState.IsValid) return View(model);

        try
        {
            await _animalRepository.UpdateAnimal(model);
        }
        catch
        {
            if (!_animalRepository.AnimalExists(model.Id).Result)
                return NotFound();
            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Animal/Delete/5
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {

        var animal = await _animalRepository.GetAnimalByIdOrNull(id);
        if (animal == null) return NotFound();

        return View(animal);
    }

    // POST: Animal/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _animalRepository.DeleteAnimal(id);
        return RedirectToAction(nameof(Index));
    }
}