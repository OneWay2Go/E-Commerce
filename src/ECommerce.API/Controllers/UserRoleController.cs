using ECommerce.Application.Interfaces;
using ECommerce.Application.Models.DTOs;
using ECommerce.Application.Mappers;
using ECommerce.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserRoleController(
    IUserRoleRepository userRoleRepository,
    UserRoleMapper userRoleMapper
) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ApiResult<UserRoleDto>>> Create([FromBody] UserRoleDto dto)
    {
        var entity = userRoleMapper.ToEntity(dto);
        await userRoleRepository.AddAsync(entity);
        await userRoleRepository.SaveChangesAsync();
        var resultDto = userRoleMapper.ToDto(entity);
        return Ok(ApiResult<UserRoleDto>.Success(resultDto));
    }

    [HttpGet]
    public ActionResult<ApiResult<IEnumerable<UserRoleDto>>> GetAll()
    {
        var entities = userRoleRepository.GetAll().ToList();
        var dtos = entities.Select(userRoleMapper.ToDto).ToList();
        return Ok(ApiResult<IEnumerable<UserRoleDto>>.Success(dtos));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResult<UserRoleDto>>> GetById(int id)
    {
        var entity = await userRoleRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<UserRoleDto>.Failure($"UserRole with id {id} not found."));
        var dto = userRoleMapper.ToDto(entity);
        return Ok(ApiResult<UserRoleDto>.Success(dto));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResult<UserRoleDto>>> Update(int id, [FromBody] UserRoleDto dto)
    {
        var entity = await userRoleRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<UserRoleDto>.Failure($"UserRole with id {id} not found."));
        var updated = userRoleMapper.ToEntity(dto);
        // Do not update the Id
        userRoleRepository.Update(updated);
        await userRoleRepository.SaveChangesAsync();
        var resultDto = userRoleMapper.ToDto(updated);
        return Ok(ApiResult<UserRoleDto>.Success(resultDto));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResult<bool>>> Delete(int id)
    {
        var entity = await userRoleRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<bool>.Failure($"UserRole with id {id} not found."));
        // No IsDeleted property, so perform hard delete
        userRoleRepository.Delete(entity);
        await userRoleRepository.SaveChangesAsync();
        return Ok(ApiResult<bool>.Success(true));
    }
} 