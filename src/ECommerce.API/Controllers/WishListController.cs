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
public class WishListController(
    IWishListRepository wishListRepository,
    WishListMapper wishListMapper
) : ControllerBase
{
    [HttpPost]
    [PermissionAuthorize(Permission.WishList_Create)]
    public async Task<ActionResult<ApiResult<WishListDto>>> Create([FromBody] WishListDto dto)
    {
        var entity = wishListMapper.ToEntity(dto);
        await wishListRepository.AddAsync(entity);
        await wishListRepository.SaveChangesAsync();
        var resultDto = wishListMapper.ToDto(entity);
        return Ok(ApiResult<WishListDto>.Success(resultDto));
    }

    [HttpGet]
    [PermissionAuthorize(Permission.WishList_GetAll)]
    public ActionResult<ApiResult<IEnumerable<WishListDto>>> GetAll()
    {
        var entities = wishListRepository.GetAll().ToList();
        var dtos = entities.Select(wishListMapper.ToDto).ToList();
        return Ok(ApiResult<IEnumerable<WishListDto>>.Success(dtos));
    }

    [HttpGet("{id}")]
    [PermissionAuthorize(Permission.WishList_GetById)]
    public async Task<ActionResult<ApiResult<WishListDto>>> GetById(int id)
    {
        var entity = await wishListRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<WishListDto>.Failure($"WishList with id {id} not found."));
        var dto = wishListMapper.ToDto(entity);
        return Ok(ApiResult<WishListDto>.Success(dto));
    }

    [HttpPut("{id}")]
    [PermissionAuthorize(Permission.WishList_Update)]
    public async Task<ActionResult<ApiResult<WishListDto>>> Update(int id, [FromBody] WishListDto dto)
    {
        var entity = await wishListRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<WishListDto>.Failure($"WishList with id {id} not found."));
        var updated = wishListMapper.ToEntity(dto);
        // Do not update the Id
        wishListRepository.Update(updated);
        await wishListRepository.SaveChangesAsync();
        var resultDto = wishListMapper.ToDto(updated);
        return Ok(ApiResult<WishListDto>.Success(resultDto));
    }

    [HttpDelete("{id}")]
    [PermissionAuthorize(Permission.WishList_Delete)]
    public async Task<ActionResult<ApiResult<bool>>> Delete(int id)
    {
        var entity = await wishListRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<bool>.Failure($"WishList with id {id} not found."));
        entity.IsDeleted = true;
        wishListRepository.Update(entity);
        await wishListRepository.SaveChangesAsync();
        return Ok(ApiResult<bool>.Success(true));
    }
} 