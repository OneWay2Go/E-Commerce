using ECommerce.Domain.Entities.Auth;
using ECommerce.Application.Models.DTOs;
using Riok.Mapperly.Abstractions;

namespace ECommerce.Application.Mappers;

[Mapper]
public partial class PermissionMapper
{
    public partial PermissionDto ToDto(Permission permission);
    public partial Permission ToEntity(PermissionDto dto);
} 