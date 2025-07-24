using ECommerce.Domain.Entities;
using ECommerce.Application.Models.DTOs;
using Riok.Mapperly.Abstractions;

namespace ECommerce.Application.Mappers;

[Mapper]
public partial class CouponMapper
{
    public partial CouponDto ToDto(Coupon coupon);
    public partial Coupon ToEntity(CouponDto dto);
} 