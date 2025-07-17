using ECommerce.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace ECommerce.Infrastructure.Auth;

public class PermissionRequirement : IAuthorizationRequirement
{
    public Permission RequiredPermission { get; }

    public PermissionRequirement(Permission permission)
    {
        RequiredPermission = permission;
    }
}
