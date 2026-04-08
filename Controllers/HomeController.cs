using Microsoft.AspNetCore.Mvc;

namespace GreenRoots.API.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
