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
public class ProductController(
    IProductRepository productRepository,
    ProductMapper productMapper
) : ControllerBase
{
    [HttpPost]
    [PermissionAuthorize(Permission.Product_Create)]
    public async Task<ActionResult<ApiResult<ProductDto>>> Create([FromBody] ProductDto dto)
    {
        var entity = productMapper.ToEntity(dto);
        await productRepository.AddAsync(entity);
        await productRepository.SaveChangesAsync();
        var resultDto = productMapper.ToDto(entity);
        return Ok(ApiResult<ProductDto>.Success(resultDto));
    }

    [HttpGet]
    [PermissionAuthorize(Permission.Product_GetAll)]
    public ActionResult<ApiResult<IEnumerable<ProductDto>>> GetAll()
    {
        var entities = productRepository.GetAll().ToList();
        var dtos = entities.Select(productMapper.ToDto).ToList();
        return Ok(ApiResult<IEnumerable<ProductDto>>.Success(dtos));
    }

    [HttpGet("{id}")]
    [PermissionAuthorize(Permission.Product_GetById)]
    public async Task<ActionResult<ApiResult<ProductDto>>> GetById(int id)
    {
        var entity = await productRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<ProductDto>.Failure($"Product with id {id} not found."));
        var dto = productMapper.ToDto(entity);
        return Ok(ApiResult<ProductDto>.Success(dto));
    }

    [HttpPut("{id}")]
    [PermissionAuthorize(Permission.Product_Update)]
    public async Task<ActionResult<ApiResult<ProductDto>>> Update(int id, [FromBody] ProductDto dto)
    {
        var entity = await productRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<ProductDto>.Failure($"Product with id {id} not found."));
        var updated = productMapper.ToEntity(dto);
        // Do not update the Id
        productRepository.Update(updated);
        await productRepository.SaveChangesAsync();
        var resultDto = productMapper.ToDto(updated);
        return Ok(ApiResult<ProductDto>.Success(resultDto));
    }

    [HttpDelete("{id}")]
    [PermissionAuthorize(Permission.Product_Delete)]
    public async Task<ActionResult<ApiResult<bool>>> Delete(int id)
    {
        var entity = await productRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<bool>.Failure($"Product with id {id} not found."));
        entity.IsDeleted = true;
        productRepository.Update(entity);
        await productRepository.SaveChangesAsync();
        return Ok(ApiResult<bool>.Success(true));
    }
} 