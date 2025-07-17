using ECommerce.Application.Interfaces;
using ECommerce.Application.Models.DTOs;
using ECommerce.Application.Mappers;
using ECommerce.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(
    IUserRepository userRepository,
    UserMapper userMapper
) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ApiResult<UserDto>>> Create([FromBody] UserDto dto)
    {
        var entity = userMapper.ToEntity(dto);
        await userRepository.AddAsync(entity);
        await userRepository.SaveChangesAsync();
        var resultDto = userMapper.ToDto(entity);
        return Ok(ApiResult<UserDto>.Success(resultDto));
    }

    [HttpGet]
    public ActionResult<ApiResult<IEnumerable<UserDto>>> GetAll()
    {
        var entities = userRepository.GetAll().ToList();
        var dtos = entities.Select(userMapper.ToDto).ToList();
        return Ok(ApiResult<IEnumerable<UserDto>>.Success(dtos));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResult<UserDto>>> GetById(int id)
    {
        var entity = await userRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<UserDto>.Failure($"User with id {id} not found."));
        var dto = userMapper.ToDto(entity);
        return Ok(ApiResult<UserDto>.Success(dto));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResult<UserDto>>> Update(int id, [FromBody] UserDto dto)
    {
        var entity = await userRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<UserDto>.Failure($"User with id {id} not found."));
        var updated = userMapper.ToEntity(dto);
        // Do not update the Id
        userRepository.Update(updated);
        await userRepository.SaveChangesAsync();
        var resultDto = userMapper.ToDto(updated);
        return Ok(ApiResult<UserDto>.Success(resultDto));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResult<bool>>> Delete(int id)
    {
        var entity = await userRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<bool>.Failure($"User with id {id} not found."));
        entity.IsDeleted = true;
        userRepository.Update(entity);
        await userRepository.SaveChangesAsync();
        return Ok(ApiResult<bool>.Success(true));
    }
} 