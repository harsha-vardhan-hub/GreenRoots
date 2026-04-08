using System;

namespace GreenRoots.API.Models;

public class TreeRequest
{
    public int Id { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int NumberOfTrees { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, Planted
    public string PaymentStatus { get; set; } = "Pending"; // Pending, Completed
    
    // Foreign Key for User
    public int UserId { get; set; }
    public User? User { get; set; }
    
    // Tracking Fields
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
}
