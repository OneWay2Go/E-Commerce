using ECommerce.Application.Models;

namespace ECommerce.Application.Interfaces;

public interface IAuthService
{
    Task<ApiResult<string>> RegisterAsync();

    Task<ApiResult<string>> LoginAsync();
    
    void GenerateJwtTokenAsync();

    void ValidateJwtTokenAsync();
}
