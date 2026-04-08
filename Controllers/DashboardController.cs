using GreenRoots.API.Data;
using GreenRoots.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenRoots.API.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly ITreeRequestService _requestService;
    private readonly AppDbContext _context;

    public DashboardController(ITreeRequestService requestService, AppDbContext context)
    {
        _requestService = requestService;
        _context = context;
    }

    // GET: /Dashboard/UserIndex
    [Authorize(Roles = "User")]
    public async Task<IActionResult> UserIndex()
    {
        var userId = await GetCurrentUserId();
        if (userId == 0) return RedirectToAction("Login", "Account");

        var requests = (await _requestService.GetUserRequestsAsync(userId)).ToList();

        ViewBag.Total = requests.Count;
        ViewBag.Pending = requests.Count(r => r.Status == "Pending");
        ViewBag.Planted = requests.Count(r => r.Status == "Planted");
        ViewBag.UserName = User.FindFirst("FullName")?.Value ?? "User";

        return View(requests);
    }

    // GET: /Dashboard/AdminIndex
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AdminIndex()
    {
        var requests = (await _requestService.GetAllRequestsAsync()).ToList();

        ViewBag.Total = requests.Count;
        ViewBag.Pending = requests.Count(r => r.Status == "Pending");
        ViewBag.Planted = requests.Count(r => r.Status == "Planted");
        ViewBag.UserName = User.FindFirst("FullName")?.Value ?? "Admin";

        return View(requests);
    }

    // POST: /Dashboard/UpdateStatus
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, string status, string? paymentStatus)
    {
        var dto = new GreenRoots.API.DTOs.UpdateStatusDto
        {
            Status = status,
            PaymentStatus = paymentStatus
        };

        await _requestService.UpdateRequestStatusAsync(id, dto);
        TempData["Success"] = "Request status updated successfully.";
        return RedirectToAction("AdminIndex");
    }

    private async Task<int> GetCurrentUserId()
    {
        var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email)) return 0;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        return user?.Id ?? 0;
    }
}
