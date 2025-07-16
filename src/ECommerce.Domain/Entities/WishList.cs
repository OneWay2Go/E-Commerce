namespace ECommerce.Domain.Entities;

public class WishList
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ProductId { get; set; }

    public DateTime AddedAt { get; set; }

    public bool IsDeleted { get; set; } = false;

    // Navigation properties

    public User User { get; set; }

    public Product Product { get; set; }
}
