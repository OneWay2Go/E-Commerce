using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities;

public class Payment
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public PaymentStatus PaymentStatus { get; set; }

    public Guid TransactionId { get; set; }

    public double AmountPaid { get; set; }

    public DateTime PaidAt { get; set; }

    // Navigation properties

    public Order Order { get; set; }
}
