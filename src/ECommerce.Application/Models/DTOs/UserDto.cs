namespace ECommerce.Application.Models.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Code { get; set; }
    public bool IsEmailConfirmed { get; set; }
} 