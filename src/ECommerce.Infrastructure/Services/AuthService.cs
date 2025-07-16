using ECommerce.Application.Interfaces;
using ECommerce.Application.Models;

namespace ECommerce.Infrastructure.Services;

public class AuthService : IAuthService
{
    public void GenerateJwtTokenAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ApiResult<string>> LoginAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ApiResult<string>> RegisterAsync()
    {
        throw new NotImplementedException();
    }

    public void ValidateJwtTokenAsync()
    {
        throw new NotImplementedException();
    }
}
