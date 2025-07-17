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
public class RolePermissionController(
    IRolePermissionRepository rolePermissionRepository,
    RolePermissionMapper rolePermissionMapper
) : ControllerBase
{
    [HttpPost]
    [PermissionAuthorize(Permission.RolePermission_Create)]
    public async Task<ActionResult<ApiResult<RolePermissionDto>>> Create([FromBody] RolePermissionDto dto)
    {
        var entity = rolePermissionMapper.ToEntity(dto);
        await rolePermissionRepository.AddAsync(entity);
        await rolePermissionRepository.SaveChangesAsync();
        var resultDto = rolePermissionMapper.ToDto(entity);
        return Ok(ApiResult<RolePermissionDto>.Success(resultDto));
    }

    [HttpGet]
    [PermissionAuthorize(Permission.RolePermission_GetAll)]
    public ActionResult<ApiResult<IEnumerable<RolePermissionDto>>> GetAll()
    {
        var entities = rolePermissionRepository.GetAll().ToList();
        var dtos = entities.Select(rolePermissionMapper.ToDto).ToList();
        return Ok(ApiResult<IEnumerable<RolePermissionDto>>.Success(dtos));
    }

    [HttpGet("{id}")]
    [PermissionAuthorize(Permission.RolePermission_GetById)]
    public async Task<ActionResult<ApiResult<RolePermissionDto>>> GetById(int id)
    {
        var entity = await rolePermissionRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<RolePermissionDto>.Failure($"RolePermission with id {id} not found."));
        var dto = rolePermissionMapper.ToDto(entity);
        return Ok(ApiResult<RolePermissionDto>.Success(dto));
    }

    [HttpPut("{id}")]
    [PermissionAuthorize(Permission.RolePermission_Update)]
    public async Task<ActionResult<ApiResult<RolePermissionDto>>> Update(int id, [FromBody] RolePermissionDto dto)
    {
        var entity = await rolePermissionRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<RolePermissionDto>.Failure($"RolePermission with id {id} not found."));
        var updated = rolePermissionMapper.ToEntity(dto);
        // Do not update the Id
        rolePermissionRepository.Update(updated);
        await rolePermissionRepository.SaveChangesAsync();
        var resultDto = rolePermissionMapper.ToDto(updated);
        return Ok(ApiResult<RolePermissionDto>.Success(resultDto));
    }

    [HttpDelete("{id}")]
    [PermissionAuthorize(Permission.RolePermission_Delete)]
    public async Task<ActionResult<ApiResult<bool>>> Delete(int id)
    {
        var entity = await rolePermissionRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<bool>.Failure($"RolePermission with id {id} not found."));
        // No IsDeleted property, so perform hard delete
        rolePermissionRepository.Delete(entity);
        await rolePermissionRepository.SaveChangesAsync();
        return Ok(ApiResult<bool>.Success(true));
    }
} 