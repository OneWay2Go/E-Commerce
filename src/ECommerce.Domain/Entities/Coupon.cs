using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities;

public class Coupon
{
    public int Id { get; set; }

    public string Code { get; set; }

    public DiscountType DiscountType { get; set; }

    public double DiscountValue { get; set; } // if DiscountType is percentage, it is likely be 25,35..., if it is fixed, it can be 25000, 35000..

    public double MinOrderAmount { get; set; }

    public DateOnly ValidFrom { get; set; }

    public DateOnly ValidTo { get; set; }

    public bool IsActive { get; set; } = true;

    public int UsageLimit { get; set; }

    public bool IsDeleted { get; set; } = false;

    // Navigation properties

    public ICollection<Order> Orders { get; set; }
}
