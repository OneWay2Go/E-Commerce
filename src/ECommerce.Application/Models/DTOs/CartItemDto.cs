namespace ECommerce.Application.Models.DTOs;

public class CartItemDto
{
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
} 