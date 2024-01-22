using Microsoft.AspNetCore.Mvc;

namespace PROG6_BeestjeOpJeFeestje.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}