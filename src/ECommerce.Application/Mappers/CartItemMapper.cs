using ECommerce.Domain.Entities;
using ECommerce.Application.Models.DTOs;
using Riok.Mapperly.Abstractions;

namespace ECommerce.Application.Mappers;

[Mapper]
public partial class CartItemMapper
{
    public partial CartItemDto ToDto(CartItem cartItem);
    public partial CartItem ToEntity(CartItemDto dto);
} 