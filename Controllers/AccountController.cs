using System.Security.Claims;
using GreenRoots.DTOs;
using GreenRoots.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace GreenRoots.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService _authService;

    public AccountController(IAuthService authService)
    {
        _authService = authService;
    }

    // GET: /Account/Register
    [HttpGet]
    public IActionResult Register() => View();

    // POST: /Account/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        try
        {
            var user = await _authService.RegisterAsync(dto);
            if (user == null)
            {
                ModelState.AddModelError("Email", "An account with this email already exists.");
                return View(dto);
            }

            await SignInUser(user.Email, user.FullName, user.Role);
            TempData["Success"] = $"Welcome to GreenRoots, {user.FullName}!";
            return RedirectToAction(user.Role == "Admin" ? "AdminIndex" : "UserIndex", "Dashboard");
        }
        catch
        {
            ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
            return View(dto);
        }
    }

    // GET: /Account/Login
    [HttpGet]
    public IActionResult Login() => View();

    // POST: /Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        try
        {
            var user = await _authService.LoginAsync(dto);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(dto);
            }

            await SignInUser(user.Email, user.FullName, user.Role);
            TempData["Success"] = $"Hello, {user.FullName}!";
            return RedirectToAction(user.Role == "Admin" ? "AdminIndex" : "UserIndex", "Dashboard");
        }
        catch
        {
            ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
            return View(dto);
        }
    }

    // POST: /Account/Logout
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        TempData["Success"] = "You have been logged out.";
        return RedirectToAction("Index", "Home");
    }

    // GET: /Account/AccessDenied
    public IActionResult AccessDenied() => View();

    // Helper: sign in and create auth cookie
    private async Task SignInUser(string email, string fullName, string role)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, email),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role),
            new Claim("FullName", fullName)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
    }
}
