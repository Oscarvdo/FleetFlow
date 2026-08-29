using System;
using System.Collections.Generic;
using System.Text;

namespace FleetFlow.Domain.Security
{
    public sealed class AppUser
    {
        public long AppUserId { get; init; }

        public required string Username { get; init; }

        public required string Email { get; init; }

        public long? DriverId { get; init; }

        public bool IsActive { get; init; }

        public bool MustChangePassword { get; init; }

        public short FailedLoginAttempts { get; init; }

        public DateTime? LockoutEndUtc { get; init; }

        public DateTime? LastLoginAtUtc { get; init; }
    }
}
