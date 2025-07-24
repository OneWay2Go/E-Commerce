namespace ECommerce.Application.Models.DTOs;

public class ReviewDto
{
    public int ProductId { get; set; }
    public int UserId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    public bool IsApproved { get; set; }
} 