using ECommerce.Application.Interfaces;
using ECommerce.Application.Mappers;
using ECommerce.Application.Models;
using ECommerce.Application.Models.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Auth;
using ECommerce.Domain.Enums;
using ECommerce.Infrastructure.Auth;
using ECommerce.Infrastructure.Auth.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartController(
    ICartRepository cartRepository,
    CartMapper cartMapper,
    AuthHelpers authHelpers,
    ILoggingService loggingService
) : ControllerBase
{
    [HttpPost]
    [PermissionAuthorize(Domain.Enums.Permission.Cart_Create)]
    public async Task<ActionResult<ApiResult<CartDto>>> Create([FromBody] CartDto dto)
    {
        var entity = cartMapper.ToEntity(dto);
        await cartRepository.AddAsync(entity);
        await cartRepository.SaveChangesAsync();
        var resultDto = cartMapper.ToDto(entity);
        return Ok(ApiResult<CartDto>.Success(resultDto));
    }

    [HttpGet]
    [PermissionAuthorize(Domain.Enums.Permission.Cart_GetAll)]
    public ActionResult<ApiResult<IEnumerable<CartDto>>> GetAll()
    {
        var entities = cartRepository.GetAll().ToList();
        var dtos = entities.Select(cartMapper.ToDto).ToList();
        return Ok(ApiResult<IEnumerable<CartDto>>.Success(dtos));
    }

    [HttpGet("{id}")]
    [PermissionAuthorize(Domain.Enums.Permission.Cart_GetById)]
    public async Task<ActionResult<ApiResult<CartDto>>> GetById(int id)
    {
        var entity = await cartRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<CartDto>.Failure($"Cart with id {id} not found."));
        var dto = cartMapper.ToDto(entity);
        return Ok(ApiResult<CartDto>.Success(dto));
    }

    [HttpPut("{id}")]
    [PermissionAuthorize(Domain.Enums.Permission.Cart_Update)]
    public async Task<ActionResult<ApiResult<CartDto>>> Update(int id, [FromBody] CartDto dto)
    {
        var entity = await cartRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<CartDto>.Failure($"Cart with id {id} not found."));
        var updated = cartMapper.ToEntity(dto);
        // Do not update the Id
        cartRepository.Update(updated);
        await cartRepository.SaveChangesAsync();
        var resultDto = cartMapper.ToDto(updated);
        return Ok(ApiResult<CartDto>.Success(resultDto));
    }

    [HttpDelete("{id}")]
    [PermissionAuthorize(Domain.Enums.Permission.Cart_Delete)]
    public async Task<ActionResult<ApiResult<bool>>> Delete(int id)
    {
        var entity = await cartRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<bool>.Failure($"Cart with id {id} not found."));
        entity.IsDeleted = true;
        cartRepository.Update(entity);
        await cartRepository.SaveChangesAsync();
        return Ok(ApiResult<bool>.Success(true));
    }

    [HttpGet("my-cart")]
    [Authorize]
    public async Task<ActionResult<ApiResult<CartDto>>> GetMyCart()
    {
        loggingService.LogInformation("=== CART ENDPOINT CALLED ===");
        loggingService.LogInformation("Request Headers: {@Headers}",
            Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()));
        {
            try
            {
                var userId = authHelpers.GetCurrentUserId();
                if (userId == -1)
                {
                    loggingService.LogWarning("Cart access attempted without valid user ID");
                    return Unauthorized(ApiResult<CartDto>.Failure("Unauthorized."));
                }

                loggingService.LogInformation("Fetching cart for user ID: {UserId}", userId);
                var cart = await cartRepository.GetByCondition(c => c.UserId == userId)
                    .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync();
                if (cart == null)
                {
                    loggingService.LogWarning("Cart not found for user ID: {UserId}", userId);
                    return NotFound(ApiResult<CartDto>.Failure("Cart not found."));
                }

                // Manual mapping to avoid Mapperly issues
                var cartDto = new CartDto
                {
                    Id = cart.Id,
                    UserId = cart.UserId,
                    CartItems = cart.CartItems?.Select(ci => new CartItemDto
                    {
                        Id = ci.Id,
                        CartId = ci.CartId,
                        ProductId = ci.ProductId,
                        Quantity = ci.Quantity,
                        Product = ci.Product != null ? new ProductDto
                        {
                            Id = ci.Product.Id,
                            Name = ci.Product.Name,
                            Description = ci.Product.Description,
                            Price = ci.Product.Price,
                            Stock = ci.Product.Stock,
                            CategoryId = ci.Product.CategoryId
                        } : null
                    }).ToList() ?? new List<CartItemDto>()
                };

                loggingService.LogInformation("Cart retrieved successfully for user: {UserId}, Cart ID: {CartId}, Items: {ItemCount}",
                    userId, cart.Id, cart.CartItems?.Count ?? 0);
                loggingService.LogUserAction("Cart Retrieved", userId, $"Cart with {cart.CartItems?.Count ?? 0} items retrieved");
                loggingService.LogDatabaseOperation("Select", "Cart", cart.Id);

                return Ok(ApiResult<CartDto>.Success(cartDto));
            }
            catch (Exception ex)
            {
                loggingService.LogError("Error retrieving cart for user", ex);
                return BadRequest(ApiResult<CartDto>.Failure($"Error: {ex.Message}"));
            }
        }
    }
}