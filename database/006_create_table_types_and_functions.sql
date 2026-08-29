/* FleetFlow table-valued parameters and reusable inline functions. */
USE FleetFlowDb;
GO

CREATE TYPE tracking.VehicleTelemetryTableType AS TABLE
(
    ClientRowId int NOT NULL,
    TelemetryId uniqueidentifier NOT NULL,
    VehicleId bigint NOT NULL,
    TripId bigint NULL,
    RecordedAtUtc datetime2(3) NOT NULL,
    SequenceNumber bigint NULL,
    Latitude decimal(9,6) NOT NULL,
    Longitude decimal(9,6) NOT NULL,
    SpeedMph decimal(6,2) NULL,
    FuelPercent decimal(5,2) NULL,
    OdometerMiles decimal(12,1) NULL,
    HeadingDegrees decimal(6,2) NULL,
    DataOriginId tinyint NOT NULL,
    ImportBatchId bigint NULL,
    SimulationRunId bigint NULL,
    PayloadJson nvarchar(max) NULL,
    PRIMARY KEY (ClientRowId)
);
GO

CREATE TYPE tracking.TripEventTableType AS TABLE
(
    ClientRowId int NOT NULL,
    EventId uniqueidentifier NOT NULL,
    TripId bigint NOT NULL,
    VehicleId bigint NOT NULL,
    TripStopId bigint NULL,
    EventType varchar(40) NOT NULL,
    OccurredAtUtc datetime2(3) NOT NULL,
    CorrelationId uniqueidentifier NULL,
    DataOriginId tinyint NOT NULL,
    ImportBatchId bigint NULL,
    SimulationRunId bigint NULL,
    Message nvarchar(500) NULL,
    PayloadJson nvarchar(max) NULL,
    PRIMARY KEY (ClientRowId)
);
GO

CREATE TYPE tracking.RoutePointTableType AS TABLE
(
    PointSequence int NOT NULL,
    Latitude decimal(9,6) NOT NULL,
    Longitude decimal(9,6) NOT NULL,
    CumulativeDistanceMiles decimal(10,3) NOT NULL,
    ExpectedOffsetSeconds int NOT NULL,
    Instruction nvarchar(300) NULL,
    PRIMARY KEY (PointSequence)
);
GO

CREATE TYPE operations.TripStopTableType AS TABLE
(
    StopSequence smallint NOT NULL,
    StopTypeCode varchar(30) NOT NULL,
    LocationCode varchar(30) NOT NULL,
    ScheduledArrivalUtc datetime2(0) NULL,
    ScheduledDepartureUtc datetime2(0) NULL,
    Instructions nvarchar(1000) NULL,
    PRIMARY KEY (StopSequence)
);
GO

CREATE TYPE import.ImportErrorTableType AS TABLE
(
    RowNumber int NOT NULL,
    ColumnName nvarchar(128) NULL,
    RawValue nvarchar(1000) NULL,
    ErrorCode varchar(50) NOT NULL,
    ErrorMessage nvarchar(1000) NOT NULL,
    RawRowJson nvarchar(max) NULL
);
GO

CREATE OR ALTER FUNCTION security.fn_UserHasPermission
(
    @AppUserId bigint,
    @PermissionCode varchar(80)
)
RETURNS TABLE
AS
RETURN
(
    SELECT CAST(CASE WHEN EXISTS
    (
        SELECT 1
        FROM dbo.AppUsers AS u
        JOIN dbo.UserRoles AS ur ON ur.AppUserId = u.AppUserId
        JOIN dbo.Roles AS r ON r.RoleId = ur.RoleId AND r.IsActive = 1
        JOIN dbo.RolePermissions AS rp ON rp.RoleId = r.RoleId
        JOIN dbo.Permissions AS p ON p.PermissionId = rp.PermissionId
        WHERE u.AppUserId = @AppUserId
          AND u.IsActive = 1
          AND p.Code = @PermissionCode
    ) THEN 1 ELSE 0 END AS bit) AS HasPermission
);
GO

CREATE OR ALTER FUNCTION operations.fn_IsValidTripTransition
(
    @FromTripStatusId tinyint,
    @ToTripStatusId tinyint
)
RETURNS TABLE
AS
RETURN
(
    SELECT
        CAST(CASE WHEN t.FromTripStatusId IS NULL THEN 0 ELSE 1 END AS bit) AS IsValid,
        t.RequiredPermissionId,
        t.IsDriverAllowed,
        t.IsDispatchAllowed,
        t.IsSystemAllowed
    FROM (VALUES (1)) AS seed(Value)
    LEFT JOIN dbo.TripStatusTransitions AS t
      ON t.FromTripStatusId = @FromTripStatusId
     AND t.ToTripStatusId = @ToTripStatusId
);
GO

CREATE OR ALTER FUNCTION operations.fn_TripProgress
(
    @TripId bigint
)
RETURNS TABLE
AS
RETURN
(
    SELECT
        @TripId AS TripId,
        COUNT_BIG(*) AS TotalStops,
        SUM(CASE WHEN ss.Code = 'COMPLETED' THEN CONVERT(bigint, 1) ELSE CONVERT(bigint, 0) END) AS CompletedStops,
        CAST
        (
            CASE WHEN COUNT_BIG(*) = 0 THEN 0
                 ELSE 100.0 * SUM(CASE WHEN ss.Code = 'COMPLETED' THEN 1.0 ELSE 0.0 END) / COUNT_BIG(*)
            END AS decimal(5,2)
        ) AS ProgressPercent
    FROM dbo.TripStops AS s
    JOIN dbo.StopStatuses AS ss ON ss.StopStatusId = s.StopStatusId
    WHERE s.TripId = @TripId
);
GO

CREATE OR ALTER FUNCTION tracking.fn_DistanceMiles
(
    @Latitude1 decimal(9,6),
    @Longitude1 decimal(9,6),
    @Latitude2 decimal(9,6),
    @Longitude2 decimal(9,6)
)
RETURNS TABLE
AS
RETURN
(
    SELECT CAST
    (
        geography::Point(@Latitude1, @Longitude1, 4326)
            .STDistance(geography::Point(@Latitude2, @Longitude2, 4326)) / 1609.344
        AS decimal(12,3)
    ) AS DistanceMiles
);
GO
