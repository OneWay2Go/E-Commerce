using ECommerce.Domain.Entities;
using ECommerce.Application.Models.DTOs;
using Riok.Mapperly.Abstractions;

namespace ECommerce.Application.Mappers;

[Mapper]
public partial class OrderMapper
{
    public partial OrderDto ToDto(Order order);
    public partial Order ToEntity(OrderDto dto);
} 