using ECommerce.Application.Interfaces;
using ECommerce.Application.Models.DTOs;
using ECommerce.Application.Mappers;
using ECommerce.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController(
    ICategoryRepository categoryRepository,
    CategoryMapper categoryMapper
) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ApiResult<CategoryDto>>> Create([FromBody] CategoryDto dto)
    {
        var entity = categoryMapper.ToEntity(dto);
        await categoryRepository.AddAsync(entity);
        await categoryRepository.SaveChangesAsync();
        var resultDto = categoryMapper.ToDto(entity);
        return Ok(ApiResult<CategoryDto>.Success(resultDto));
    }

    [HttpGet]
    public ActionResult<ApiResult<IEnumerable<CategoryDto>>> GetAll()
    {
        var entities = categoryRepository.GetAll().ToList();
        var dtos = entities.Select(categoryMapper.ToDto).ToList();
        return Ok(ApiResult<IEnumerable<CategoryDto>>.Success(dtos));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResult<CategoryDto>>> GetById(int id)
    {
        var entity = await categoryRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<CategoryDto>.Failure($"Category with id {id} not found."));
        var dto = categoryMapper.ToDto(entity);
        return Ok(ApiResult<CategoryDto>.Success(dto));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResult<CategoryDto>>> Update(int id, [FromBody] CategoryDto dto)
    {
        var entity = await categoryRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<CategoryDto>.Failure($"Category with id {id} not found."));
        var updated = categoryMapper.ToEntity(dto);
        // Do not update the Id
        categoryRepository.Update(updated);
        await categoryRepository.SaveChangesAsync();
        var resultDto = categoryMapper.ToDto(updated);
        return Ok(ApiResult<CategoryDto>.Success(resultDto));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResult<bool>>> Delete(int id)
    {
        var entity = await categoryRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<bool>.Failure($"Category with id {id} not found."));
        entity.IsDeleted = true;
        categoryRepository.Update(entity);
        await categoryRepository.SaveChangesAsync();
        return Ok(ApiResult<bool>.Success(true));
    }
} 