using ECommerce.Domain.Entities;
using ECommerce.Application.Models.DTOs;
using Riok.Mapperly.Abstractions;

namespace ECommerce.Application.Mappers;

[Mapper]
public partial class CartMapper
{
    public partial CartDto ToDto(Cart cart);
    public partial Cart ToEntity(CartDto dto);
} 