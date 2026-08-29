using System;
using System.Collections.Generic;
using System.Text;

namespace FleetFlow.Infrastructure.Security
{
    internal sealed class LoginUserRecord
    {
        public long AppUserId { get; set; }

        public string Username { get; set; } = string.Empty;

        public string NormalizedUsername { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public Guid SecurityStamp { get; set; }

        public long? DriverId { get; set; }

        public bool IsActive { get; set; }

        public bool MustChangePassword { get; set; }

        public short FailedLoginAttempts { get; set; }

        public DateTime? LockoutEndUtc { get; set; }

        public DateTime? LastLoginAtUtc { get; set; }

        public byte[] RowVersion { get; set; } = [];
    }
}
