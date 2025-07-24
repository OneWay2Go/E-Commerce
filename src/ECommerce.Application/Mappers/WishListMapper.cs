using ECommerce.Domain.Entities;
using ECommerce.Application.Models.DTOs;
using Riok.Mapperly.Abstractions;

namespace ECommerce.Application.Mappers;

[Mapper]
public partial class WishListMapper
{
    public partial WishListDto ToDto(WishList wishList);
    public partial WishList ToEntity(WishListDto dto);
} 