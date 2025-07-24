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
public class ShippingAddressController(
    IShippingAddressRepository shippingAddressRepository,
    ShippingAddressMapper shippingAddressMapper
) : ControllerBase
{
    [HttpPost]
    [PermissionAuthorize(Permission.ShippingAddress_Create)]
    public async Task<ActionResult<ApiResult<ShippingAddressDto>>> Create([FromBody] ShippingAddressDto dto)
    {
        var entity = shippingAddressMapper.ToEntity(dto);
        await shippingAddressRepository.AddAsync(entity);
        await shippingAddressRepository.SaveChangesAsync();
        var resultDto = shippingAddressMapper.ToDto(entity);
        return Ok(ApiResult<ShippingAddressDto>.Success(resultDto));
    }

    [HttpGet]
    [PermissionAuthorize(Permission.ShippingAddress_GetAll)]
    public ActionResult<ApiResult<IEnumerable<ShippingAddressDto>>> GetAll()
    {
        var entities = shippingAddressRepository.GetAll().ToList();
        var dtos = entities.Select(shippingAddressMapper.ToDto).ToList();
        return Ok(ApiResult<IEnumerable<ShippingAddressDto>>.Success(dtos));
    }

    [HttpGet("{id}")]
    [PermissionAuthorize(Permission.ShippingAddress_GetById)]
    public async Task<ActionResult<ApiResult<ShippingAddressDto>>> GetById(int id)
    {
        var entity = await shippingAddressRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<ShippingAddressDto>.Failure($"ShippingAddress with id {id} not found."));
        var dto = shippingAddressMapper.ToDto(entity);
        return Ok(ApiResult<ShippingAddressDto>.Success(dto));
    }

    [HttpPut("{id}")]
    [PermissionAuthorize(Permission.ShippingAddress_Update)]
    public async Task<ActionResult<ApiResult<ShippingAddressDto>>> Update(int id, [FromBody] ShippingAddressDto dto)
    {
        var entity = await shippingAddressRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<ShippingAddressDto>.Failure($"ShippingAddress with id {id} not found."));
        var updated = shippingAddressMapper.ToEntity(dto);
        // Do not update the Id
        shippingAddressRepository.Update(updated);
        await shippingAddressRepository.SaveChangesAsync();
        var resultDto = shippingAddressMapper.ToDto(updated);
        return Ok(ApiResult<ShippingAddressDto>.Success(resultDto));
    }

    [HttpDelete("{id}")]
    [PermissionAuthorize(Permission.ShippingAddress_Delete)]
    public async Task<ActionResult<ApiResult<bool>>> Delete(int id)
    {
        var entity = await shippingAddressRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<bool>.Failure($"ShippingAddress with id {id} not found."));
        entity.IsDeleted = true;
        shippingAddressRepository.Update(entity);
        await shippingAddressRepository.SaveChangesAsync();
        return Ok(ApiResult<bool>.Success(true));
    }
} 