namespace ECommerce.Domain.Entities.Auth;

public class Role
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool IsDeleted { get; set; } = false;

    // Navigation properties

    public ICollection<UserRole> UserRoles { get; set; }
    public ICollection<RolePermission> RolePermissions { get; set; }
}
