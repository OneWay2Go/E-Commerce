namespace ECommerce.Application.Models.DTOs;

public class OrderDto
{
    public int UserId { get; set; }
    public int? CouponId { get; set; }
    public DateTime OrderDate { get; set; }
    public int Status { get; set; }
    public double TotalAmount { get; set; }
    public int ShippingAddressId { get; set; }
    public string Notes { get; set; }
} 