namespace FleetFlow.Application.Tracking;

/// <summary>
/// Calcula el movimiento progresivo de un camión entre
/// los puntos que componen la ruta de un viaje.
/// </summary>
public sealed class TruckRouteInterpolator
{
    private const decimal DefaultMilesPerGallon = 6.5M;
    private const decimal MaximumSimulatedSpeedMph = 75M;

    public VehicleTelemetryUpdate Advance(
        TruckSimulationState state,
        long simulationRunId,
        TimeSpan realElapsed,
        decimal timeScale,
        DateTime recordedAtUtc)
    {
        ArgumentNullException.ThrowIfNull(state);

        if (simulationRunId <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(simulationRunId));
        }

        if (realElapsed <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(realElapsed),
                "Elapsed time must be greater than zero.");
        }

        if (timeScale <= 0M)
        {
            throw new ArgumentOutOfRangeException(
                nameof(timeScale),
                "Time scale must be greater than zero.");
        }

        if (state.IsCompleted)
        {
            return state.CreateTelemetry(
                simulationRunId,
                recordedAtUtc);
        }

        decimal previousElapsedSeconds =
            state.SimulatedElapsedSeconds;

        decimal simulatedSeconds =
            (decimal)realElapsed.TotalSeconds * timeScale;

        decimal nextElapsedSeconds =
            previousElapsedSeconds + simulatedSeconds;

        IReadOnlyList<LiveTrackingRoutePoint> route =
            state.Route;

        LiveTrackingRoutePoint finalPoint =
            route[route.Count - 1];

        bool completed =
            nextElapsedSeconds >=
            finalPoint.ExpectedOffsetSeconds;

        if (completed)
        {
            decimal previousDistance =
                CalculateDistanceAtTime(
                    route,
                    previousElapsedSeconds);

            decimal distanceTravelled =
                Math.Max(
                    0M,
                    finalPoint.CumulativeDistanceMiles -
                    previousDistance);

            decimal nextFuel =
                CalculateRemainingFuel(
                    state.FuelPercent,
                    distanceTravelled);

            state.SetPosition(
                currentPointIndex: route.Count - 1,
                simulatedElapsedSeconds:
                    finalPoint.ExpectedOffsetSeconds,
                latitude:
                    finalPoint.Latitude,
                longitude:
                    finalPoint.Longitude,
                speedMph:
                    0M,
                fuelPercent:
                    nextFuel,
                odometerMiles:
                    state.OdometerMiles + distanceTravelled,
                headingDegrees:
                    state.HeadingDegrees,
                trackingStatus:
                    "STOPPED",
                isCompleted:
                    true);

            return state.CreateTelemetry(
                simulationRunId,
                recordedAtUtc);
        }

        int segmentIndex =
            FindSegmentIndex(
                route,
                nextElapsedSeconds);

        LiveTrackingRoutePoint origin =
            route[segmentIndex];

        LiveTrackingRoutePoint destination =
            route[segmentIndex + 1];

        decimal segmentDurationSeconds =
            destination.ExpectedOffsetSeconds -
            origin.ExpectedOffsetSeconds;

        decimal segmentProgress;

        if (segmentDurationSeconds <= 0M)
        {
            segmentProgress = 1M;
        }
        else
        {
            segmentProgress =
                (
                    nextElapsedSeconds -
                    origin.ExpectedOffsetSeconds
                )
                /
                segmentDurationSeconds;
        }

        segmentProgress =
            Math.Clamp(
                segmentProgress,
                0M,
                1M);

        decimal latitude =
            Interpolate(
                origin.Latitude,
                destination.Latitude,
                segmentProgress);

        decimal longitude =
            Interpolate(
                origin.Longitude,
                destination.Longitude,
                segmentProgress);

        decimal currentDistance =
            Interpolate(
                origin.CumulativeDistanceMiles,
                destination.CumulativeDistanceMiles,
                segmentProgress);

        decimal previousDistanceAtTime =
            CalculateDistanceAtTime(
                route,
                previousElapsedSeconds);

        decimal distanceSinceLastUpdate =
            Math.Max(
                0M,
                currentDistance -
                previousDistanceAtTime);

        decimal speedMph =
            CalculateSpeed(
                origin,
                destination);

        decimal fuelPercent =
            CalculateRemainingFuel(
                state.FuelPercent,
                distanceSinceLastUpdate);

        decimal heading =
            CalculateHeading(
                origin.Latitude,
                origin.Longitude,
                destination.Latitude,
                destination.Longitude);

        state.SetPosition(
            currentPointIndex:
                segmentIndex,
            simulatedElapsedSeconds:
                nextElapsedSeconds,
            latitude:
                latitude,
            longitude:
                longitude,
            speedMph:
                speedMph,
            fuelPercent:
                fuelPercent,
            odometerMiles:
                state.OdometerMiles +
                distanceSinceLastUpdate,
            headingDegrees:
                heading,
            trackingStatus:
                speedMph >= 1M
                    ? "MOVING"
                    : "STOPPED",
            isCompleted:
                false);

        return state.CreateTelemetry(
            simulationRunId,
            recordedAtUtc);
    }

    private static int FindSegmentIndex(
        IReadOnlyList<LiveTrackingRoutePoint> route,
        decimal elapsedSeconds)
    {
        for (int index = 0;
             index < route.Count - 1;
             index++)
        {
            LiveTrackingRoutePoint destination =
                route[index + 1];

            if (elapsedSeconds <
                destination.ExpectedOffsetSeconds)
            {
                return index;
            }
        }

        return route.Count - 2;
    }

    private static decimal CalculateDistanceAtTime(
        IReadOnlyList<LiveTrackingRoutePoint> route,
        decimal elapsedSeconds)
    {
        if (elapsedSeconds <=
            route[0].ExpectedOffsetSeconds)
        {
            return route[0].CumulativeDistanceMiles;
        }

        LiveTrackingRoutePoint finalPoint =
            route[route.Count - 1];

        if (elapsedSeconds >=
            finalPoint.ExpectedOffsetSeconds)
        {
            return finalPoint.CumulativeDistanceMiles;
        }

        int segmentIndex =
            FindSegmentIndex(
                route,
                elapsedSeconds);

        LiveTrackingRoutePoint origin =
            route[segmentIndex];

        LiveTrackingRoutePoint destination =
            route[segmentIndex + 1];

        decimal duration =
            destination.ExpectedOffsetSeconds -
            origin.ExpectedOffsetSeconds;

        if (duration <= 0M)
        {
            return destination.CumulativeDistanceMiles;
        }

        decimal progress =
            (
                elapsedSeconds -
                origin.ExpectedOffsetSeconds
            )
            /
            duration;

        progress =
            Math.Clamp(
                progress,
                0M,
                1M);

        return Interpolate(
            origin.CumulativeDistanceMiles,
            destination.CumulativeDistanceMiles,
            progress);
    }

    private static decimal CalculateSpeed(
        LiveTrackingRoutePoint origin,
        LiveTrackingRoutePoint destination)
    {
        decimal distanceMiles =
            destination.CumulativeDistanceMiles -
            origin.CumulativeDistanceMiles;

        decimal durationSeconds =
            destination.ExpectedOffsetSeconds -
            origin.ExpectedOffsetSeconds;

        if (distanceMiles <= 0M ||
            durationSeconds <= 0M)
        {
            return 0M;
        }

        decimal speedMph =
            distanceMiles /
            durationSeconds *
            3600M;

        return Math.Round(
            Math.Clamp(
                speedMph,
                0M,
                MaximumSimulatedSpeedMph),
            2);
    }

    private static decimal CalculateRemainingFuel(
        decimal currentFuelPercent,
        decimal distanceMiles)
    {
        if (distanceMiles <= 0M)
        {
            return currentFuelPercent;
        }

        /*
            For the initial simulation we treat 100 percent
            as approximately 100 gallons. This can later use
            the actual fuel-tank capacity of each vehicle.
        */
        decimal gallonsUsed =
            distanceMiles /
            DefaultMilesPerGallon;

        decimal fuelUsedPercent =
            gallonsUsed;

        return Math.Round(
            Math.Max(
                0M,
                currentFuelPercent -
                fuelUsedPercent),
            2);
    }

    private static decimal Interpolate(
        decimal start,
        decimal end,
        decimal progress)
    {
        return start +
            ((end - start) * progress);
    }

    private static decimal CalculateHeading(
        decimal originLatitude,
        decimal originLongitude,
        decimal destinationLatitude,
        decimal destinationLongitude)
    {
        double latitude1 =
            DegreesToRadians(
                (double)originLatitude);

        double latitude2 =
            DegreesToRadians(
                (double)destinationLatitude);

        double longitudeDifference =
            DegreesToRadians(
                (double)(
                    destinationLongitude -
                    originLongitude));

        double y =
            Math.Sin(longitudeDifference) *
            Math.Cos(latitude2);

        double x =
            Math.Cos(latitude1) *
            Math.Sin(latitude2)
            -
            Math.Sin(latitude1) *
            Math.Cos(latitude2) *
            Math.Cos(longitudeDifference);

        double heading =
            Math.Atan2(y, x) *
            180D /
            Math.PI;

        decimal normalizedHeading =
            (decimal)heading % 360M;

        if (normalizedHeading < 0M)
        {
            normalizedHeading += 360M;
        }

        return Math.Round(
            normalizedHeading,
            2);
    }

    private static double DegreesToRadians(
        double degrees)
    {
        return degrees *
            Math.PI /
            180D;
    }
}