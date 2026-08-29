using System;
using System.Collections.Generic;
using System.Text;

namespace FleetFlow.Application.Dashboard
{
    public sealed class DashboardSummary
    {
        public int ActiveTrips { get; init; }

        public int AvailableDrivers { get; init; }

        public int AvailableVehicles { get; init; }

        public int PendingLoads { get; init; }

        public int DelayedTrips { get; init; }

        public int ActiveIncidents { get; init; }

        public int TrackedVehicles { get; init; }

        public DateTime GeneratedAtUtc { get; init; }
    }
}
