using GreenRoots.Data;
using GreenRoots.DTOs;
using GreenRoots.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenRoots.Controllers;

[Authorize(Roles = "User")]
public class RequestsController : Controller
{
    private readonly ITreeRequestService _requestService;
    private readonly AppDbContext _context;

    public RequestsController(ITreeRequestService requestService, AppDbContext context)
    {
        _requestService = requestService;
        _context = context;
    }

    // GET: /Requests/MyRequests
    public async Task<IActionResult> MyRequests()
    {
        var userId = await GetCurrentUserId();
        var requests = await _requestService.GetUserRequestsAsync(userId);
        return View(requests);
    }

    // GET: /Requests/Submit
    public IActionResult Submit() => View(new CreateTreeRequestDto());

    // POST: /Requests/Submit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(CreateTreeRequestDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        try
        {
            var userId = await GetCurrentUserId();
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? "";
            await _requestService.CreateRequestAsync(userId, email, dto);

            TempData["Success"] = $"âœ… Your request for {dto.NumberOfTrees} tree(s) at '{dto.Location}' has been submitted!";
            return RedirectToAction("MyRequests");
        }
        catch
        {
            ModelState.AddModelError("", "Something went wrong. Please try again.");
            return View(dto);
        }
    }

    private async Task<int> GetCurrentUserId()
    {
        var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email)) return 0;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        return user?.Id ?? 0;
    }
}
