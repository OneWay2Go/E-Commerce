namespace ECommerce.Domain.Entities.Auth;

public class Permission
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool IsDeleted { get; set; } = false;

    // Navigation properties

    public ICollection<RolePermission> RolePermissions { get; set; }
}
