using ECommerce.Application.Interfaces;
using ECommerce.Application.Mappers;
using ECommerce.Application.Models;
using ECommerce.Application.Models.DTOs;
using ECommerce.Domain.Entities.Auth;
using ECommerce.Infrastructure.Auth;
using ECommerce.Infrastructure.Auth.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(
    IUserRepository userRepository,
    UserMapper userMapper,
    ProfileMapper profileMapper,
    AuthHelpers authHelpers,
    ILoggingService loggingService
) : ControllerBase
{
    [HttpPost]
    [PermissionAuthorize(Domain.Enums.Permission.User_Create)]
    public async Task<ActionResult<ApiResult<UserDto>>> Create([FromBody] UserDto dto)
    {
        var entity = userMapper.ToEntity(dto);
        await userRepository.AddAsync(entity);
        await userRepository.SaveChangesAsync();
        var resultDto = userMapper.ToDto(entity);
        return Ok(ApiResult<UserDto>.Success(resultDto));
    }

    [HttpGet]
    [PermissionAuthorize(Domain.Enums.Permission.User_GetAll)]
    public ActionResult<ApiResult<IEnumerable<UserDto>>> GetAll()
    {
        var entities = userRepository.GetAll().ToList();
        var dtos = entities.Select(userMapper.ToDto).ToList();
        return Ok(ApiResult<IEnumerable<UserDto>>.Success(dtos));
    }

    [HttpGet("{id}")]
    [PermissionAuthorize(Domain.Enums.Permission.User_GetById)]
    public async Task<ActionResult<ApiResult<UserDto>>> GetById(int id)
    {
        var entity = await userRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<UserDto>.Failure($"User with id {id} not found."));
        var dto = userMapper.ToDto(entity);
        return Ok(ApiResult<UserDto>.Success(dto));
    }

    [HttpPut("{id}")]
    [PermissionAuthorize(Domain.Enums.Permission.User_Update)]
    public async Task<ActionResult<ApiResult<UserDto>>> Update(int id, [FromBody] UserDto dto)
    {
        var entity = await userRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<UserDto>.Failure($"User with id {id} not found."));
        var updated = userMapper.ToEntity(dto);
        // Do not update the Id
        userRepository.Update(updated);
        await userRepository.SaveChangesAsync();
        var resultDto = userMapper.ToDto(updated);
        return Ok(ApiResult<UserDto>.Success(resultDto));
    }

    [HttpDelete("{id}")]
    [PermissionAuthorize(Domain.Enums.Permission.User_Delete)]
    public async Task<ActionResult<ApiResult<bool>>> Delete(int id)
    {
        var entity = await userRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<bool>.Failure($"User with id {id} not found."));
        entity.IsDeleted = true;
        userRepository.Update(entity);
        await userRepository.SaveChangesAsync();
        return Ok(ApiResult<bool>.Success(true));
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<ActionResult<ApiResult<ProfileDto>>> GetProfile()
    {
        loggingService.LogInformation("=== PROFILE ENDPOINT CALLED ===");
        loggingService.LogInformation("Request Headers: {@Headers}",
            Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()));
        {
            try
            {
                var userId = authHelpers.GetCurrentUserId();
                if (userId == -1)
                {
                    loggingService.LogWarning("Profile access attempted without valid user ID");
                    return Unauthorized(ApiResult<ProfileDto>.Failure("Unauthorized."));
                }

                loggingService.LogInformation("Fetching profile for user ID: {UserId}", userId);
                var user = await userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    loggingService.LogWarning("User with ID {UserId} not found in database", userId);
                    return NotFound(ApiResult<ProfileDto>.Failure("User not found."));
                }

                // Manual mapping to avoid Mapperly issues
                var profileDto = new ProfileDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                };

                loggingService.LogInformation("Profile retrieved successfully for user: {UserId}, {FullName}, {Email}",
                    user.Id, user.FullName, user.Email);
                loggingService.LogUserAction("Profile Retrieved", userId, $"Profile data retrieved for {user.FullName}");

                return Ok(ApiResult<ProfileDto>.Success(profileDto));
            }
            catch (Exception ex)
            {
                loggingService.LogError("Error retrieving profile for user", ex);
                return BadRequest(ApiResult<ProfileDto>.Failure($"Error: {ex.Message}"));
            }
        }
    }
    [HttpGet("test-id/{id}")]
    public async Task<ActionResult<ApiResult<User>>> TestUserById(int id)
    {
        try
        {
            loggingService.LogInformation("Testing user lookup by ID: {UserId}", id);
            var user = await userRepository.GetByIdAsync(id);
            if (user == null)
            {
                loggingService.LogWarning("Test user not found by ID: {UserId}", id);
                return NotFound(ApiResult<User>.Failure($"User with ID {id} not found."));
            }

            loggingService.LogInformation("Test user by ID found: Id={UserId}, FullName={FullName}, Email={Email}, IsDeleted={IsDeleted}",
                user.Id, user.FullName, user.Email, user.IsDeleted);
            return Ok(ApiResult<User>.Success(user));
        }
        catch (Exception ex)
        {
            loggingService.LogError("Error in test user lookup by ID: {UserId}", ex, id);
            return BadRequest(ApiResult<User>.Failure($"Error: {ex.Message}"));
        }
    }
    [HttpGet("test-auth")]
    public ActionResult<ApiResult<string>> TestAuth()
    {
        loggingService.LogInformation("=== TEST AUTH ENDPOINT CALLED ===");
        loggingService.LogInformation("User Identity: {Identity}", User?.Identity?.Name ?? "null");
        loggingService.LogInformation("Is Authenticated: {IsAuthenticated}", User?.Identity?.IsAuthenticated ?? false);
        loggingService.LogInformation("Claims: {@Claims}", User?.Claims?.Select(c => new { c.Type, c.Value }).ToList());

        return Ok(ApiResult<string>.Success("Test auth endpoint working"));
    }
    [HttpGet("test-no-auth")]
    [AllowAnonymous]
    public ActionResult<ApiResult<string>> TestNoAuth()
    {
        loggingService.LogInformation("=== TEST NO AUTH ENDPOINT CALLED ===");
        loggingService.LogInformation("Request Headers: {@Headers}",
            Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()));

        return Ok(ApiResult<string>.Success("Test no auth endpoint working"));
    }
    [HttpGet("test/{email}")]
    public async Task<ActionResult<ApiResult<User>>> TestUser(string email)
    {
        try
        {
            loggingService.LogInformation("Testing user lookup by email: {Email}", email);
            var user = await userRepository.GetByCondition(u => u.Email == email).FirstOrDefaultAsync();
            if (user == null)
            {
                loggingService.LogWarning("Test user not found by email: {Email}", email);
                return NotFound(ApiResult<User>.Failure($"User with email {email} not found."));
            }

            loggingService.LogInformation("Test user found: Id={UserId}, FullName={FullName}, Email={Email}, IsDeleted={IsDeleted}",
                user.Id, user.FullName, user.Email, user.IsDeleted);
            return Ok(ApiResult<User>.Success(user));
        }
        catch (Exception ex)
        {
            loggingService.LogError("Error in test user lookup by email: {Email}", ex, email);
            return BadRequest(ApiResult<User>.Failure($"Error: {ex.Message}"));
        }
    }
    [HttpPost("test-generate-token")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResult<string>>> TestGenerateToken([FromBody] LoginRequestDto request)
    {
        try
        {
            loggingService.LogInformation("=== TEST TOKEN GENERATION ===");
            loggingService.LogInformation("Generating token for email: {Email}", request.Email);

            var authService = HttpContext.RequestServices.GetRequiredService<IAuthService>();
            var token = await authService.GenerateJwtTokenAsync(request.Email);

            loggingService.LogInformation("Token generated successfully: {Token}", token);

            return Ok(ApiResult<string>.Success(token));
        }
        catch (Exception ex)
        {
            loggingService.LogError("Error generating token", ex);
            return BadRequest(ApiResult<string>.Failure($"Error: {ex.Message}"));
        }
    }
}