using System;
using System.Collections.Generic;
using System.Text;

using FleetFlow.Domain.Security;

namespace FleetFlow.Application.Authentication;

public sealed class UserSession
{
    private readonly HashSet<string> _permissionCodes;

    public UserSession(
        AppUser user,
        IReadOnlyCollection<Role> roles,
        IEnumerable<string> permissionCodes)
    {
        User = user;
        Roles = roles;

        _permissionCodes = new HashSet<string>(
            permissionCodes,
            StringComparer.OrdinalIgnoreCase);
    }

    public AppUser User { get; }

    public IReadOnlyCollection<Role> Roles { get; }

    public IReadOnlySet<string> PermissionCodes => _permissionCodes;

    public bool HasPermission(string permissionCode)
    {
        return _permissionCodes.Contains(permissionCode);
    }
}
