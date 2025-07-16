namespace ECommerce.Domain.Entities;

public class Role
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool IsDeleted { get; set; } = false;
}
