using ECommerce.Application.Interfaces;
using ECommerce.Application.Models;
using ECommerce.Application.Models.DTOs;
using ECommerce.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ECommerce.Infrastructure.Services;

public class AuthService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IRoleRepository roleRepository,
    IUserRoleRepository userRoleRepository,
    IOptions<JwtOptions> jwtOptions,
    EmailService emailService,
    ILoggingService loggingService) : IAuthService
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public async Task<string> GenerateJwtTokenAsync(string email)
    {
        loggingService.LogInformation("Generating JWT token for email: {Email}", email);
        
        var user = await userRepository.GetByCondition(u => u.Email == email)
            .Include(u => u.UserRoles)
            .ThenInclude(u => u.Role)
            .ThenInclude(x => x.RolePermissions)
            .ThenInclude(x => x.Permission)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            loggingService.LogWarning("User not found for email: {Email}", email);
            throw new InvalidOperationException($"User not found for email: {email}");
        }

        var claims = new List<Claim>
        {
            new Claim("userId", user.Id.ToString()),
            new Claim("email", email)
        };

        if (user.UserRoles.Any())
        {
            foreach(var userRole in user.UserRoles)
            {
                claims.Add(new Claim("role", userRole.Role.Name));
                foreach (var permission in userRole.Role.RolePermissions.Select(rp => rp.Permission))
                {
                    claims.Add(new Claim("permissions", permission.Name));
                }
            }
        }

        loggingService.LogInformation("JWT Claims: {@Claims}", claims.Select(c => new { c.Type, c.Value }));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        
        loggingService.LogInformation("JWT Options: Issuer={Issuer}, Audience={Audience}, ExpirationMinutes={ExpirationMinutes}", 
            _jwtOptions.Issuer, _jwtOptions.Audience, _jwtOptions.ExpirationMinutes);

        var jwtToken = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        loggingService.LogInformation("JWT token generated successfully for user: {UserId}", user.Id);
        
        return token;
    }

    public string GenerateRefreshToken()
    {
        byte[] bytes = new byte[64];

        using var randomGenerator =
            RandomNumberGenerator.Create();

        randomGenerator.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }

    public async Task<ApiResult<LoginResponseDto>> LoginAsync(LoginRequestDto request)
    {
        loggingService.LogInformation("Login attempt for email: {Email}", request.Email);
        
        var user = await userRepository.GetByCondition(u => u.Email == request.Email && !u.IsDeleted)
            .Include(u => u.UserRoles)
            .ThenInclude(u => u.Role)
            .ThenInclude(x => x.RolePermissions)
            .ThenInclude(x => x.Permission)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            loggingService.LogWarning("Login failed: User not found or deleted for email: {Email}", request.Email);
            return ApiResult<LoginResponseDto>.Failure("User not found or deleted.");
        }

        loggingService.LogInformation("User found for login: {UserId}, {FullName}", user.Id, user.FullName);

        //if (!user.IsEmailConfirmed)
        //{
        //    return ApiResult<LoginResponseDto>.Failure("Email not confirmed. Please check your email.");
        //}

        if (!passwordHasher.Verify(user.PasswordHash, request.Password, user.PasswordSalt))
        {
            loggingService.LogWarning("Login failed: Invalid password for user: {UserId}", user.Id);
            return ApiResult<LoginResponseDto>.Failure("Invalid password.");
        }

        loggingService.LogInformation("Password verified successfully for user: {UserId}", user.Id);

        var accessToken = await GenerateJwtTokenAsync(user.Email);
        var refreshToken = GenerateRefreshToken();
        var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();

        loggingService.LogInformation("Login successful for user: {UserId}, Roles: {Roles}", user.Id, string.Join(", ", roles));
        loggingService.LogAuthenticationEvent("Login", request.Email, true);

        return ApiResult<LoginResponseDto>.Success(new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Role = string.Join(',', roles)
        });
    }

    public async Task<ApiResult<RegisterResponseDto>> RegisterAsync(RegisterRequestDto request)
    {
        var existingUser = await userRepository.GetByCondition(u => u.Email == request.Email).FirstOrDefaultAsync();
        if(existingUser != null)
        {
            return ApiResult<RegisterResponseDto>.Failure("User already exists.");
        }

        var emailValidator = new EmailAddressAttribute();
        if (!emailValidator.IsValid(request.Email))
            return ApiResult<RegisterResponseDto>.Failure("Email formati noto‘g‘ri");

        var salt = Guid.NewGuid().ToString();
        var hashedPassword = passwordHasher.Encrypt(request.Password, salt);

        var newUser = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash = hashedPassword,
            PasswordSalt = salt,
            IsDeleted = false,
            //IsEmailConfirmed = false,
            PhoneNumber = request.PhoneNumber
        };

        await userRepository.AddAsync(newUser);
        await userRepository.SaveChangesAsync();

        var role = await roleRepository.GetByCondition(r => r.Name == "User").FirstOrDefaultAsync();
        if (role == null)
        {
            return ApiResult<RegisterResponseDto>.Failure("User role not found, please check your code or database.");
        }

        var userRole = new UserRole
        {
            RoleId = role.Id,
            UserId = newUser.Id
        };
        await userRoleRepository.AddAsync(userRole);
        await userRoleRepository.SaveChangesAsync();

        //var response = await emailService.SendEmailAsync(newUser.Email);
        //if (!response)
        //{
        //    return ApiResult<RegisterResponseDto>
        //        .Failure("Failed to send confirmation email. Please try again later.");
        //}

        return ApiResult<RegisterResponseDto>.Success(new RegisterResponseDto
        {
            Email = newUser.Email,
            Message = "User registered successfully.",
        });
    }
}
