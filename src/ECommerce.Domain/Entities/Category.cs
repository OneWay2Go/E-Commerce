namespace ECommerce.Domain.Entities;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int ParentId { get; set; }

    public bool IsDeleted { get; set; } = false;

    // Navigation properties 

    public ICollection<Product> Products { get; set; }
}
