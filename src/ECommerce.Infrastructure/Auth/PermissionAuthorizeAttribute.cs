using ECommerce.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace ECommerce.Infrastructure.Auth;

public class PermissionAuthorizeAttribute : AuthorizeAttribute
{
    public PermissionAuthorizeAttribute(Permission permission)
        : base(policy: permission.ToString())
    {
    }
}
