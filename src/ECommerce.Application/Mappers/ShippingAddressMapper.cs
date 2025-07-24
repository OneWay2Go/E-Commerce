using ECommerce.Domain.Entities;
using ECommerce.Application.Models.DTOs;
using Riok.Mapperly.Abstractions;

namespace ECommerce.Application.Mappers;

[Mapper]
public partial class ShippingAddressMapper
{
    public partial ShippingAddressDto ToDto(ShippingAddress address);
    public partial ShippingAddress ToEntity(ShippingAddressDto dto);
} 