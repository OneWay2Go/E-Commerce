namespace ECommerce.Domain.Entities.Auth;

public class UserRole
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public int UserId { get; set; }

    // Navigation properties

    public User User { get; set; }

    public Role Role { get; set; }
}
