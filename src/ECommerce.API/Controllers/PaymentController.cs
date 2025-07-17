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
public class PaymentController(
    IPaymentRepository paymentRepository,
    PaymentMapper paymentMapper
) : ControllerBase
{
    [HttpPost]
    [PermissionAuthorize(Permission.Payment_Create)]
    public async Task<ActionResult<ApiResult<PaymentDto>>> Create([FromBody] PaymentDto dto)
    {
        var entity = paymentMapper.ToEntity(dto);
        await paymentRepository.AddAsync(entity);
        await paymentRepository.SaveChangesAsync();
        var resultDto = paymentMapper.ToDto(entity);
        return Ok(ApiResult<PaymentDto>.Success(resultDto));
    }

    [HttpGet]
    [PermissionAuthorize(Permission.Payment_GetAll)]
    public ActionResult<ApiResult<IEnumerable<PaymentDto>>> GetAll()
    {
        var entities = paymentRepository.GetAll().ToList();
        var dtos = entities.Select(paymentMapper.ToDto).ToList();
        return Ok(ApiResult<IEnumerable<PaymentDto>>.Success(dtos));
    }

    [HttpGet("{id}")]
    [PermissionAuthorize(Permission.Payment_GetById)]
    public async Task<ActionResult<ApiResult<PaymentDto>>> GetById(int id)
    {
        var entity = await paymentRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<PaymentDto>.Failure($"Payment with id {id} not found."));
        var dto = paymentMapper.ToDto(entity);
        return Ok(ApiResult<PaymentDto>.Success(dto));
    }

    [HttpPut("{id}")]
    [PermissionAuthorize(Permission.Payment_Update)]
    public async Task<ActionResult<ApiResult<PaymentDto>>> Update(int id, [FromBody] PaymentDto dto)
    {
        var entity = await paymentRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<PaymentDto>.Failure($"Payment with id {id} not found."));
        var updated = paymentMapper.ToEntity(dto);
        // Do not update the Id
        paymentRepository.Update(updated);
        await paymentRepository.SaveChangesAsync();
        var resultDto = paymentMapper.ToDto(updated);
        return Ok(ApiResult<PaymentDto>.Success(resultDto));
    }

    [HttpDelete("{id}")]
    [PermissionAuthorize(Permission.Payment_Delete)]
    public async Task<ActionResult<ApiResult<bool>>> Delete(int id)
    {
        var entity = await paymentRepository.GetByIdAsync(id);
        if (entity == null)
            return NotFound(ApiResult<bool>.Failure($"Payment with id {id} not found."));
        // No IsDeleted property, so perform hard delete
        paymentRepository.Delete(entity);
        await paymentRepository.SaveChangesAsync();
        return Ok(ApiResult<bool>.Success(true));
    }
} 