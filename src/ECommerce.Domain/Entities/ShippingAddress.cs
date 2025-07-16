namespace ECommerce.Domain.Entities;

public class ShippingAddress
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string FullName { get; set; }

    public string Street { get; set; }

    public string City { get; set; }

    public string PostalCode { get; set; }

    public string Country { get; set; }

    public string PhoneNumber { get; set; }

    public bool IsDeleted { get; set; } = false;

    // Navigation properties

    public User User { get; set; }

    public ICollection<Order> Orders { get; set; }
}
