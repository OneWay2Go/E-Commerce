namespace ECommerce.Application.Models.DTOs;

public class LoginResponseDto
{
    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }

    public string Role { get; set; }
}
