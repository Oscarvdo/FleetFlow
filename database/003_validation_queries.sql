USE FleetFlowDb;
GO

/* Dispatch board: one row per active trip assignment. */
SELECT
    t.TripNumber,
    ts.DisplayName AS TripStatus,
    l.LoadNumber,
    c.CompanyName AS Customer,
    CONCAT(d.FirstName, N' ', d.LastName) AS Driver,
    v.UnitNumber AS Truck,
    tr.UnitNumber AS Trailer,
    ats.DisplayName AS AssignmentStatus,
    pickup.LocationName AS Pickup,
    delivery.LocationName AS Delivery,
    t.ScheduledPickupUtc,
    t.ScheduledDeliveryUtc
FROM dbo.TripAssignments AS a
JOIN dbo.Trips AS t ON t.TripId = a.TripId
JOIN dbo.TripStatuses AS ts ON ts.TripStatusId = t.TripStatusId
JOIN dbo.Loads AS l ON l.LoadId = t.LoadId
JOIN dbo.Customers AS c ON c.CustomerId = l.CustomerId
JOIN dbo.Drivers AS d ON d.DriverId = a.DriverId
JOIN dbo.Vehicles AS v ON v.VehicleId = a.VehicleId
LEFT JOIN dbo.Trailers AS tr ON tr.TrailerId = a.TrailerId
JOIN dbo.AssignmentStatuses AS ats ON ats.AssignmentStatusId = a.AssignmentStatusId
OUTER APPLY
(
    SELECT TOP (1) loc.LocationName
    FROM dbo.TripStops AS s
    JOIN dbo.StopTypes AS st ON st.StopTypeId = s.StopTypeId
    JOIN dbo.Locations AS loc ON loc.LocationId = s.LocationId
    WHERE s.TripId = t.TripId AND st.Code = 'PICKUP'
    ORDER BY s.StopSequence
) AS pickup
OUTER APPLY
(
    SELECT TOP (1) loc.LocationName
    FROM dbo.TripStops AS s
    JOIN dbo.StopTypes AS st ON st.StopTypeId = s.StopTypeId
    JOIN dbo.Locations AS loc ON loc.LocationId = s.LocationId
    WHERE s.TripId = t.TripId AND st.Code = 'DELIVERY'
    ORDER BY s.StopSequence DESC
) AS delivery
WHERE a.IsActive = 1
ORDER BY t.ScheduledPickupUtc;
GO

/* Resources that Dispatch can assign. */
SELECT DriverId, DriverNumber, FirstName, LastName
FROM dbo.Drivers AS d
JOIN dbo.DriverStatuses AS ds ON ds.DriverStatusId = d.DriverStatusId
WHERE d.IsActive = 1 AND ds.IsAvailable = 1;

SELECT VehicleId, UnitNumber, Make, Model, MaxPayloadLbs
FROM dbo.Vehicles AS v
JOIN dbo.FleetAssetStatuses AS fs ON fs.FleetAssetStatusId = v.FleetAssetStatusId
WHERE v.IsActive = 1 AND fs.IsAvailable = 1;

SELECT TrailerId, UnitNumber, TrailerType, MaxPayloadLbs
FROM dbo.Trailers AS t
JOIN dbo.FleetAssetStatuses AS fs ON fs.FleetAssetStatusId = t.FleetAssetStatusId
WHERE t.IsActive = 1 AND fs.IsAvailable = 1;
GO

/* Ordered stops for the demo trip. */
SELECT
    t.TripNumber,
    s.StopSequence,
    st.DisplayName AS StopType,
    ss.DisplayName AS StopStatus,
    loc.LocationName,
    loc.City,
    loc.StateCode,
    s.ScheduledArrivalUtc
FROM dbo.TripStops AS s
JOIN dbo.Trips AS t ON t.TripId = s.TripId
JOIN dbo.StopTypes AS st ON st.StopTypeId = s.StopTypeId
JOIN dbo.StopStatuses AS ss ON ss.StopStatusId = s.StopStatusId
JOIN dbo.Locations AS loc ON loc.LocationId = s.LocationId
WHERE t.TripNumber = 'TRIP-2026-0001'
ORDER BY s.StopSequence;
GO

/* Low-volume domain event stream consumed by the Dispatch board. */
SELECT TOP (100)
    t.TripNumber,
    v.UnitNumber,
    e.EventType,
    e.OccurredAtUtc,
    e.Message,
    origin.Code AS DataOrigin
FROM dbo.TripEvents AS e
JOIN dbo.Trips AS t ON t.TripId = e.TripId
JOIN dbo.Vehicles AS v ON v.VehicleId = e.VehicleId
JOIN dbo.DataOrigins AS origin ON origin.DataOriginId = e.DataOriginId
ORDER BY e.OccurredAtUtc DESC;
GO

/* High-volume telemetry stream used by the simulator and future analytics. */
SELECT TOP (100)
    v.UnitNumber,
    t.TripNumber,
    tm.RecordedAtUtc,
    tm.ReceivedAtUtc,
    tm.Latitude,
    tm.Longitude,
    tm.SpeedMph,
    tm.FuelPercent,
    tm.OdometerMiles,
    tm.HeadingDegrees,
    origin.Code AS DataOrigin,
    sr.Name AS SimulationRun
FROM dbo.VehicleTelemetry AS tm
JOIN dbo.Vehicles AS v ON v.VehicleId = tm.VehicleId
LEFT JOIN dbo.Trips AS t ON t.TripId = tm.TripId
JOIN dbo.DataOrigins AS origin ON origin.DataOriginId = tm.DataOriginId
LEFT JOIN dbo.SimulationRuns AS sr ON sr.SimulationRunId = tm.SimulationRunId
ORDER BY tm.RecordedAtUtc DESC;
GO

/* Planned route consumed by the WinForms map and truck simulator. */
SELECT
    t.TripNumber,
    rp.PointSequence,
    rp.Latitude,
    rp.Longitude,
    rp.CumulativeDistanceMiles,
    rp.ExpectedOffsetSeconds,
    rp.Instruction,
    origin.Code AS DataOrigin
FROM dbo.TripRoutePoints AS rp
JOIN dbo.Trips AS t ON t.TripId = rp.TripId
JOIN dbo.DataOrigins AS origin ON origin.DataOriginId = rp.DataOriginId
WHERE t.TripNumber = 'TRIP-2026-0001'
ORDER BY rp.PointSequence;
GO

/* Import lineage and row-level validation errors. */
SELECT
    b.ImportBatchUid,
    b.EntityType,
    b.FileName,
    b.Status,
    b.TotalRows,
    b.ValidRows,
    b.InvalidRows,
    b.ImportedRows,
    COUNT(e.ImportBatchErrorId) AS RecordedErrors
FROM dbo.ImportBatches AS b
LEFT JOIN dbo.ImportBatchErrors AS e ON e.ImportBatchId = b.ImportBatchId
GROUP BY
    b.ImportBatchUid, b.EntityType, b.FileName, b.Status,
    b.TotalRows, b.ValidRows, b.InvalidRows, b.ImportedRows
ORDER BY b.StartedAtUtc DESC;
GO

/* Reproducible simulation scenarios. */
SELECT
    SimulationRunUid,
    Name,
    ScenarioCode,
    Status,
    RandomSeed,
    TimeScale,
    UpdateIntervalMilliseconds,
    PlannedVehicleCount,
    StartedAtUtc,
    EndedAtUtc
FROM dbo.SimulationRuns
ORDER BY CreatedAtUtc DESC;
GO
