using System;
using System.Collections.Generic;
using System.Text;

namespace FleetFlow.Domain.Security
{
    public sealed class Permission
    {
        public short PermissionId { get; init; }

        public required string Code { get; init; }

        public required string DisplayName { get; init; }

        public required string Module { get; init; }

        public string? Description { get; init; }
    }
}
