using ECommerce.Domain.Entities.Auth;
using ECommerce.Application.Models.DTOs;
using Riok.Mapperly.Abstractions;

namespace ECommerce.Application.Mappers;

[Mapper]
public partial class UserRoleMapper
{
    public partial UserRoleDto ToDto(UserRole userRole);
    public partial UserRole ToEntity(UserRoleDto dto);
} 