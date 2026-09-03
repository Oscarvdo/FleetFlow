/*
    FleetFlow Live Tracking API

    Prerequisites:
        001_create_database.sql
        005_create_database_api_schemas.sql
        006_create_table_types_and_functions.sql
        007_create_views.sql
        008_create_stored_procedures.sql

    This script does not create duplicate tracking tables.
    It exposes the existing telemetry, route and simulation
    structures to the application.
*/

USE FleetFlowDb;
GO

/* ============================================================
   1. Current state used by the Live Tracking map
   ============================================================ */

CREATE OR ALTER PROCEDURE tracking.LiveTracking_GetMapState
    @IncludeOffline bit = 1,
    @OfflineAfterSeconds int = 60,
    @SimulationRunId bigint = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @OfflineAfterSeconds < 5
    BEGIN
        THROW 51080,
            'OfflineAfterSeconds must be at least 5 seconds.',
            1;
    END;

    SELECT
        vehicle.VehicleId,
        vehicle.UnitNumber,
        vehicle.Make,
        vehicle.Model,

        assignment.TripAssignmentId,
        assignment.TripId,
        trip.TripNumber,
        tripStatus.Code AS TripStatusCode,
        tripStatus.DisplayName AS TripStatus,

        load.LoadId,
        load.LoadNumber,
        customer.CustomerId,
        customer.CompanyName AS CustomerName,

        assignment.DriverId,
        driver.DriverNumber,
        CONCAT(driver.FirstName, N' ', driver.LastName)
            AS DriverName,

        assignment.TrailerId,
        trailer.UnitNumber AS TrailerUnitNumber,

        currentState.RecordedAtUtc,
        currentState.Latitude,
        currentState.Longitude,
        currentState.SpeedMph,
        currentState.FuelPercent,
        currentState.OdometerMiles,
        currentState.HeadingDegrees,
        currentState.SimulationRunId,

        CASE
            WHEN currentState.RecordedAtUtc IS NULL
                THEN NULL
            ELSE DATEDIFF(
                second,
                currentState.RecordedAtUtc,
                SYSUTCDATETIME())
        END AS TelemetryAgeSeconds,

        CASE
            WHEN currentState.RecordedAtUtc IS NULL
                THEN 'OFFLINE'

            WHEN DATEDIFF(
                    second,
                    currentState.RecordedAtUtc,
                    SYSUTCDATETIME()) > @OfflineAfterSeconds
                THEN 'OFFLINE'

            WHEN currentState.TripId IS NULL
                 AND ISNULL(currentState.SpeedMph, 0) < 1
                THEN 'IDLE'

            WHEN ISNULL(currentState.SpeedMph, 0) >= 1
                THEN 'MOVING'

            ELSE 'STOPPED'
        END AS TrackingStatus,

        nextStop.TripStopId AS NextTripStopId,
        nextStop.StopSequence AS NextStopSequence,
        nextStop.StopTypeCode AS NextStopType,
        nextStop.LocationId AS NextLocationId,
        nextStop.LocationName AS NextStopName,
        nextStop.Latitude AS NextStopLatitude,
        nextStop.Longitude AS NextStopLongitude,
        nextStop.ScheduledArrivalUtc AS NextScheduledArrivalUtc,

        routeProgress.PointSequence AS NearestRoutePointSequence,
        routeProgress.CumulativeDistanceMiles,

        CAST(
            CASE
                WHEN trip.PlannedDistanceMiles IS NULL
                     OR trip.PlannedDistanceMiles <= 0
                     OR routeProgress.CumulativeDistanceMiles IS NULL
                    THEN 0

                WHEN routeProgress.CumulativeDistanceMiles
                     >= trip.PlannedDistanceMiles
                    THEN 100

                ELSE
                    routeProgress.CumulativeDistanceMiles
                    * 100.0
                    / trip.PlannedDistanceMiles
            END
            AS decimal(6,2)
        ) AS ProgressPercent

    FROM dbo.Vehicles AS vehicle

    LEFT JOIN dbo.VehicleCurrentState AS currentState
        ON currentState.VehicleId = vehicle.VehicleId

    OUTER APPLY
    (
        SELECT TOP (1)
            tripAssignment.TripAssignmentId,
            tripAssignment.TripId,
            tripAssignment.DriverId,
            tripAssignment.TrailerId
        FROM dbo.TripAssignments AS tripAssignment
        WHERE tripAssignment.VehicleId = vehicle.VehicleId
          AND tripAssignment.IsActive = 1
        ORDER BY
            tripAssignment.CreatedAtUtc DESC,
            tripAssignment.TripAssignmentId DESC
    ) AS assignment

    LEFT JOIN dbo.Trips AS trip
        ON trip.TripId = assignment.TripId

    LEFT JOIN dbo.TripStatuses AS tripStatus
        ON tripStatus.TripStatusId = trip.TripStatusId

    LEFT JOIN dbo.Loads AS load
        ON load.LoadId = trip.LoadId

    LEFT JOIN dbo.Customers AS customer
        ON customer.CustomerId = load.CustomerId

    LEFT JOIN dbo.Drivers AS driver
        ON driver.DriverId = assignment.DriverId

    LEFT JOIN dbo.Trailers AS trailer
        ON trailer.TrailerId = assignment.TrailerId

    OUTER APPLY
    (
        SELECT TOP (1)
            tripStop.TripStopId,
            tripStop.StopSequence,
            stopType.Code AS StopTypeCode,
            tripStop.LocationId,
            location.LocationName,
            location.Latitude,
            location.Longitude,
            tripStop.ScheduledArrivalUtc
        FROM dbo.TripStops AS tripStop
        INNER JOIN dbo.StopTypes AS stopType
            ON stopType.StopTypeId = tripStop.StopTypeId
        INNER JOIN dbo.StopStatuses AS stopStatus
            ON stopStatus.StopStatusId = tripStop.StopStatusId
        INNER JOIN dbo.Locations AS location
            ON location.LocationId = tripStop.LocationId
        WHERE tripStop.TripId = assignment.TripId
          AND stopStatus.Code NOT IN ('COMPLETED', 'SKIPPED')
        ORDER BY
            tripStop.StopSequence,
            tripStop.TripStopId
    ) AS nextStop

    OUTER APPLY
    (
        SELECT TOP (1)
            routePoint.PointSequence,
            routePoint.CumulativeDistanceMiles
        FROM dbo.TripRoutePoints AS routePoint
        WHERE routePoint.TripId = assignment.TripId
          AND currentState.Latitude IS NOT NULL
          AND currentState.Longitude IS NOT NULL
        ORDER BY
            POWER(
                CONVERT(float, routePoint.Latitude)
                    - CONVERT(float, currentState.Latitude),
                2)
            +
            POWER(
                CONVERT(float, routePoint.Longitude)
                    - CONVERT(float, currentState.Longitude),
                2),
            routePoint.PointSequence
    ) AS routeProgress

    WHERE vehicle.IsActive = 1
      AND
      (
          @SimulationRunId IS NULL
          OR currentState.SimulationRunId = @SimulationRunId
      )
      AND
      (
          @IncludeOffline = 1
          OR
          (
              currentState.RecordedAtUtc IS NOT NULL
              AND DATEDIFF(
                    second,
                    currentState.RecordedAtUtc,
                    SYSUTCDATETIME()) <= @OfflineAfterSeconds
          )
      )

    ORDER BY
        CASE
            WHEN currentState.RecordedAtUtc IS NULL THEN 1
            ELSE 0
        END,
        vehicle.UnitNumber;
