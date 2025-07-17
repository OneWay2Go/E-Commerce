using ECommerce.Application.Interfaces;
using ECommerce.Application.Models.DTOs;
using ECommerce.Application.Mappers;
using ECommerce.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CouponController(
    ICouponRepository couponRepository,
    CouponMapper couponMapper
) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ApiResult<CouponDto>>> Create([FromBody] CouponDto dto)
    {
        var entity = couponMapper.ToEntity(dto);
        await couponRepository.AddAsync(entity);
        await couponRepository.SaveChangesAsync();
        var resultDto = couponMapper.ToDto(entity);
        return Ok(ApiResult<CouponDto>.Success(resultDto));
    }

    [HttpGet]
    public ActionResult<ApiResult<IEnumerable<CouponDto>>> GetAll()
    {
        var entities = couponRepository.GetAll().ToList();
        var dtos = entities.Select(couponMapper.ToDto).ToList();
        return Ok(ApiResult<IEnumerable<CouponDto>>.Success(dtos));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResult<CouponDto>>> GetById(int id)
    {
        var entity = await couponRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<CouponDto>.Failure($"Coupon with id {id} not found."));
        var dto = couponMapper.ToDto(entity);
        return Ok(ApiResult<CouponDto>.Success(dto));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResult<CouponDto>>> Update(int id, [FromBody] CouponDto dto)
    {
        var entity = await couponRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<CouponDto>.Failure($"Coupon with id {id} not found."));
        var updated = couponMapper.ToEntity(dto);
        // Do not update the Id
        couponRepository.Update(updated);
        await couponRepository.SaveChangesAsync();
        var resultDto = couponMapper.ToDto(updated);
        return Ok(ApiResult<CouponDto>.Success(resultDto));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResult<bool>>> Delete(int id)
    {
        var entity = await couponRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<bool>.Failure($"Coupon with id {id} not found."));
        entity.IsDeleted = true;
        couponRepository.Update(entity);
        await couponRepository.SaveChangesAsync();
        return Ok(ApiResult<bool>.Success(true));
    }
} 