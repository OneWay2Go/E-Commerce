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
public class ReviewController(
    IReviewRepository reviewRepository,
    ReviewMapper reviewMapper
) : ControllerBase
{
    [HttpPost]
    [PermissionAuthorize(Permission.Review_Create)]
    public async Task<ActionResult<ApiResult<ReviewDto>>> Create([FromBody] ReviewDto dto)
    {
        var entity = reviewMapper.ToEntity(dto);
        await reviewRepository.AddAsync(entity);
        await reviewRepository.SaveChangesAsync();
        var resultDto = reviewMapper.ToDto(entity);
        return Ok(ApiResult<ReviewDto>.Success(resultDto));
    }

    [HttpGet]
    [PermissionAuthorize(Permission.Review_GetAll)]
    public ActionResult<ApiResult<IEnumerable<ReviewDto>>> GetAll()
    {
        var entities = reviewRepository.GetAll().ToList();
        var dtos = entities.Select(reviewMapper.ToDto).ToList();
        return Ok(ApiResult<IEnumerable<ReviewDto>>.Success(dtos));
    }

    [HttpGet("{id}")]
    [PermissionAuthorize(Permission.Review_GetById)]
    public async Task<ActionResult<ApiResult<ReviewDto>>> GetById(int id)
    {
        var entity = await reviewRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<ReviewDto>.Failure($"Review with id {id} not found."));
        var dto = reviewMapper.ToDto(entity);
        return Ok(ApiResult<ReviewDto>.Success(dto));
    }

    [HttpPut("{id}")]
    [PermissionAuthorize(Permission.Review_Update)]
    public async Task<ActionResult<ApiResult<ReviewDto>>> Update(int id, [FromBody] ReviewDto dto)
    {
        var entity = await reviewRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<ReviewDto>.Failure($"Review with id {id} not found."));
        var updated = reviewMapper.ToEntity(dto);
        // Do not update the Id
        reviewRepository.Update(updated);
        await reviewRepository.SaveChangesAsync();
        var resultDto = reviewMapper.ToDto(updated);
        return Ok(ApiResult<ReviewDto>.Success(resultDto));
    }

    [HttpDelete("{id}")]
    [PermissionAuthorize(Permission.Review_Delete)]
    public async Task<ActionResult<ApiResult<bool>>> Delete(int id)
    {
        var entity = await reviewRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<bool>.Failure($"Review with id {id} not found."));
        entity.IsDeleted = true;
        reviewRepository.Update(entity);
        await reviewRepository.SaveChangesAsync();
        return Ok(ApiResult<bool>.Success(true));
    }
} 