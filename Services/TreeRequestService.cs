using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GreenRoots.Data;
using GreenRoots.DTOs;
using GreenRoots.Models;

namespace GreenRoots.Services;

public interface ITreeRequestService
{
    Task<TreeRequestResponseDto> CreateRequestAsync(int userId, string email, CreateTreeRequestDto dto);
    Task<IEnumerable<TreeRequestResponseDto>> GetUserRequestsAsync(int userId);
    Task<IEnumerable<TreeRequestResponseDto>> GetAllRequestsAsync();
    Task<TreeRequestResponseDto?> UpdateRequestStatusAsync(int id, UpdateStatusDto dto);
}

public class TreeRequestService : ITreeRequestService
{
    private readonly AppDbContext _context;

    public TreeRequestService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TreeRequestResponseDto> CreateRequestAsync(int userId, string email, CreateTreeRequestDto dto)
    {
        try
        {
            var request = new TreeRequest
            {
                Location = dto.Location,
                Message = dto.Message,
                NumberOfTrees = dto.NumberOfTrees,
                UserId = userId,
                CreatedBy = email,
                CreatedAt = DateTime.UtcNow,
                Status = "Pending",
                PaymentStatus = "Pending"
            };

            _context.TreeRequests.Add(request);
            await _context.SaveChangesAsync();

            return MapToResponse(request, email);
        }
        catch (Exception ex)
        {
            throw new Exception("Error creating tree request.", ex);
        }
    }

    public async Task<IEnumerable<TreeRequestResponseDto>> GetUserRequestsAsync(int userId)
    {
        try
        {
            var requests = await _context.TreeRequests
                .Include(r => r.User)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return requests.Select(r => MapToResponse(r, r.User?.Email ?? string.Empty));
        }
        catch (Exception ex)
        {
            throw new Exception("Error fetching user requests.", ex);
        }
    }

    public async Task<IEnumerable<TreeRequestResponseDto>> GetAllRequestsAsync()
    {
        try
        {
            var requests = await _context.TreeRequests
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return requests.Select(r => MapToResponse(r, r.User?.Email ?? string.Empty));
        }
        catch (Exception ex)
        {
            throw new Exception("Error fetching all requests.", ex);
        }
    }

    public async Task<TreeRequestResponseDto?> UpdateRequestStatusAsync(int id, UpdateStatusDto dto)
    {
        try
        {
            var request = await _context.TreeRequests
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null)
                return null;

            request.Status = dto.Status;
            
            if (!string.IsNullOrEmpty(dto.PaymentStatus))
            {
                request.PaymentStatus = dto.PaymentStatus;
            }

            request.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return MapToResponse(request, request.User?.Email ?? string.Empty);
        }
        catch (Exception ex)
        {
            throw new Exception("Error updating request.", ex);
        }
    }

    private TreeRequestResponseDto MapToResponse(TreeRequest request, string userEmail)
    {
        return new TreeRequestResponseDto
        {
            Id = request.Id,
            Location = request.Location,
            Message = request.Message,
            NumberOfTrees = request.NumberOfTrees,
            Status = request.Status,
            PaymentStatus = request.PaymentStatus,
            CreatedAt = request.CreatedAt,
            CreatedBy = request.CreatedBy,
            UserEmail = userEmail
        };
    }
}