END;
GO

/* ============================================================
   2. Route points drawn as a polyline on the map
   ============================================================ */

CREATE OR ALTER PROCEDURE tracking.LiveTracking_GetTripRoute
    @TripId bigint
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.Trips
        WHERE TripId = @TripId
    )
    BEGIN
        THROW 51081,
            'The requested trip does not exist.',
            1;
    END;

    SELECT
        tripRoutePoint.TripRoutePointId,
        tripRoutePoint.TripId,
        trip.TripNumber,
        tripRoutePoint.PointSequence,
        tripRoutePoint.Latitude,
        tripRoutePoint.Longitude,
        tripRoutePoint.CumulativeDistanceMiles,
        tripRoutePoint.ExpectedOffsetSeconds,
        tripRoutePoint.Instruction,
        dataOrigin.Code AS DataOriginCode
    FROM dbo.TripRoutePoints AS tripRoutePoint
    INNER JOIN dbo.Trips AS trip
        ON trip.TripId = tripRoutePoint.TripId
    INNER JOIN dbo.DataOrigins AS dataOrigin
        ON dataOrigin.DataOriginId =
            tripRoutePoint.DataOriginId
    WHERE tripRoutePoint.TripId = @TripId
    ORDER BY tripRoutePoint.PointSequence;
END;
GO

