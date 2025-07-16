namespace ECommerce.Domain.Entities;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public double Price { get; set; }

    public int Stock { get; set; }

    public int CategoryId { get; set; }

    public bool IsDeleted { get; set; } = false;

    // Navigation properties

    public ICollection<CartItem> CartItems { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
    public ICollection<Review> Reviews { get; set; }
    public ICollection<WishList> Wishlist { get; set; }
}
