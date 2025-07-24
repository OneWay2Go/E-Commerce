using ECommerce.Application.Models;
using ECommerce.Application.Models.DTOs;

namespace ECommerce.Application.Interfaces;

public interface IAuthService
{
    Task<ApiResult<RegisterResponseDto>> RegisterAsync(RegisterRequestDto request);

    Task<ApiResult<LoginResponseDto>> LoginAsync(LoginRequestDto request);

    Task<string> GenerateJwtTokenAsync(string email);

    string GenerateRefreshToken();
}
