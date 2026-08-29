/* FleetFlow database API schemas and operational support tables. */
USE FleetFlowDb;
GO

SET XACT_ABORT ON;
GO

IF SCHEMA_ID(N'catalog') IS NULL EXEC(N'CREATE SCHEMA catalog AUTHORIZATION dbo;');
IF SCHEMA_ID(N'operations') IS NULL EXEC(N'CREATE SCHEMA operations AUTHORIZATION dbo;');
IF SCHEMA_ID(N'dispatch') IS NULL EXEC(N'CREATE SCHEMA dispatch AUTHORIZATION dbo;');
IF SCHEMA_ID(N'tracking') IS NULL EXEC(N'CREATE SCHEMA tracking AUTHORIZATION dbo;');
IF SCHEMA_ID(N'import') IS NULL EXEC(N'CREATE SCHEMA import AUTHORIZATION dbo;');
IF SCHEMA_ID(N'simulation') IS NULL EXEC(N'CREATE SCHEMA simulation AUTHORIZATION dbo;');
IF SCHEMA_ID(N'security') IS NULL EXEC(N'CREATE SCHEMA security AUTHORIZATION dbo;');
IF SCHEMA_ID(N'reporting') IS NULL EXEC(N'CREATE SCHEMA reporting AUTHORIZATION dbo;');
GO

CREATE TABLE dbo.TripStatusTransitions
(
    FromTripStatusId tinyint NOT NULL,
    ToTripStatusId tinyint NOT NULL,
    RequiredPermissionId smallint NULL,
    IsDriverAllowed bit NOT NULL CONSTRAINT DF_TripStatusTransitions_Driver DEFAULT (0),
    IsDispatchAllowed bit NOT NULL CONSTRAINT DF_TripStatusTransitions_Dispatch DEFAULT (1),
    IsSystemAllowed bit NOT NULL CONSTRAINT DF_TripStatusTransitions_System DEFAULT (1),
    CONSTRAINT PK_TripStatusTransitions PRIMARY KEY (FromTripStatusId, ToTripStatusId),
    CONSTRAINT CK_TripStatusTransitions_Different CHECK (FromTripStatusId <> ToTripStatusId),
    CONSTRAINT FK_TripStatusTransitions_From FOREIGN KEY (FromTripStatusId)
        REFERENCES dbo.TripStatuses (TripStatusId),
    CONSTRAINT FK_TripStatusTransitions_To FOREIGN KEY (ToTripStatusId)
        REFERENCES dbo.TripStatuses (TripStatusId),
    CONSTRAINT FK_TripStatusTransitions_Permissions FOREIGN KEY (RequiredPermissionId)
        REFERENCES dbo.Permissions (PermissionId)
);
GO

CREATE TABLE dbo.VehicleCurrentState
(
    VehicleId bigint NOT NULL,
    TripId bigint NULL,
    LastVehicleTelemetryId bigint NOT NULL,
    SimulationRunId bigint NULL,
    RecordedAtUtc datetime2(3) NOT NULL,
    Latitude decimal(9,6) NOT NULL,
    Longitude decimal(9,6) NOT NULL,
    SpeedMph decimal(6,2) NULL,
    FuelPercent decimal(5,2) NULL,
    OdometerMiles decimal(12,1) NULL,
    HeadingDegrees decimal(6,2) NULL,
    UpdatedAtUtc datetime2(3) NOT NULL CONSTRAINT DF_VehicleCurrentState_Updated DEFAULT (SYSUTCDATETIME()),
    RowVersion rowversion NOT NULL,
    CONSTRAINT PK_VehicleCurrentState PRIMARY KEY (VehicleId),
    CONSTRAINT UQ_VehicleCurrentState_Telemetry UNIQUE (LastVehicleTelemetryId),
    CONSTRAINT CK_VehicleCurrentState_Latitude CHECK (Latitude BETWEEN -90 AND 90),
    CONSTRAINT CK_VehicleCurrentState_Longitude CHECK (Longitude BETWEEN -180 AND 180),
    CONSTRAINT CK_VehicleCurrentState_Speed CHECK (SpeedMph IS NULL OR SpeedMph BETWEEN 0 AND 150),
    CONSTRAINT CK_VehicleCurrentState_Fuel CHECK (FuelPercent IS NULL OR FuelPercent BETWEEN 0 AND 100),
    CONSTRAINT FK_VehicleCurrentState_Vehicles FOREIGN KEY (VehicleId)
        REFERENCES dbo.Vehicles (VehicleId),
    CONSTRAINT FK_VehicleCurrentState_Trips FOREIGN KEY (TripId)
        REFERENCES dbo.Trips (TripId),
    CONSTRAINT FK_VehicleCurrentState_Telemetry FOREIGN KEY (LastVehicleTelemetryId)
        REFERENCES dbo.VehicleTelemetry (VehicleTelemetryId),
    CONSTRAINT FK_VehicleCurrentState_SimulationRuns FOREIGN KEY (SimulationRunId)
        REFERENCES dbo.SimulationRuns (SimulationRunId)
);
GO

