using System;
using System.Collections.Generic;
using System.Text;

namespace FleetFlow.Infrastructure.Security
{
    internal sealed class EffectivePermissionRecord
    {
        public long AppUserId { get; set; }

        public string Username { get; set; } = string.Empty;

        public short RoleId { get; set; }

        public string RoleCode { get; set; } = string.Empty;

        public short PermissionId { get; set; }

        public string PermissionCode { get; set; } = string.Empty;

        public string Module { get; set; } = string.Empty;
    }
}
