using System;
using System.Collections.Generic;
using System.Text;

namespace FleetFlow.Domain.Security
{
    public sealed class Role
    {
        public short RoleId { get; init; }

        public required string Code { get; init; }

        public required string DisplayName { get; init; }

        public string? Description { get; init; }

        public bool IsSystemRole { get; init; }

        public bool IsActive { get; init; }
    }
}
