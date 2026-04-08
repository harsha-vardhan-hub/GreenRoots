using System;
using System.ComponentModel.DataAnnotations;

namespace GreenRoots.API.DTOs;

public class CreateTreeRequestDto
{
    [Required]
    public string Location { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Number of trees must be greater than 0")]
    public int NumberOfTrees { get; set; }
}

public class TreeRequestResponseDto
{
    public int Id { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int NumberOfTrees { get; set; }
    public string Status { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string UserEmail { get; set; } = string.Empty;
}

public class UpdateStatusDto
{
    [Required]
    public string Status { get; set; } = string.Empty;
    
    // Optional: Could also allow updating PaymentStatus via admin
    public string? PaymentStatus { get; set; }
}