/* ============================================================
   3. Stops drawn as markers on the map
   ============================================================ */

CREATE OR ALTER PROCEDURE tracking.LiveTracking_GetTripStops
    @TripId bigint
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.Trips
        WHERE TripId = @TripId
    )
    BEGIN
        THROW 51081,
            'The requested trip does not exist.',
            1;
    END;

    SELECT
        tripStop.TripStopId,
        tripStop.TripId,
        tripStop.StopSequence,
        stopType.Code AS StopTypeCode,
        stopType.DisplayName AS StopType,
        stopStatus.Code AS StopStatusCode,
        stopStatus.DisplayName AS StopStatus,
        tripStop.LocationId,
        location.LocationCode,
        location.LocationName,
        location.Address1,
        location.Address2,
        location.City,
        location.StateCode,
        location.PostalCode,
        location.Latitude,
        location.Longitude,
        tripStop.ScheduledArrivalUtc,
        tripStop.ScheduledDepartureUtc,
        tripStop.ActualArrivalUtc,
        tripStop.ActualDepartureUtc,
        tripStop.Instructions
    FROM dbo.TripStops AS tripStop
    INNER JOIN dbo.StopTypes AS stopType
        ON stopType.StopTypeId = tripStop.StopTypeId
    INNER JOIN dbo.StopStatuses AS stopStatus
        ON stopStatus.StopStatusId =
            tripStop.StopStatusId
    INNER JOIN dbo.Locations AS location
        ON location.LocationId = tripStop.LocationId
    WHERE tripStop.TripId = @TripId
    ORDER BY tripStop.StopSequence;
END;
GO

/* ============================================================
   4. Trips that are ready for concurrent simulation

   Each returned row can be represented by one independent
   Task in the C# simulator.
   ============================================================ */

CREATE OR ALTER PROCEDURE simulation.LiveTracking_GetCandidates
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        assignment.TripAssignmentId,
        assignment.TripId,
        trip.TripNumber,

        assignment.VehicleId,
        vehicle.UnitNumber,

        assignment.DriverId,
        driver.DriverNumber,
        CONCAT(driver.FirstName, N' ', driver.LastName)
            AS DriverName,

        assignment.TrailerId,
        trailer.UnitNumber AS TrailerUnitNumber,

        trip.PlannedDistanceMiles,
        trip.ScheduledPickupUtc,
        trip.ScheduledDeliveryUtc,

        tripStatus.Code AS TripStatusCode,
        tripStatus.DisplayName AS TripStatus,

        routeSummary.RoutePointCount,
        routeSummary.FirstPointSequence,
        routeSummary.FirstLatitude,
        routeSummary.FirstLongitude,
        routeSummary.LastPointSequence,
        routeSummary.LastLatitude,
        routeSummary.LastLongitude

    FROM dbo.TripAssignments AS assignment

    INNER JOIN dbo.Trips AS trip
        ON trip.TripId = assignment.TripId

    INNER JOIN dbo.TripStatuses AS tripStatus
        ON tripStatus.TripStatusId = trip.TripStatusId

    INNER JOIN dbo.Vehicles AS vehicle
        ON vehicle.VehicleId = assignment.VehicleId

    INNER JOIN dbo.Drivers AS driver
        ON driver.DriverId = assignment.DriverId

    LEFT JOIN dbo.Trailers AS trailer
        ON trailer.TrailerId = assignment.TrailerId

    CROSS APPLY
    (
        SELECT
            COUNT(*) AS RoutePointCount,
            MIN(routePoint.PointSequence)
                AS FirstPointSequence,
            MAX(routePoint.PointSequence)
                AS LastPointSequence,

            MAX(
                CASE
                    WHEN routePoint.PointSequence =
                    (
                        SELECT MIN(firstPoint.PointSequence)
                        FROM dbo.TripRoutePoints AS firstPoint
                        WHERE firstPoint.TripId = trip.TripId
                    )
                    THEN routePoint.Latitude
                END
            ) AS FirstLatitude,

            MAX(
                CASE
                    WHEN routePoint.PointSequence =
                    (
                        SELECT MIN(firstPoint.PointSequence)
                        FROM dbo.TripRoutePoints AS firstPoint
                        WHERE firstPoint.TripId = trip.TripId
                    )
                    THEN routePoint.Longitude
                END
            ) AS FirstLongitude,

            MAX(
                CASE
                    WHEN routePoint.PointSequence =
                    (
                        SELECT MAX(lastPoint.PointSequence)
                        FROM dbo.TripRoutePoints AS lastPoint
                        WHERE lastPoint.TripId = trip.TripId
                    )
                    THEN routePoint.Latitude
                END
            ) AS LastLatitude,

            MAX(
                CASE
                    WHEN routePoint.PointSequence =
                    (
                        SELECT MAX(lastPoint.PointSequence)
                        FROM dbo.TripRoutePoints AS lastPoint
                        WHERE lastPoint.TripId = trip.TripId
                    )
                    THEN routePoint.Longitude
                END
            ) AS LastLongitude

        FROM dbo.TripRoutePoints AS routePoint
        WHERE routePoint.TripId = trip.TripId
    ) AS routeSummary

    WHERE assignment.IsActive = 1
      AND vehicle.IsActive = 1
      AND driver.IsActive = 1
      AND tripStatus.IsTerminal = 0
      AND routeSummary.RoutePointCount >= 2

    ORDER BY
        trip.ScheduledPickupUtc,
        vehicle.UnitNumber;
