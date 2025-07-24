namespace ECommerce.Application.Models.DTOs;

public class WishListDto
{
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public DateTime AddedAt { get; set; }
} 