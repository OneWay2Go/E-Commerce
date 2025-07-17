using ECommerce.Domain.Entities.Auth;

namespace ECommerce.Domain.Entities;

public class Review
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int UserId { get; set; }

    public int Rating { get; set; }

    public string Comment { get; set; }

    public bool IsApproved { get; set; } // For moderation to check for bad words

    public bool IsDeleted { get; set; } = false;

    // Navigation properties

    public User User { get; set; }

    public Product Product { get; set; }
}