END;
GO

/* ============================================================
   5. Existing simulation runs
   ============================================================ */

CREATE OR ALTER PROCEDURE simulation.LiveTracking_GetRuns
    @IncludeCompleted bit = 0
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        simulationRun.SimulationRunId,
        simulationRun.SimulationRunUid,
        simulationRun.Name,
        simulationRun.ScenarioCode,
        simulationRun.Status,
        simulationRun.RandomSeed,
        simulationRun.TimeScale,
        simulationRun.UpdateIntervalMilliseconds,
        simulationRun.PlannedVehicleCount,
        simulationRun.ConfigurationJson,
        simulationRun.StartedAtUtc,
        simulationRun.EndedAtUtc,
        simulationRun.CreatedAtUtc,

        (
            SELECT COUNT_BIG(*)
            FROM dbo.VehicleTelemetry AS telemetry
            WHERE telemetry.SimulationRunId =
                simulationRun.SimulationRunId
        ) AS TelemetryRows,

        (
            SELECT COUNT(DISTINCT telemetry.VehicleId)
            FROM dbo.VehicleTelemetry AS telemetry
            WHERE telemetry.SimulationRunId =
                simulationRun.SimulationRunId
        ) AS ActualVehicleCount

    FROM dbo.SimulationRuns AS simulationRun

    WHERE @IncludeCompleted = 1
       OR simulationRun.Status NOT IN
          ('COMPLETED', 'FAILED', 'CANCELLED')

    ORDER BY
        simulationRun.CreatedAtUtc DESC,
        simulationRun.SimulationRunId DESC;
END;
GO

/* ============================================================
   6. Permissions
   ============================================================ */

GRANT EXECUTE
ON OBJECT::tracking.LiveTracking_GetMapState
TO FleetFlowAppExecutor;
GO

GRANT EXECUTE
ON OBJECT::tracking.LiveTracking_GetTripRoute
TO FleetFlowAppExecutor;
GO

GRANT EXECUTE
ON OBJECT::tracking.LiveTracking_GetTripStops
TO FleetFlowAppExecutor;
GO

GRANT EXECUTE
ON OBJECT::simulation.LiveTracking_GetCandidates
TO FleetFlowAppExecutor;
GO

GRANT EXECUTE
ON OBJECT::simulation.LiveTracking_GetRuns
TO FleetFlowAppExecutor;
GO

/* ============================================================
   7. Verification
   ============================================================ */

EXEC tracking.LiveTracking_GetMapState
    @IncludeOffline = 1,
    @OfflineAfterSeconds = 60,
    @SimulationRunId = NULL;
GO

EXEC simulation.LiveTracking_GetCandidates;
GO

PRINT '026_create_live_tracking.sql completed successfully.';
GO