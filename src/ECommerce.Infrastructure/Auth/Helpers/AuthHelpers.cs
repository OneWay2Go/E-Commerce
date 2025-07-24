using Microsoft.AspNetCore.Http;
using System;
using System.IdentityModel.Tokens.Jwt;
using ECommerce.Application.Interfaces;

namespace ECommerce.Infrastructure.Auth.Helpers;

public class AuthHelpers
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILoggingService _loggingService;

    public AuthHelpers(IHttpContextAccessor httpContextAccessor, ILoggingService loggingService)
    {
        _httpContextAccessor = httpContextAccessor;
        _loggingService = loggingService;
    }

    public int GetCurrentUserId()
    {
        try
        {
            var authHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var jwt = authHeader.Substring("Bearer ".Length).Trim();

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(jwt);

                // Debug: Log all claims
                var allClaims = jwtToken.Claims.Select(c => $"{c.Type}: {c.Value}").ToList();
                _loggingService.LogDebug("JWT Claims: {Claims}", string.Join(", ", allClaims));

                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
                if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int userId))
                {
                    _loggingService.LogDebug("Found userId: {UserId} from JWT token", userId);
                    return userId;
                }
                else
                {
                    _loggingService.LogWarning("userId claim not found or invalid: {UserIdClaim}", userIdClaim);
                }
            }
            else
            {
                _loggingService.LogWarning("Authorization header not found or invalid: {AuthHeader}", authHeader);
            }

            return -1;
        }
        catch (Exception ex)
        {
            // If there's any error parsing the JWT token, return -1
            _loggingService.LogError("Error parsing JWT token", ex);
            return -1;
        }
    }
}
