using Microsoft.AspNetCore.Http;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace ECommerce.Infrastructure.Auth.Helpers;

public class AuthHelpers
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthHelpers(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int GetCurrentUserId()
    {
        var authHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            var jwt = authHeader.Substring("Bearer ".Length).Trim();

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(jwt);

            int userId = int.Parse(jwtToken.Claims.FirstOrDefault(c => c.Type == "userId")?.Value);

            return userId;
        }

        return -1;
    }
}
