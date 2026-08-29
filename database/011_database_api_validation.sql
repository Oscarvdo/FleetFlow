/* Read-only validation for the complete FleetFlow database API. */
USE FleetFlowDb;
GO

/* Inventory by object type and schema. */
SELECT s.name AS SchemaName, o.type_desc AS ObjectType, COUNT(*) AS ObjectCount
FROM sys.objects AS o
JOIN sys.schemas AS s ON s.schema_id = o.schema_id
WHERE s.name IN ('dbo', 'catalog', 'operations', 'dispatch', 'tracking',
                 'import', 'simulation', 'security', 'reporting')
  AND o.is_ms_shipped = 0
GROUP BY s.name, o.type_desc
ORDER BY s.name, o.type_desc;
GO

/* Every required database API object should return IsMissing = 0. */
WITH RequiredObjects AS
(
    SELECT * FROM (VALUES
        ('dispatch.vw_DispatchBoard', 'V'),
        ('operations.vw_TripDetails', 'V'),
        ('tracking.vw_ActiveVehiclePositions', 'V'),
        ('reporting.vw_TripPerformance', 'V'),
        ('security.AppUser_GetForLogin', 'P'),
        ('catalog.Customer_Create', 'P'),
        ('operations.Trip_Create', 'P'),
        ('operations.Trip_TransitionStatus', 'P'),
        ('dispatch.Assignment_Offer', 'P'),
        ('tracking.VehicleTelemetry_AppendBatch', 'P'),
        ('import.Batch_Begin', 'P'),
        ('simulation.Run_Create', 'P'),
        ('dbo.TR_TripStatusHistory_Immutable', 'TR')
    ) AS required(ObjectName, ObjectType)
)
SELECT ObjectName, ObjectType,
       CAST(CASE WHEN OBJECT_ID(ObjectName, ObjectType) IS NULL THEN 1 ELSE 0 END AS bit) AS IsMissing
FROM RequiredObjects;
GO

/* Workflow integrity. These queries should return zero rows. */
SELECT transition.*
FROM dbo.TripStatusTransitions AS transition
LEFT JOIN dbo.TripStatuses AS sourceStatus
  ON sourceStatus.TripStatusId = transition.FromTripStatusId
LEFT JOIN dbo.TripStatuses AS targetStatus
  ON targetStatus.TripStatusId = transition.ToTripStatusId
WHERE sourceStatus.TripStatusId IS NULL OR targetStatus.TripStatusId IS NULL;
GO

/* Current-state pointers must match the historical telemetry row. */
SELECT state.VehicleId, state.LastVehicleTelemetryId
FROM dbo.VehicleCurrentState AS state
JOIN dbo.VehicleTelemetry AS telemetry
  ON telemetry.VehicleTelemetryId = state.LastVehicleTelemetryId
WHERE state.VehicleId <> telemetry.VehicleId
   OR state.RecordedAtUtc <> telemetry.RecordedAtUtc;
GO

/* Validate read models against the seeded trip. */
SELECT * FROM operations.vw_TripDetails WHERE TripNumber = 'TRIP-2026-0001';
SELECT * FROM operations.vw_TripStops WHERE TripNumber = 'TRIP-2026-0001' ORDER BY StopSequence;
SELECT * FROM tracking.vw_TripRoute WHERE TripNumber = 'TRIP-2026-0001' ORDER BY PointSequence;
SELECT * FROM dispatch.vw_DispatchBoard WHERE TripNumber = 'TRIP-2026-0001';
GO

/* Role and permission matrix used by application authorization. */
SELECT r.Code AS RoleCode, COUNT(*) AS PermissionCount
FROM dbo.RolePermissions AS rp
JOIN dbo.Roles AS r ON r.RoleId = rp.RoleId
GROUP BY r.Code
ORDER BY r.Code;
GO
