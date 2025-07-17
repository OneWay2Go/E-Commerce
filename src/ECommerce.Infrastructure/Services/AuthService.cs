using ECommerce.Application.Interfaces;
using ECommerce.Application.Models;
using ECommerce.Application.Models.DTOs;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ECommerce.Infrastructure.Services;

public class AuthService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IRoleRepository roleRepository,
    IUserRoleRepository userRoleRepository) : IAuthService
{
    public async Task<string> GenerateJwtTokenAsync(string email)
    {
        var user = await userRepository.GetByCondition(u => u.Email == email).FirstOrDefaultAsync();

        var claims = new List<Claim>
        {
            new Claim("email", email)
        };

        return user.Email.ToString();
    }

    public async Task<ApiResult<LoginResponseDto>> LoginAsync(LoginRequestDto request)
    {
        var user = await userRepository.GetByCondition(u => u.Email == request.Email && !u.IsDeleted)
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

        return ApiResult<LoginResponseDto>.Success(new LoginResponseDto
        {
            AccessToken = await GenerateJwtTokenAsync(user.Email),
            RefreshToken = Guid.NewGuid().ToString(), // Placeholder for refresh token generation
            Role = user.UserRoles
                .Select(ur => ur.Role.Name)
                .FirstOrDefault() ?? "User" // Default to "User" if no roles found
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

    public Task<bool> ValidateJwtTokenAsync()
    {
        throw new NotImplementedException();
    }
}
