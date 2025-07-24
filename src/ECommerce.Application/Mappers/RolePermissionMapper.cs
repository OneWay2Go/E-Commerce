using ECommerce.Domain.Entities.Auth;
using ECommerce.Application.Models.DTOs;
using Riok.Mapperly.Abstractions;

namespace ECommerce.Application.Mappers;

[Mapper]
public partial class RolePermissionMapper
{
    public partial RolePermissionDto ToDto(RolePermission rolePermission);
    public partial RolePermission ToEntity(RolePermissionDto dto);
} 