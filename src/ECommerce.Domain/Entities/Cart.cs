namespace ECommerce.Domain.Entities;

public class Cart
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public bool IsDeleted { get; set; } = false;

    // Navigation properties

    public ICollection<CartItem> CartItems { get; set; }
}
