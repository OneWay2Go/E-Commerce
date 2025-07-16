namespace ECommerce.Domain.Entities;

public class User
{
    public int Id { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public string PhoneNumber { get; set; }

    public bool IsDeleted { get; set; } = false;

    public bool IsEmailConfirmed { get; set; } = false;
}
