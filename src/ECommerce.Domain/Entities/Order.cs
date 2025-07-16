using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities;

public class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int? CouponId { get; set; } // For discount

    public DateTime OrderDate { get; set; }

    public Status Status { get; set; }

    public double TotalAmount { get; set; }

    public int ShippingAddressId { get; set; }

    public string Notes { get; set; }

    public bool IsDeleted { get; set; } = false;

    // Navigation properties

    public User User { get; set; }

    public ShippingAddress ShippingAddress { get; set; }

    public Payment Payment { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; }

    public Coupon Coupon { get; set; }
}
