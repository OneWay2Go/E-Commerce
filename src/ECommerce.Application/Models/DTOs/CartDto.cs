namespace ECommerce.Application.Models.DTOs;

public class CartDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public ICollection<CartItemDto> CartItems { get; set; }
} 