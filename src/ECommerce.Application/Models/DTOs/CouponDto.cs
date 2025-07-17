namespace ECommerce.Application.Models.DTOs;

public class CouponDto
{
    public int Id { get; set; }
    public string Code { get; set; }
    public int DiscountType { get; set; }
    public double DiscountValue { get; set; }
    public double MinOrderAmount { get; set; }
    public DateOnly ValidFrom { get; set; }
    public DateOnly ValidTo { get; set; }
    public bool IsActive { get; set; }
    public int UsageLimit { get; set; }
} 