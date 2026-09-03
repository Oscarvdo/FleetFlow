namespace FleetFlow.Application.Tracking;

/// <summary>
/// Estado independiente de un camión durante una simulación.
/// Cada Task de camión será propietario de una instancia.
/// </summary>
public sealed class TruckSimulationState
{
    public TruckSimulationState(
        LiveTrackingSimulationCandidate candidate,
        IReadOnlyList<LiveTrackingRoutePoint> route,
        decimal initialFuelPercent,
        decimal initialOdometerMiles)
    {
        ArgumentNullException.ThrowIfNull(candidate);
        ArgumentNullException.ThrowIfNull(route);

        if (route.Count < 2)
        {
            throw new ArgumentException(
                "A simulated truck requires at least two route points.",
                nameof(route));
        }

        if (initialFuelPercent is < 0M or > 100M)
        {
            throw new ArgumentOutOfRangeException(
                nameof(initialFuelPercent),
                "Initial fuel must be between 0 and 100.");
        }

        if (initialOdometerMiles < 0M)
        {
            throw new ArgumentOutOfRangeException(
                nameof(initialOdometerMiles),
                "Initial odometer cannot be negative.");
        }

        Candidate = candidate;

        Route = route
            .OrderBy(point => point.PointSequence)
            .ToArray();

        FuelPercent = initialFuelPercent;
        OdometerMiles = initialOdometerMiles;

        CurrentPointIndex = 0;
        SequenceNumber = 0;
        SimulatedElapsedSeconds = 0;
        TrackingStatus = "STOPPED";

        Latitude = Route[0].Latitude;
        Longitude = Route[0].Longitude;
        HeadingDegrees = CalculateHeading(
            Route[0],
            Route[1]);
    }

    public LiveTrackingSimulationCandidate Candidate { get; }

    public IReadOnlyList<LiveTrackingRoutePoint> Route { get; }

    public int CurrentPointIndex { get; private set; }

    public long SequenceNumber { get; private set; }

    public decimal SimulatedElapsedSeconds { get; private set; }

    public decimal Latitude { get; private set; }

    public decimal Longitude { get; private set; }

    public decimal SpeedMph { get; private set; }

    public decimal FuelPercent { get; private set; }

    public decimal OdometerMiles { get; private set; }

    public decimal HeadingDegrees { get; private set; }

    public string TrackingStatus { get; private set; }

    public bool IsCompleted { get; private set; }

    public LiveTrackingRoutePoint CurrentPoint =>
        Route[CurrentPointIndex];

    public LiveTrackingRoutePoint? NextPoint =>
        CurrentPointIndex + 1 < Route.Count
            ? Route[CurrentPointIndex + 1]
            : null;

    public void SetPosition(
        int currentPointIndex,
        decimal simulatedElapsedSeconds,
        decimal latitude,
        decimal longitude,
        decimal speedMph,
        decimal fuelPercent,
        decimal odometerMiles,
        decimal headingDegrees,
        string trackingStatus,
        bool isCompleted)
    {
        if (currentPointIndex < 0 ||
            currentPointIndex >= Route.Count)
        {
            throw new ArgumentOutOfRangeException(
                nameof(currentPointIndex));
        }

        CurrentPointIndex = currentPointIndex;
        SimulatedElapsedSeconds = simulatedElapsedSeconds;
        Latitude = latitude;
        Longitude = longitude;
        SpeedMph = speedMph;
        FuelPercent = Math.Clamp(
            fuelPercent,
            0M,
            100M);
        OdometerMiles = Math.Max(
            0M,
            odometerMiles);
        HeadingDegrees = NormalizeHeading(
            headingDegrees);
        TrackingStatus = trackingStatus;
        IsCompleted = isCompleted;
    }

    public long NextSequenceNumber()
    {
        SequenceNumber++;
        return SequenceNumber;
    }

    public VehicleTelemetryUpdate CreateTelemetry(
        long simulationRunId,
        DateTime recordedAtUtc)
    {
        if (simulationRunId <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(simulationRunId));
        }

        return new VehicleTelemetryUpdate
        {
            TelemetryId = Guid.NewGuid(),
            VehicleId = Candidate.VehicleId,
            TripId = Candidate.TripId,
            RecordedAtUtc = EnsureUtc(recordedAtUtc),
            SequenceNumber = NextSequenceNumber(),
            Latitude = Latitude,
            Longitude = Longitude,
            SpeedMph = SpeedMph,
            FuelPercent = FuelPercent,
            OdometerMiles = OdometerMiles,
            HeadingDegrees = HeadingDegrees,
            DataOriginId = 3,
            SimulationRunId = simulationRunId
        };
    }

    private static decimal CalculateHeading(
        LiveTrackingRoutePoint origin,
        LiveTrackingRoutePoint destination)
    {
        double originLatitude =
            DegreesToRadians((double)origin.Latitude);

        double destinationLatitude =
            DegreesToRadians((double)destination.Latitude);

        double longitudeDifference =
            DegreesToRadians(
                (double)(
                    destination.Longitude -
                    origin.Longitude));

        double y =
            Math.Sin(longitudeDifference) *
            Math.Cos(destinationLatitude);

        double x =
            Math.Cos(originLatitude) *
            Math.Sin(destinationLatitude)
            -
            Math.Sin(originLatitude) *
            Math.Cos(destinationLatitude) *
            Math.Cos(longitudeDifference);

        double heading =
            RadiansToDegrees(Math.Atan2(y, x));

        return NormalizeHeading((decimal)heading);
    }

    private static decimal NormalizeHeading(decimal heading)
    {
        decimal normalized = heading % 360M;

        if (normalized < 0M)
        {
            normalized += 360M;
        }

        return Math.Round(normalized, 2);
    }

    private static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180D;
    }

    private static double RadiansToDegrees(double radians)
    {
        return radians * 180D / Math.PI;
    }

    private static DateTime EnsureUtc(DateTime value)
    {
        return value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(
                value,
                DateTimeKind.Utc)
        };
    }
}