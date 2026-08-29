/* FleetFlow read models for WinForms, API, MAUI, reporting, and monitoring. */
USE FleetFlowDb;
GO

CREATE OR ALTER VIEW dispatch.vw_DispatchBoard
AS
SELECT
    t.TripId,
    t.TripNumber,
    ts.Code AS TripStatusCode,
    ts.DisplayName AS TripStatus,
    l.LoadId,
    l.LoadNumber,
    c.CustomerId,
    c.CompanyName AS Customer,
    a.TripAssignmentId,
    ats.Code AS AssignmentStatusCode,
    d.DriverId,
    d.DriverNumber,
    CONCAT(d.FirstName, N' ', d.LastName) AS DriverName,
    v.VehicleId,
    v.UnitNumber AS VehicleUnitNumber,
    tr.TrailerId,
    tr.UnitNumber AS TrailerUnitNumber,
    t.ScheduledPickupUtc,
    t.ScheduledDeliveryUtc,
    pickup.LocationName AS PickupLocation,
    delivery.LocationName AS DeliveryLocation,
    state.RecordedAtUtc AS LastTelemetryUtc,
    state.Latitude,
    state.Longitude,
    state.SpeedMph,
    state.FuelPercent,
    progress.ProgressPercent,
    t.RowVersion
FROM dbo.Trips AS t
JOIN dbo.TripStatuses AS ts
    ON ts.TripStatusId = t.TripStatusId
JOIN dbo.Loads AS l
    ON l.LoadId = t.LoadId
JOIN dbo.Customers AS c
    ON c.CustomerId = l.CustomerId
LEFT JOIN dbo.TripAssignments AS a
    ON a.TripId = t.TripId
    AND a.IsActive = 1
LEFT JOIN dbo.AssignmentStatuses AS ats
    ON ats.AssignmentStatusId = a.AssignmentStatusId
LEFT JOIN dbo.Drivers AS d
    ON d.DriverId = a.DriverId
LEFT JOIN dbo.Vehicles AS v
    ON v.VehicleId = a.VehicleId
LEFT JOIN dbo.Trailers AS tr
    ON tr.TrailerId = a.TrailerId
LEFT JOIN dbo.VehicleCurrentState AS state
    ON state.VehicleId = v.VehicleId
OUTER APPLY
(
    SELECT TOP (1)
        loc.LocationName
    FROM dbo.TripStops AS s
    JOIN dbo.Locations AS loc
        ON loc.LocationId = s.LocationId
    WHERE s.TripId = t.TripId
    ORDER BY s.StopSequence
) AS pickup
OUTER APPLY
(
    SELECT TOP (1)
        loc.LocationName
    FROM dbo.TripStops AS s
    JOIN dbo.Locations AS loc
        ON loc.LocationId = s.LocationId
    WHERE s.TripId = t.TripId
    ORDER BY s.StopSequence DESC
) AS delivery
OUTER APPLY operations.fn_TripProgress(t.TripId) AS progress
WHERE ts.IsTerminal = 0;
GO

CREATE OR ALTER VIEW dispatch.vw_AvailableDrivers
AS
SELECT
    d.DriverId,
    d.DriverNumber,
    d.FirstName,
    d.LastName,
    d.LicenseExpirationDate,
    d.RowVersion
FROM dbo.Drivers AS d
JOIN dbo.DriverStatuses AS s
    ON s.DriverStatusId = d.DriverStatusId
WHERE d.IsActive = 1
  AND s.IsAvailable = 1
  AND d.LicenseExpirationDate >= CONVERT(date, SYSUTCDATETIME());
GO

CREATE OR ALTER VIEW dispatch.vw_AvailableVehicles
AS
SELECT
    v.VehicleId,
    v.UnitNumber,
    v.Make,
    v.Model,
    v.ModelYear,
    v.MaxPayloadLbs,
    v.CurrentOdometerMiles,
    v.RowVersion
FROM dbo.Vehicles AS v
JOIN dbo.FleetAssetStatuses AS s
    ON s.FleetAssetStatusId = v.FleetAssetStatusId
WHERE v.IsActive = 1
  AND s.IsAvailable = 1;
GO

CREATE OR ALTER VIEW dispatch.vw_AvailableTrailers
AS
SELECT
    t.TrailerId,
    t.UnitNumber,
    t.TrailerType,
    t.MaxPayloadLbs,
    t.RowVersion
FROM dbo.Trailers AS t
JOIN dbo.FleetAssetStatuses AS s
    ON s.FleetAssetStatusId = t.FleetAssetStatusId
WHERE t.IsActive = 1
  AND s.IsAvailable = 1;
GO

