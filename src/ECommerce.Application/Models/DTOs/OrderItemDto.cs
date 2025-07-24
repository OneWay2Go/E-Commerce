namespace ECommerce.Application.Models.DTOs;

public class OrderItemDto
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public double Subtotal { get; set; }
} 