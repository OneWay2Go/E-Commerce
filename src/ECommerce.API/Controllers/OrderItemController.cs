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
public class OrderItemController(
    IOrderItemRepository orderItemRepository,
    OrderItemMapper orderItemMapper
) : ControllerBase
{
    [HttpPost]
    [PermissionAuthorize(Permission.OrderItem_Create)]
    public async Task<ActionResult<ApiResult<OrderItemDto>>> Create([FromBody] OrderItemDto dto)
    {
        var entity = orderItemMapper.ToEntity(dto);
        await orderItemRepository.AddAsync(entity);
        await orderItemRepository.SaveChangesAsync();
        var resultDto = orderItemMapper.ToDto(entity);
        return Ok(ApiResult<OrderItemDto>.Success(resultDto));
    }

    [HttpGet]
    [PermissionAuthorize(Permission.OrderItem_GetAll)]
    public ActionResult<ApiResult<IEnumerable<OrderItemDto>>> GetAll()
    {
        var entities = orderItemRepository.GetAll().ToList();
        var dtos = entities.Select(orderItemMapper.ToDto).ToList();
        return Ok(ApiResult<IEnumerable<OrderItemDto>>.Success(dtos));
    }

    [HttpGet("{id}")]
    [PermissionAuthorize(Permission.OrderItem_GetById)]
    public async Task<ActionResult<ApiResult<OrderItemDto>>> GetById(int id)
    {
        var entity = await orderItemRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<OrderItemDto>.Failure($"OrderItem with id {id} not found."));
        var dto = orderItemMapper.ToDto(entity);
        return Ok(ApiResult<OrderItemDto>.Success(dto));
    }

    [HttpPut("{id}")]
    [PermissionAuthorize(Permission.OrderItem_Update)]
    public async Task<ActionResult<ApiResult<OrderItemDto>>> Update(int id, [FromBody] OrderItemDto dto)
    {
        var entity = await orderItemRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<OrderItemDto>.Failure($"OrderItem with id {id} not found."));
        var updated = orderItemMapper.ToEntity(dto);
        // Do not update the Id
        orderItemRepository.Update(updated);
        await orderItemRepository.SaveChangesAsync();
        var resultDto = orderItemMapper.ToDto(updated);
        return Ok(ApiResult<OrderItemDto>.Success(resultDto));
    }

    [HttpDelete("{id}")]
    [PermissionAuthorize(Permission.OrderItem_Delete)]
    public async Task<ActionResult<ApiResult<bool>>> Delete(int id)
    {
        var entity = await orderItemRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<bool>.Failure($"OrderItem with id {id} not found."));
        // No IsDeleted property, so perform hard delete
        orderItemRepository.Delete(entity);
        await orderItemRepository.SaveChangesAsync();
        return Ok(ApiResult<bool>.Success(true));
    }
} 