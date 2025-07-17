namespace ECommerce.Application.Models.DTOs;

public class WishListDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public DateTime AddedAt { get; set; }
} 