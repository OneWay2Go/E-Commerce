using ECommerce.Domain.Entities.Auth;
using ECommerce.Application.Models.DTOs;
using Riok.Mapperly.Abstractions;

namespace ECommerce.Application.Mappers;

[Mapper]
public partial class RoleMapper
{
    public partial RoleDto ToDto(Role role);
    public partial Role ToEntity(RoleDto dto);
} 