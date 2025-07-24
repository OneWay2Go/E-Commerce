using ECommerce.Domain.Entities.Auth;
using ECommerce.Application.Models.DTOs;
using Riok.Mapperly.Abstractions;

namespace ECommerce.Application.Mappers;

[Mapper]
public partial class UserMapper
{
    public partial UserDto ToDto(User user);
    public partial User ToEntity(UserDto dto);
} 