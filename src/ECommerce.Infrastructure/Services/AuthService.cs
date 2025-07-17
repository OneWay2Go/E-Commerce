using ECommerce.Application.Interfaces;
using ECommerce.Application.Models;
using ECommerce.Application.Models.DTOs;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ECommerce.Infrastructure.Services;

public class AuthService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IRoleRepository roleRepository,
    IUserRoleRepository userRoleRepository,
    IOptions<JwtOptions> jwtOptions) : IAuthService
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public async Task<string> GenerateJwtTokenAsync(string email)
    {
        var user = await userRepository.GetByCondition(u => u.Email == email)
            .Include(u => u.UserRoles)
            .ThenInclude(u => u.Role)
            .ThenInclude(x => x.RolePermissions)
            .ThenInclude(x => x.Permission)
            .FirstOrDefaultAsync();

        var claims = new List<Claim>
        {
            new Claim("email", email)
        };

        if (user.UserRoles.Any())
        {
            foreach(var userRole in user.UserRoles)
            {
                claims.Add(new Claim("role", userRole.Role.Name));
                foreach (var permission in userRole.Role.RolePermissions.Select(rp => rp.Permission))
                {
                    claims.Add(new Claim("permission", permission.Name));
                }
            }
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));

        var jwtToken = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
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
        var user = await userRepository.GetByCondition(u => u.Email == request.Email && !u.IsDeleted)
            .Include(u => u.UserRoles)
            .ThenInclude(u => u.Role)
            .ThenInclude(x => x.RolePermissions)
            .ThenInclude(x => x.Permission)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return ApiResult<LoginResponseDto>.Failure("User not found or deleted.");
        }

        if (!user.IsEmailConfirmed)
        {
            return ApiResult<LoginResponseDto>.Failure("Email not confirmed. Please check your email.");
        }

        if (!passwordHasher.Verify(user.PasswordHash, request.Password, user.PasswordSalt))
        {
            return ApiResult<LoginResponseDto>.Failure("Invalid password.");
        }

        var accessToken = await GenerateJwtTokenAsync(user.Email);
        var refreshToken = GenerateRefreshToken();
        var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();

        return ApiResult<LoginResponseDto>.Success(new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Role = string.Join(',', roles)
        });
    }

    public async Task<ApiResult<RegisterResponseDto>> RegisterAsync(RegisterRequestDto request)
    {
        var existingUser = userRepository.GetByCondition(u => u.Email == request.Email);
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
            IsEmailConfirmed = false,
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

        return ApiResult<RegisterResponseDto>.Success(new RegisterResponseDto
        {
            Email = newUser.Email,
            Message = "User registered successfully, chech your email to confirm your registration.",
        });
    }
}