CREATE INDEX IX_VehicleCurrentState_Trip
    ON dbo.VehicleCurrentState (TripId, RecordedAtUtc DESC)
    INCLUDE (VehicleId, Latitude, Longitude, SpeedMph, FuelPercent)
    WHERE TripId IS NOT NULL;
GO

CREATE TABLE dbo.ChangeAuditLog
(
    ChangeAuditLogId bigint IDENTITY(1,1) NOT NULL,
    SchemaName sysname NOT NULL,
    TableName sysname NOT NULL,
    RecordKey nvarchar(200) NOT NULL,
    Action varchar(10) NOT NULL,
    ChangedByAppUserId bigint NULL,
    ChangedBy nvarchar(120) NOT NULL,
    ChangedAtUtc datetime2(3) NOT NULL CONSTRAINT DF_ChangeAuditLog_Changed DEFAULT (SYSUTCDATETIME()),
    CorrelationId uniqueidentifier NULL,
    ClientApplication varchar(40) NOT NULL,
    BeforeJson nvarchar(max) NULL,
    AfterJson nvarchar(max) NULL,
    CONSTRAINT PK_ChangeAuditLog PRIMARY KEY (ChangeAuditLogId),
    CONSTRAINT CK_ChangeAuditLog_Action CHECK (Action IN ('INSERT', 'UPDATE', 'DELETE')),
    CONSTRAINT CK_ChangeAuditLog_Client CHECK
        (ClientApplication IN ('DISPATCH_WINFORMS', 'DRIVER_MAUI', 'API', 'SIMULATOR', 'SYSTEM')),
    CONSTRAINT CK_ChangeAuditLog_BeforeJson CHECK (BeforeJson IS NULL OR ISJSON(BeforeJson) = 1),
    CONSTRAINT CK_ChangeAuditLog_AfterJson CHECK (AfterJson IS NULL OR ISJSON(AfterJson) = 1),
    CONSTRAINT FK_ChangeAuditLog_AppUsers FOREIGN KEY (ChangedByAppUserId)
        REFERENCES dbo.AppUsers (AppUserId)
);
GO

CREATE INDEX IX_ChangeAuditLog_Record
    ON dbo.ChangeAuditLog (SchemaName, TableName, RecordKey, ChangedAtUtc DESC);
GO

CREATE TABLE dbo.DataRetentionPolicies
(
    DataType varchar(40) NOT NULL,
    HotRetentionDays int NOT NULL,
    ArchiveAfterDays int NULL,
    DeleteAfterDays int NULL,
    IsEnabled bit NOT NULL CONSTRAINT DF_DataRetentionPolicies_Enabled DEFAULT (1),
    UpdatedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_DataRetentionPolicies_Updated DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT PK_DataRetentionPolicies PRIMARY KEY (DataType),
    CONSTRAINT CK_DataRetentionPolicies_DataType CHECK
        (DataType IN ('VEHICLE_TELEMETRY', 'TRIP_EVENTS', 'SECURITY_AUDIT', 'CHANGE_AUDIT', 'IMPORT_ERRORS')),
    CONSTRAINT CK_DataRetentionPolicies_Days CHECK
    (
        HotRetentionDays > 0 AND
        (ArchiveAfterDays IS NULL OR ArchiveAfterDays >= HotRetentionDays) AND
        (DeleteAfterDays IS NULL OR DeleteAfterDays >= COALESCE(ArchiveAfterDays, HotRetentionDays))
    )
);
GO
