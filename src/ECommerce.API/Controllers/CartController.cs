using ECommerce.Application.Interfaces;
using ECommerce.Application.Models.DTOs;
using ECommerce.Application.Mappers;
using ECommerce.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartController(
    ICartRepository cartRepository,
    CartMapper cartMapper
) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ApiResult<CartDto>>> Create([FromBody] CartDto dto)
    {
        var entity = cartMapper.ToEntity(dto);
        await cartRepository.AddAsync(entity);
        await cartRepository.SaveChangesAsync();
        var resultDto = cartMapper.ToDto(entity);
        return Ok(ApiResult<CartDto>.Success(resultDto));
    }

    [HttpGet]
    public ActionResult<ApiResult<IEnumerable<CartDto>>> GetAll()
    {
        var entities = cartRepository.GetAll().ToList();
        var dtos = entities.Select(cartMapper.ToDto).ToList();
        return Ok(ApiResult<IEnumerable<CartDto>>.Success(dtos));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResult<CartDto>>> GetById(int id)
    {
        var entity = await cartRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<CartDto>.Failure($"Cart with id {id} not found."));
        var dto = cartMapper.ToDto(entity);
        return Ok(ApiResult<CartDto>.Success(dto));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResult<CartDto>>> Update(int id, [FromBody] CartDto dto)
    {
        var entity = await cartRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<CartDto>.Failure($"Cart with id {id} not found."));
        var updated = cartMapper.ToEntity(dto);
        // Do not update the Id
        cartRepository.Update(updated);
        await cartRepository.SaveChangesAsync();
        var resultDto = cartMapper.ToDto(updated);
        return Ok(ApiResult<CartDto>.Success(resultDto));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResult<bool>>> Delete(int id)
    {
        var entity = await cartRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<bool>.Failure($"Cart with id {id} not found."));
        entity.IsDeleted = true;
        cartRepository.Update(entity);
        await cartRepository.SaveChangesAsync();
        return Ok(ApiResult<bool>.Success(true));
    }
} 