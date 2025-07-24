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
public class PermissionController(
    IPermissionRepository permissionRepository,
    PermissionMapper permissionMapper
) : ControllerBase
{
    [HttpPost]
    [PermissionAuthorize(Permission.Permission_Create)]
    public async Task<ActionResult<ApiResult<PermissionDto>>> Create([FromBody] PermissionDto dto)
    {
        var entity = permissionMapper.ToEntity(dto);
        await permissionRepository.AddAsync(entity);
        await permissionRepository.SaveChangesAsync();
        var resultDto = permissionMapper.ToDto(entity);
        return Ok(ApiResult<PermissionDto>.Success(resultDto));
    }

    [HttpGet]
    [PermissionAuthorize(Permission.Permission_GetAll)]
    public ActionResult<ApiResult<IEnumerable<PermissionDto>>> GetAll()
    {
        var entities = permissionRepository.GetAll().ToList();
        var dtos = entities.Select(permissionMapper.ToDto).ToList();
        return Ok(ApiResult<IEnumerable<PermissionDto>>.Success(dtos));
    }

    [HttpGet("{id}")]
    [PermissionAuthorize(Permission.Permission_GetById)]
    public async Task<ActionResult<ApiResult<PermissionDto>>> GetById(int id)
    {
        var entity = await permissionRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<PermissionDto>.Failure($"Permission with id {id} not found."));
        var dto = permissionMapper.ToDto(entity);
        return Ok(ApiResult<PermissionDto>.Success(dto));
    }

    [HttpPut("{id}")]
    [PermissionAuthorize(Permission.Permission_Update)]
    public async Task<ActionResult<ApiResult<PermissionDto>>> Update(int id, [FromBody] PermissionDto dto)
    {
        var entity = await permissionRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<PermissionDto>.Failure($"Permission with id {id} not found."));
        var updated = permissionMapper.ToEntity(dto);
        // Do not update the Id
        permissionRepository.Update(updated);
        await permissionRepository.SaveChangesAsync();
        var resultDto = permissionMapper.ToDto(updated);
        return Ok(ApiResult<PermissionDto>.Success(resultDto));
    }

    [HttpDelete("{id}")]
    [PermissionAuthorize(Permission.Permission_Delete)]
    public async Task<ActionResult<ApiResult<bool>>> Delete(int id)
    {
        var entity = await permissionRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<bool>.Failure($"Permission with id {id} not found."));
        entity.IsDeleted = true;
        permissionRepository.Update(entity);
        await permissionRepository.SaveChangesAsync();
        return Ok(ApiResult<bool>.Success(true));
    }
} 