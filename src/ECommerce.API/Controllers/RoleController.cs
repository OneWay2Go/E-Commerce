using ECommerce.Application.Interfaces;
using ECommerce.Application.Models.DTOs;
using ECommerce.Application.Mappers;
using ECommerce.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoleController(
    IRoleRepository roleRepository,
    RoleMapper roleMapper
) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ApiResult<RoleDto>>> Create([FromBody] RoleDto dto)
    {
        var entity = roleMapper.ToEntity(dto);
        await roleRepository.AddAsync(entity);
        await roleRepository.SaveChangesAsync();
        var resultDto = roleMapper.ToDto(entity);
        return Ok(ApiResult<RoleDto>.Success(resultDto));
    }

    [HttpGet]
    public ActionResult<ApiResult<IEnumerable<RoleDto>>> GetAll()
    {
        var entities = roleRepository.GetAll().ToList();
        var dtos = entities.Select(roleMapper.ToDto).ToList();
        return Ok(ApiResult<IEnumerable<RoleDto>>.Success(dtos));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResult<RoleDto>>> GetById(int id)
    {
        var entity = await roleRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<RoleDto>.Failure($"Role with id {id} not found."));
        var dto = roleMapper.ToDto(entity);
        return Ok(ApiResult<RoleDto>.Success(dto));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResult<RoleDto>>> Update(int id, [FromBody] RoleDto dto)
    {
        var entity = await roleRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<RoleDto>.Failure($"Role with id {id} not found."));
        var updated = roleMapper.ToEntity(dto);
        // Do not update the Id
        roleRepository.Update(updated);
        await roleRepository.SaveChangesAsync();
        var resultDto = roleMapper.ToDto(updated);
        return Ok(ApiResult<RoleDto>.Success(resultDto));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResult<bool>>> Delete(int id)
    {
        var entity = await roleRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<bool>.Failure($"Role with id {id} not found."));
        entity.IsDeleted = true;
        roleRepository.Update(entity);
        await roleRepository.SaveChangesAsync();
        return Ok(ApiResult<bool>.Success(true));
    }
} 