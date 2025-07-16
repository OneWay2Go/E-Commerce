namespace ECommerce.Domain.Entities;

public class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public double Subtotal { get; set; } // Quantity * Price, Price can be brought by ProductId from Product
}
