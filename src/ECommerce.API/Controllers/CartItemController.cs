using ECommerce.Application.Interfaces;
using ECommerce.Application.Models.DTOs;
using ECommerce.Application.Mappers;
using ECommerce.Application.Models;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Domain.Enums;
using ECommerce.Infrastructure.Auth;
using ECommerce.Infrastructure.Auth.Helpers;
using Microsoft.EntityFrameworkCore;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace ECommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartItemController(
    ICartItemRepository cartItemRepository,
    CartItemMapper cartItemMapper,
    ICartRepository cartRepository,
    AuthHelpers authHelpers
) : ControllerBase
{
    [HttpPost]
    [PermissionAuthorize(Permission.CartItem_Create)]
    public async Task<ActionResult<ApiResult<CartItemDto>>> Create([FromBody] CartItemDto dto)
    {
        var entity = cartItemMapper.ToEntity(dto);
        await cartItemRepository.AddAsync(entity);
        await cartItemRepository.SaveChangesAsync();
        var resultDto = cartItemMapper.ToDto(entity);
        return Ok(ApiResult<CartItemDto>.Success(resultDto));
    }

    [HttpGet]
    [PermissionAuthorize(Permission.CartItem_GetAll)]
    public ActionResult<ApiResult<IEnumerable<CartItemDto>>> GetAll()
    {
        var entities = cartItemRepository.GetAll().ToList();
        var dtos = entities.Select(cartItemMapper.ToDto).ToList();
        return Ok(ApiResult<IEnumerable<CartItemDto>>.Success(dtos));
    }

    [HttpGet("{id}")]
    [PermissionAuthorize(Permission.CartItem_GetById)]
    public async Task<ActionResult<ApiResult<CartItemDto>>> GetById(int id)
    {
        var entity = await cartItemRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<CartItemDto>.Failure($"CartItem with id {id} not found."));
        var dto = cartItemMapper.ToDto(entity);
        return Ok(ApiResult<CartItemDto>.Success(dto));
    }

    [HttpPut("{id}")]
    [PermissionAuthorize(Permission.CartItem_Update)]
    public async Task<ActionResult<ApiResult<CartItemDto>>> Update(int id, [FromBody] CartItemDto dto)
    {
        var entity = await cartItemRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<CartItemDto>.Failure($"CartItem with id {id} not found."));
        var updated = cartItemMapper.ToEntity(dto);
        // Do not update the Id
        cartItemRepository.Update(updated);
        await cartItemRepository.SaveChangesAsync();
        var resultDto = cartItemMapper.ToDto(updated);
        return Ok(ApiResult<CartItemDto>.Success(resultDto));
    }

    [HttpDelete("{id}")]
    [PermissionAuthorize(Permission.CartItem_Delete)]
    public async Task<ActionResult<ApiResult<bool>>> Delete(int id)
    {
        var entity = await cartItemRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<bool>.Failure($"CartItem with id {id} not found."));
        // No IsDeleted property, so perform hard delete
        cartItemRepository.Delete(entity);
        await cartItemRepository.SaveChangesAsync();
        return Ok(ApiResult<bool>.Success(true));
    }

    [HttpPost("add-to-cart")]
    [Authorize]
    public async Task<ActionResult<ApiResult<CartItemDto>>> AddCartItemToCart([FromBody]CartItemDto request)
    {
        var userId = authHelpers.GetCurrentUserId();
        if (userId == -1)
            return Unauthorized();

        var cart = await cartRepository.GetAll()
            .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsDeleted);

        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            await cartRepository.AddAsync(cart);
            await cartRepository.SaveChangesAsync();
        }

        var existingItem = await cartItemRepository.GetAll()
            .FirstOrDefaultAsync(ci => ci.CartId == cart.Id && ci.ProductId == request.ProductId);

        if(existingItem != null)
        {
            existingItem.Quantity += request.Quantity;
            cartItemRepository.Update(existingItem);
        }
        else
        {
            var newItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };
            await cartItemRepository.AddAsync(newItem);
        }
        await cartItemRepository.SaveChangesAsync();

        var updatedItem = await cartItemRepository.GetAll()
            .FirstOrDefaultAsync(ci => ci.CartId == cart.Id && ci.ProductId == request.ProductId);
        
        var dto = cartItemMapper.ToDto(updatedItem);
        return Ok(ApiResult<CartItemDto>.Success(dto));
    }

    [HttpPut("update-quantity")]
    [Authorize]
    public async Task<ActionResult<ApiResult<CartItemDto>>> UpdateQuantity([FromBody]CartItemDto request)
    {
        var userId = authHelpers.GetCurrentUserId();
        if(userId == -1) 
            return Unauthorized();

        var existingCart = await cartRepository.GetByCondition(c => c.UserId == userId && !c.IsDeleted)
            .FirstOrDefaultAsync();
        if (existingCart == null)
            return NotFound(ApiResult<CartItemDto>.Failure("Cart to add item is not found."));

        var existingCartItem = await cartItemRepository.GetByCondition(c => c.CartId == existingCart.Id && c.ProductId == request.ProductId)
            .FirstOrDefaultAsync();
        if (existingCartItem == null)
            return NotFound(ApiResult<CartItemDto>.Failure("Cart item is not found."));

        existingCartItem.Quantity += request.Quantity;

        cartItemRepository.Update(existingCartItem);
        await cartItemRepository.SaveChangesAsync();

        var dto = cartItemMapper.ToDto(existingCartItem);
        return Ok(ApiResult<CartItemDto>.Success(dto));
    }

    [HttpDelete("remove/{itemId}")]
    public async Task<ActionResult<ApiResult<bool>>> RemoveItemFromCart(int itemId)
    {
        var userId = authHelpers.GetCurrentUserId();
        if (userId == -1)
            return Unauthorized();

        var existingCartItem = await cartItemRepository.GetByIdAsync(itemId);
        if (existingCartItem == null)
            return NotFound();

        var existingCart = await cartRepository.GetByCondition(c => c.UserId == userId && c.Id == existingCartItem.CartId && !c.IsDeleted)
            .FirstOrDefaultAsync();
        if (existingCart == null)
            return Forbid();

        cartItemRepository.Delete(existingCartItem);
        await cartItemRepository.SaveChangesAsync();

        return Ok(ApiResult<bool>.Success(true));
    }
} 