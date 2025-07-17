using ECommerce.Application.Interfaces;
using ECommerce.Application.Models.DTOs;
using ECommerce.Application.Mappers;
using ECommerce.Application.Models;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Domain.Enums;
using ECommerce.Infrastructure.Auth;

namespace ECommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartItemController(
    ICartItemRepository cartItemRepository,
    CartItemMapper cartItemMapper
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
} 