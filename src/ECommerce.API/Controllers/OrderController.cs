using ECommerce.Application.Interfaces;
using ECommerce.Application.Models.DTOs;
using ECommerce.Application.Mappers;
using ECommerce.Application.Models;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Domain.Enums;
using ECommerce.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using ECommerce.Infrastructure.Auth.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController(
    IOrderRepository orderRepository,
    OrderMapper orderMapper,
    AuthHelpers authHelpers
) : ControllerBase
{
    [HttpPost]
    [PermissionAuthorize(Permission.Order_Create)]
    public async Task<ActionResult<ApiResult<OrderDto>>> Create([FromBody] OrderDto dto)
    {
        var entity = orderMapper.ToEntity(dto);
        await orderRepository.AddAsync(entity);
        await orderRepository.SaveChangesAsync();
        var resultDto = orderMapper.ToDto(entity);
        return Ok(ApiResult<OrderDto>.Success(resultDto));
    }

    [HttpGet]
    [PermissionAuthorize(Permission.Order_GetAll)]
    public ActionResult<ApiResult<IEnumerable<OrderDto>>> GetAll()
    {
        var entities = orderRepository.GetAll().ToList();
        var dtos = entities.Select(orderMapper.ToDto).ToList();
        return Ok(ApiResult<IEnumerable<OrderDto>>.Success(dtos));
    }

    [HttpGet("{id}")]
    [PermissionAuthorize(Permission.Order_GetById)]
    public async Task<ActionResult<ApiResult<OrderDto>>> GetById(int id)
    {
        var entity = await orderRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<OrderDto>.Failure($"Order with id {id} not found."));
        var dto = orderMapper.ToDto(entity);
        return Ok(ApiResult<OrderDto>.Success(dto));
    }

    [HttpPut("{id}")]
    [PermissionAuthorize(Permission.Order_Update)]
    public async Task<ActionResult<ApiResult<OrderDto>>> Update(int id, [FromBody] OrderDto dto)
    {
        var entity = await orderRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<OrderDto>.Failure($"Order with id {id} not found."));
        var updated = orderMapper.ToEntity(dto);
        // Do not update the Id
        orderRepository.Update(updated);
        await orderRepository.SaveChangesAsync();
        var resultDto = orderMapper.ToDto(updated);
        return Ok(ApiResult<OrderDto>.Success(resultDto));
    }

    [HttpDelete("{id}")]
    [PermissionAuthorize(Permission.Order_Delete)]
    public async Task<ActionResult<ApiResult<bool>>> Delete(int id)
    {
        var entity = await orderRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<bool>.Failure($"Order with id {id} not found."));
        entity.IsDeleted = true;
        orderRepository.Update(entity);
        await orderRepository.SaveChangesAsync();
        return Ok(ApiResult<bool>.Success(true));
    }

    [HttpGet("my-orders")]
    [Authorize]
    public async Task<ActionResult<ApiResult<IEnumerable<OrderDto>>>> GetMyOrders()
    {
        var userId = authHelpers.GetCurrentUserId();
        if (userId == -1)
            return Unauthorized();

        var orders = await orderRepository.GetAll()
            .Where(o => o.UserId == userId && !o.IsDeleted)
            .ToListAsync();

        var dtos = orders.Select(orderMapper.ToDto).ToList();
        return Ok(ApiResult<IEnumerable<OrderDto>>.Success(dtos));
    }
} 