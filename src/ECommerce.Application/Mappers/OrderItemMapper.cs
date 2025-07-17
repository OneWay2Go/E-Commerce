using ECommerce.Domain.Entities;
using ECommerce.Application.Models.DTOs;
using Riok.Mapperly.Abstractions;

namespace ECommerce.Application.Mappers;

[Mapper]
public partial class OrderItemMapper
{
    public partial OrderItemDto ToDto(OrderItem orderItem);
    public partial OrderItem ToEntity(OrderItemDto dto);
} 