namespace ECommerce.Domain.Enums;

public enum Status
{
    Pending = 0,         // Order created but not paid
    Paid = 1,            // Payment successful
    Processing = 2,      // Preparing the order (e.g., picking, packing)
    Shipped = 3,         // Order has been shipped to the customer
    Delivered = 4,       // Customer has received the order
    Cancelled = 5,       // Cancelled by user or system before shipping
    Refunded = 6,        // Payment refunded after delivery or cancellation
    Failed = 7           // Payment failed or system error
}