CREATE OR ALTER VIEW operations.vw_TripDetails
AS
SELECT
    t.TripId,
    t.TripNumber,
    t.LoadId,
    l.LoadNumber,
    l.Description AS LoadDescription,
    l.WeightLbs,
    c.CustomerId,
    c.CustomerNumber,
    c.CompanyName,
    ts.Code AS TripStatusCode,
    ts.DisplayName AS TripStatus,
    t.ScheduledPickupUtc,
    t.ScheduledDeliveryUtc,
    t.ActualStartUtc,
    t.ActualDeliveryUtc,
    t.PlannedDistanceMiles,
    t.ActualDistanceMiles,
    t.Notes,
    progress.TotalStops,
    progress.CompletedStops,
    progress.ProgressPercent,
    t.RowVersion
FROM dbo.Trips AS t
JOIN dbo.TripStatuses AS ts
    ON ts.TripStatusId = t.TripStatusId
JOIN dbo.Loads AS l
    ON l.LoadId = t.LoadId
JOIN dbo.Customers AS c
    ON c.CustomerId = l.CustomerId
OUTER APPLY operations.fn_TripProgress(t.TripId) AS progress;
GO

CREATE OR ALTER VIEW operations.vw_TripStops
AS
SELECT
    s.TripStopId,
    s.TripId,
    t.TripNumber,
    s.StopSequence,
    st.Code AS StopTypeCode,
    ss.Code AS StopStatusCode,
    loc.LocationId,
    loc.LocationCode,
    loc.LocationName,
    loc.Address1,
    loc.Address2,
    loc.City,
    loc.StateCode,
    loc.PostalCode,
    loc.Latitude,
    loc.Longitude,
    s.ScheduledArrivalUtc,
    s.ScheduledDepartureUtc,
    s.ActualArrivalUtc,
    s.ActualDepartureUtc,
    s.Instructions,
    s.RowVersion
FROM dbo.TripStops AS s
JOIN dbo.Trips AS t
    ON t.TripId = s.TripId
JOIN dbo.StopTypes AS st
    ON st.StopTypeId = s.StopTypeId
JOIN dbo.StopStatuses AS ss
    ON ss.StopStatusId = s.StopStatusId
JOIN dbo.Locations AS loc
    ON loc.LocationId = s.LocationId;
GO

CREATE OR ALTER VIEW operations.vw_TripStatusTimeline
AS
SELECT
    h.TripStatusHistoryId,
    h.TripId,
    t.TripNumber,
    previousStatus.Code AS PreviousStatusCode,
    newStatus.Code AS NewStatusCode,
    h.ChangedAtUtc,
    h.ChangedBy,
    h.Source,
    h.Notes
FROM dbo.TripStatusHistory AS h
JOIN dbo.Trips AS t
    ON t.TripId = h.TripId
LEFT JOIN dbo.TripStatuses AS previousStatus
    ON previousStatus.TripStatusId = h.PreviousTripStatusId
JOIN dbo.TripStatuses AS newStatus
    ON newStatus.TripStatusId = h.NewTripStatusId;
GO

CREATE OR ALTER VIEW tracking.vw_ActiveVehiclePositions
AS
SELECT
    state.VehicleId,
    v.UnitNumber,
    state.TripId,
    t.TripNumber,
    state.RecordedAtUtc,
    state.Latitude,
    state.Longitude,
    state.SpeedMph,
    state.FuelPercent,
    state.OdometerMiles,
    state.HeadingDegrees,
    DATEDIFF(
        second,
        state.RecordedAtUtc,
        SYSUTCDATETIME()
    ) AS TelemetryAgeSeconds,
    state.SimulationRunId
FROM dbo.VehicleCurrentState AS state
JOIN dbo.Vehicles AS v
    ON v.VehicleId = state.VehicleId
LEFT JOIN dbo.Trips AS t
    ON t.TripId = state.TripId
WHERE v.IsActive = 1;
GO

CREATE OR ALTER VIEW tracking.vw_TripRoute
AS
SELECT
    rp.TripRoutePointId,
    rp.TripId,
    t.TripNumber,
    rp.PointSequence,
    rp.Latitude,
    rp.Longitude,
    rp.CumulativeDistanceMiles,
    rp.ExpectedOffsetSeconds,
    rp.Instruction,
    origin.Code AS DataOriginCode
FROM dbo.TripRoutePoints AS rp
JOIN dbo.Trips AS t
    ON t.TripId = rp.TripId
JOIN dbo.DataOrigins AS origin
    ON origin.DataOriginId = rp.DataOriginId;
GO

CREATE OR ALTER VIEW tracking.vw_TripEventTimeline
AS
SELECT
    e.TripEventId,
    e.EventId,
    e.TripId,
    t.TripNumber,
    e.VehicleId,
    v.UnitNumber,
    e.TripStopId,
    e.EventType,
    e.OccurredAtUtc,
    e.ReceivedAtUtc,
    e.Message,
    e.PayloadJson,
    origin.Code AS DataOriginCode,
    e.ImportBatchId,
    e.SimulationRunId,
    e.CorrelationId
