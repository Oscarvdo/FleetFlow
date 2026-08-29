using System;
using System.Collections.Generic;
using System.Text;

namespace FleetFlow.Application.Dispatch
{
    public sealed class DispatchBoardItem
    {
        public long TripId { get; init; }

        public string TripNumber { get; init; } = string.Empty;

        public string TripStatusCode { get; init; } = string.Empty;

        public string TripStatus { get; init; } = string.Empty;

        public long LoadId { get; init; }

        public string LoadNumber { get; init; } = string.Empty;

        public long CustomerId { get; init; }

        public string Customer { get; init; } = string.Empty;

        public long? TripAssignmentId { get; init; }

        public string? AssignmentStatusCode { get; init; }

        public long? DriverId { get; init; }

        public string? DriverNumber { get; init; }

        public string? DriverName { get; init; }

        public long? VehicleId { get; init; }

        public string? VehicleUnitNumber { get; init; }

        public long? TrailerId { get; init; }

        public string? TrailerUnitNumber { get; init; }

        public DateTime ScheduledPickupUtc { get; init; }

        public DateTime ScheduledDeliveryUtc { get; init; }

        public string? PickupLocation { get; init; }

        public string? DeliveryLocation { get; init; }

        public DateTime? LastTelemetryUtc { get; init; }

        public decimal? Latitude { get; init; }

        public decimal? Longitude { get; init; }

        public decimal? SpeedMph { get; init; }

        public decimal? FuelPercent { get; init; }

        public decimal ProgressPercent { get; init; }

        public byte[] RowVersion { get; init; } = [];
    }
}
