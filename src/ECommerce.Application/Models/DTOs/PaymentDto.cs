namespace ECommerce.Application.Models.DTOs;

public class PaymentDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int PaymentMethod { get; set; }
    public int PaymentStatus { get; set; }
    public Guid TransactionId { get; set; }
    public double AmountPaid { get; set; }
    public DateTime PaidAt { get; set; }
} 