FROM dbo.TripEvents AS e
JOIN dbo.Trips AS t
    ON t.TripId = e.TripId
JOIN dbo.Vehicles AS v
    ON v.VehicleId = e.VehicleId
JOIN dbo.DataOrigins AS origin
    ON origin.DataOriginId = e.DataOriginId;
GO

CREATE OR ALTER VIEW security.vw_UserEffectivePermissions
AS
SELECT DISTINCT
    u.AppUserId,
    u.Username,
    r.RoleId,
    r.Code AS RoleCode,
    p.PermissionId,
    p.Code AS PermissionCode,
    p.Module
FROM dbo.AppUsers AS u
JOIN dbo.UserRoles AS ur
    ON ur.AppUserId = u.AppUserId
JOIN dbo.Roles AS r
    ON r.RoleId = ur.RoleId
    AND r.IsActive = 1
JOIN dbo.RolePermissions AS rp
    ON rp.RoleId = r.RoleId
JOIN dbo.Permissions AS p
    ON p.PermissionId = rp.PermissionId
WHERE u.IsActive = 1;
GO

CREATE OR ALTER VIEW import.vw_ImportBatchSummary
AS
SELECT
    b.ImportBatchId,
    b.ImportBatchUid,
    b.EntityType,
    b.FileName,
    b.FileSha256,
    b.Status,
    b.StartedAtUtc,
    b.CompletedAtUtc,
    b.TotalRows,
    b.ValidRows,
    b.InvalidRows,
    b.ImportedRows,
    b.CreatedByAppUserId,
    COUNT_BIG(e.ImportBatchErrorId) AS RecordedErrors
FROM dbo.ImportBatches AS b
LEFT JOIN dbo.ImportBatchErrors AS e
    ON e.ImportBatchId = b.ImportBatchId
GROUP BY
    b.ImportBatchId,
    b.ImportBatchUid,
    b.EntityType,
    b.FileName,
    b.FileSha256,
    b.Status,
    b.StartedAtUtc,
    b.CompletedAtUtc,
    b.TotalRows,
    b.ValidRows,
    b.InvalidRows,
    b.ImportedRows,
    b.CreatedByAppUserId;
GO

CREATE OR ALTER VIEW simulation.vw_SimulationRunSummary
AS
SELECT
    r.SimulationRunId,
    r.SimulationRunUid,
    r.Name,
    r.ScenarioCode,
    r.Status,
    r.RandomSeed,
    r.TimeScale,
    r.UpdateIntervalMilliseconds,
    r.PlannedVehicleCount,
    r.StartedAtUtc,
    r.EndedAtUtc,
    r.CreatedAtUtc,
    r.CreatedByAppUserId,
    COUNT_BIG(t.VehicleTelemetryId) AS TelemetryRows,
    COUNT(DISTINCT t.VehicleId) AS ActualVehicleCount
FROM dbo.SimulationRuns AS r
LEFT JOIN dbo.VehicleTelemetry AS t
    ON t.SimulationRunId = r.SimulationRunId
GROUP BY
    r.SimulationRunId,
    r.SimulationRunUid,
    r.Name,
    r.ScenarioCode,
    r.Status,
    r.RandomSeed,
    r.TimeScale,
    r.UpdateIntervalMilliseconds,
    r.PlannedVehicleCount,
    r.StartedAtUtc,
    r.EndedAtUtc,
    r.CreatedAtUtc,
    r.CreatedByAppUserId;
GO

CREATE OR ALTER VIEW reporting.vw_TripPerformance
AS
SELECT
    t.TripId,
    t.TripNumber,
    l.LoadNumber,
    c.CustomerNumber,
    c.CompanyName,
    ts.Code AS TripStatusCode,
    t.ScheduledPickupUtc,
    t.ScheduledDeliveryUtc,
    t.ActualStartUtc,
    t.ActualDeliveryUtc,
    t.PlannedDistanceMiles,
    t.ActualDistanceMiles,
    CASE
        WHEN t.ActualDeliveryUtc IS NULL
            THEN NULL
        WHEN t.ActualDeliveryUtc <= t.ScheduledDeliveryUtc
            THEN CAST(1 AS bit)
        ELSE CAST(0 AS bit)
    END AS WasDeliveredOnTime,
    DATEDIFF(
        minute,
        t.ScheduledDeliveryUtc,
        t.ActualDeliveryUtc
    ) AS DeliveryVarianceMinutes,
    progress.ProgressPercent
FROM dbo.Trips AS t
JOIN dbo.TripStatuses AS ts
    ON ts.TripStatusId = t.TripStatusId
JOIN dbo.Loads AS l
    ON l.LoadId = t.LoadId
JOIN dbo.Customers AS c
    ON c.CustomerId = l.CustomerId
OUTER APPLY operations.fn_TripProgress(t.TripId) AS progress;
GO