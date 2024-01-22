using AutoMapper;
using Domain.Data;
using Domain.Models;
using Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG6_BeestjeOpJeFeestje.ViewModels;

namespace PROG6_BeestjeOpJeFeestje.Controllers;

public class UserController : Controller
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public UserController(IMapper mapper, IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    // GET: User
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Index()
    {
        var users = await _userRepository.GetAllUsers();
        return View(users);
    }

    // GET: User/Details/5
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Details(string? id)
    {
        if (id == null) return NotFound();

        var user = await _userRepository.GetUserByIdOrNull(id);
        if (user == null) return NotFound();

        return View(user);
    }

    // GET: User/Create
    [Authorize(Policy = "AdminOnly")]
    public ActionResult Create()
    {
        return View();
    }

    // POST: User/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create([Bind("FirstName,InBetween,LastName,Address,Email,PhoneNumber,MemberCard")] UserCreateVM user)
    {
        if (ModelState.IsValid)
        {
            User model = _mapper.Map<User>(user);
            model.UserName = user.Email;
            model.NormalizedUserName = user.Email.ToUpper();
            model.NormalizedEmail = user.Email.ToUpper();
            await _userRepository.AddUser(model);
            return RedirectToAction(nameof(UserPassword), new {id = model.Id});
        }

        return View(user);
    }
    
    [HttpGet]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UserPassword(string? id)
    {
        
        if (id == null) return NotFound();
        
        var model = await _userRepository.GetUserByIdOrNull(id);

        if (model == null) return NotFound();
        
        var user = _mapper.Map<UserPasswordVM>(model);

        string password = GeneratePassword();
        var pass = new PasswordHasher<object>().HashPassword(user, password);
        
        user.Password = password;
        model.PasswordHash = pass;
        
        await _userRepository.UpdateUser(model);
        
        return View(user);
        
    }

    // GET: User/Edit/5
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Edit(string? id)
    {
        if (id == null) return NotFound();

        var user = await _userRepository.GetUserByIdOrNull(id);
        if (user == null) return NotFound();
        
        
        return View(user);
    }

    // POST: User/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Edit(string id, [Bind("Id,FirstName,InBetween,LastName,Address,MemberCard")] User model)
    {
        if (id != model.Id) return NotFound();

        if (!ModelState.IsValid) return View(model);

        try
        {
            await _userRepository.UpdateUser(model);
        }
        catch
        {
            if (!_userRepository.UserExists(model.Id).Result)
                return NotFound();
            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: User/Delete/5
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(string? id)
    {
        if (id == null) return NotFound();

        var user = await _userRepository.GetUserByIdOrNull(id);
        if (user == null) return NotFound();

        return View(user);
    }

    // POST: User/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        await _userRepository.DeleteUser(id);
        return RedirectToAction(nameof(Index));
    }
    
    private string GeneratePassword()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%&*()_+=-<>?/.,;:[]{}";
        var stringChars = new char[8];
        var random = new Random();

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }

        return new string(stringChars);
    }
}