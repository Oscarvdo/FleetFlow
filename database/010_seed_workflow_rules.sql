/* Allowed trip transitions and initial retention policies. */
USE FleetFlowDb;
GO

SET XACT_ABORT ON;
BEGIN TRANSACTION;

DECLARE @TripUpdatePermissionId smallint =
    (SELECT PermissionId FROM dbo.Permissions WHERE Code = 'TRIPS.UPDATE');

;WITH TransitionSeed AS
(
    SELECT * FROM (VALUES
        ('PLANNED', 'OFFERED',                 0, 1, 1),
        ('PLANNED', 'CANCELLED',               0, 1, 1),
        ('OFFERED', 'PLANNED',                 0, 1, 1),
        ('OFFERED', 'ASSIGNED',                1, 1, 1),
        ('OFFERED', 'CANCELLED',               0, 1, 1),
        ('ASSIGNED', 'EN_ROUTE_TO_PICKUP',     1, 1, 1),
        ('ASSIGNED', 'CANCELLED',              0, 1, 1),
        ('EN_ROUTE_TO_PICKUP', 'AT_PICKUP',    1, 1, 1),
        ('EN_ROUTE_TO_PICKUP', 'DELAYED',      1, 1, 1),
        ('EN_ROUTE_TO_PICKUP', 'INCIDENT_REPORTED', 1, 1, 1),
        ('EN_ROUTE_TO_PICKUP', 'VEHICLE_BREAKDOWN', 1, 1, 1),
        ('AT_PICKUP', 'LOADED',                1, 1, 1),
        ('AT_PICKUP', 'DELAYED',               1, 1, 1),
        ('LOADED', 'EN_ROUTE_TO_DELIVERY',     1, 1, 1),
        ('LOADED', 'CANCELLED',                0, 1, 1),
        ('EN_ROUTE_TO_DELIVERY', 'AT_DELIVERY',1, 1, 1),
        ('EN_ROUTE_TO_DELIVERY', 'DELAYED',    1, 1, 1),
        ('EN_ROUTE_TO_DELIVERY', 'INCIDENT_REPORTED', 1, 1, 1),
        ('EN_ROUTE_TO_DELIVERY', 'VEHICLE_BREAKDOWN', 1, 1, 1),
        ('AT_DELIVERY', 'DELIVERED',           1, 1, 1),
        ('AT_DELIVERY', 'DELAYED',             1, 1, 1),
        ('DELIVERED', 'COMPLETED',             1, 1, 1),
        ('DELAYED', 'EN_ROUTE_TO_PICKUP',      1, 1, 1),
        ('DELAYED', 'AT_PICKUP',               1, 1, 1),
        ('DELAYED', 'EN_ROUTE_TO_DELIVERY',    1, 1, 1),
        ('DELAYED', 'AT_DELIVERY',             1, 1, 1),
        ('DELAYED', 'CANCELLED',               0, 1, 1),
        ('INCIDENT_REPORTED', 'EN_ROUTE_TO_PICKUP',   1, 1, 1),
        ('INCIDENT_REPORTED', 'EN_ROUTE_TO_DELIVERY', 1, 1, 1),
        ('INCIDENT_REPORTED', 'CANCELLED',      0, 1, 1),
        ('VEHICLE_BREAKDOWN', 'EN_ROUTE_TO_PICKUP',   0, 1, 1),
        ('VEHICLE_BREAKDOWN', 'EN_ROUTE_TO_DELIVERY', 0, 1, 1),
        ('VEHICLE_BREAKDOWN', 'CANCELLED',      0, 1, 1)
    ) AS value(FromCode, ToCode, IsDriverAllowed, IsDispatchAllowed, IsSystemAllowed)
)
INSERT dbo.TripStatusTransitions
    (FromTripStatusId, ToTripStatusId, RequiredPermissionId,
     IsDriverAllowed, IsDispatchAllowed, IsSystemAllowed)
SELECT sourceStatus.TripStatusId, targetStatus.TripStatusId, @TripUpdatePermissionId,
       seed.IsDriverAllowed, seed.IsDispatchAllowed, seed.IsSystemAllowed
FROM TransitionSeed AS seed
JOIN dbo.TripStatuses AS sourceStatus ON sourceStatus.Code = seed.FromCode
JOIN dbo.TripStatuses AS targetStatus ON targetStatus.Code = seed.ToCode;

INSERT dbo.DataRetentionPolicies
    (DataType, HotRetentionDays, ArchiveAfterDays, DeleteAfterDays)
VALUES
    ('VEHICLE_TELEMETRY', 30, 90, 365),
    ('TRIP_EVENTS', 180, 365, NULL),
    ('SECURITY_AUDIT', 365, 730, NULL),
    ('CHANGE_AUDIT', 365, 730, NULL),
    ('IMPORT_ERRORS', 90, 180, 365);

COMMIT TRANSACTION;
GO
