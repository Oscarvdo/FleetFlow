/*
    FleetFlow MVP database for SQL Server 2022+
    Scope: customers, drivers, trucks, trailers, loads, trips, stops,
           assignments, trip status history, and simulated trip events.
*/

IF DB_ID(N'FleetFlowDb') IS NULL
BEGIN
    CREATE DATABASE FleetFlowDb;
END;
GO

USE FleetFlowDb;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET XACT_ABORT ON;
GO

CREATE TABLE dbo.FleetAssetStatuses
(
    FleetAssetStatusId tinyint NOT NULL,
    Code varchar(30) NOT NULL,
    DisplayName nvarchar(60) NOT NULL,
    IsAvailable bit NOT NULL,
    SortOrder tinyint NOT NULL,
    CONSTRAINT PK_FleetAssetStatuses PRIMARY KEY (FleetAssetStatusId),
    CONSTRAINT UQ_FleetAssetStatuses_Code UNIQUE (Code)
);
GO

CREATE TABLE dbo.DriverStatuses
(
    DriverStatusId tinyint NOT NULL,
    Code varchar(30) NOT NULL,
    DisplayName nvarchar(60) NOT NULL,
    IsAvailable bit NOT NULL,
    SortOrder tinyint NOT NULL,
    CONSTRAINT PK_DriverStatuses PRIMARY KEY (DriverStatusId),
    CONSTRAINT UQ_DriverStatuses_Code UNIQUE (Code)
);
GO

CREATE TABLE dbo.LoadStatuses
(
    LoadStatusId tinyint NOT NULL,
    Code varchar(30) NOT NULL,
    DisplayName nvarchar(60) NOT NULL,
    SortOrder tinyint NOT NULL,
    CONSTRAINT PK_LoadStatuses PRIMARY KEY (LoadStatusId),
    CONSTRAINT UQ_LoadStatuses_Code UNIQUE (Code)
);
GO

CREATE TABLE dbo.TripStatuses
(
    TripStatusId tinyint NOT NULL,
    Code varchar(30) NOT NULL,
    DisplayName nvarchar(60) NOT NULL,
    IsTerminal bit NOT NULL,
    SortOrder tinyint NOT NULL,
    CONSTRAINT PK_TripStatuses PRIMARY KEY (TripStatusId),
    CONSTRAINT UQ_TripStatuses_Code UNIQUE (Code)
);
GO

CREATE TABLE dbo.AssignmentStatuses
(
    AssignmentStatusId tinyint NOT NULL,
    Code varchar(30) NOT NULL,
    DisplayName nvarchar(60) NOT NULL,
    IsTerminal bit NOT NULL,
    SortOrder tinyint NOT NULL,
    CONSTRAINT PK_AssignmentStatuses PRIMARY KEY (AssignmentStatusId),
    CONSTRAINT UQ_AssignmentStatuses_Code UNIQUE (Code)
);
GO

CREATE TABLE dbo.StopTypes
(
    StopTypeId tinyint NOT NULL,
    Code varchar(30) NOT NULL,
    DisplayName nvarchar(60) NOT NULL,
    SortOrder tinyint NOT NULL,
    CONSTRAINT PK_StopTypes PRIMARY KEY (StopTypeId),
    CONSTRAINT UQ_StopTypes_Code UNIQUE (Code)
);
GO

CREATE TABLE dbo.StopStatuses
(
    StopStatusId tinyint NOT NULL,
    Code varchar(30) NOT NULL,
    DisplayName nvarchar(60) NOT NULL,
    SortOrder tinyint NOT NULL,
    CONSTRAINT PK_StopStatuses PRIMARY KEY (StopStatusId),
    CONSTRAINT UQ_StopStatuses_Code UNIQUE (Code)
);
GO

CREATE TABLE dbo.DataOrigins
(
    DataOriginId tinyint NOT NULL,
    Code varchar(30) NOT NULL,
    DisplayName nvarchar(60) NOT NULL,
    IsSynthetic bit NOT NULL,
    SortOrder tinyint NOT NULL,
    CONSTRAINT PK_DataOrigins PRIMARY KEY (DataOriginId),
    CONSTRAINT UQ_DataOrigins_Code UNIQUE (Code)
);
GO

CREATE TABLE dbo.ImportBatches
(
    ImportBatchId bigint IDENTITY(1,1) NOT NULL,
    ImportBatchUid uniqueidentifier NOT NULL CONSTRAINT DF_ImportBatches_Uid DEFAULT (NEWID()),
    DataOriginId tinyint NOT NULL,
    EntityType varchar(40) NOT NULL,
    FileName nvarchar(260) NOT NULL,
    FileSha256 char(64) NOT NULL,
    Status varchar(20) NOT NULL,
    StartedAtUtc datetime2(3) NOT NULL CONSTRAINT DF_ImportBatches_StartedAtUtc DEFAULT (SYSUTCDATETIME()),
    CompletedAtUtc datetime2(3) NULL,
    TotalRows int NOT NULL CONSTRAINT DF_ImportBatches_TotalRows DEFAULT (0),
    ValidRows int NOT NULL CONSTRAINT DF_ImportBatches_ValidRows DEFAULT (0),
    InvalidRows int NOT NULL CONSTRAINT DF_ImportBatches_InvalidRows DEFAULT (0),
    ImportedRows int NOT NULL CONSTRAINT DF_ImportBatches_ImportedRows DEFAULT (0),
    Notes nvarchar(1000) NULL,
    CONSTRAINT PK_ImportBatches PRIMARY KEY (ImportBatchId),
    CONSTRAINT UQ_ImportBatches_Uid UNIQUE (ImportBatchUid),
    CONSTRAINT CK_ImportBatches_EntityType CHECK
    (
        EntityType IN
        (
            'CUSTOMERS', 'LOCATIONS', 'DRIVERS', 'VEHICLES', 'TRAILERS',
            'LOADS', 'TRIPS', 'TRIP_STOPS', 'ROUTE_POINTS', 'TELEMETRY', 'TRIP_EVENTS'
        )
    ),
    CONSTRAINT CK_ImportBatches_Status CHECK
    (
        Status IN ('PENDING', 'VALIDATING', 'READY', 'IMPORTING', 'COMPLETED', 'COMPLETED_WITH_ERRORS', 'FAILED', 'CANCELLED')
    ),
    CONSTRAINT CK_ImportBatches_Completion CHECK
    (
        CompletedAtUtc IS NULL OR CompletedAtUtc >= StartedAtUtc
    ),
    CONSTRAINT CK_ImportBatches_Counts CHECK
    (
        TotalRows >= 0 AND ValidRows >= 0 AND InvalidRows >= 0 AND ImportedRows >= 0 AND
        ValidRows + InvalidRows <= TotalRows AND ImportedRows <= ValidRows
    ),
    CONSTRAINT CK_ImportBatches_CsvOrigin CHECK (DataOriginId = 2),
    CONSTRAINT CK_ImportBatches_FileHash CHECK (FileSha256 NOT LIKE '%[^0-9A-Fa-f]%'),
    CONSTRAINT FK_ImportBatches_DataOrigins FOREIGN KEY (DataOriginId)
        REFERENCES dbo.DataOrigins (DataOriginId)
);
GO

CREATE INDEX IX_ImportBatches_FileHash
    ON dbo.ImportBatches (FileSha256, EntityType, StartedAtUtc DESC);
GO

CREATE TABLE dbo.ImportBatchErrors
(
    ImportBatchErrorId bigint IDENTITY(1,1) NOT NULL,
    ImportBatchId bigint NOT NULL,
    RowNumber int NOT NULL,
    ColumnName nvarchar(128) NULL,
    RawValue nvarchar(1000) NULL,
    ErrorCode varchar(50) NOT NULL,
    ErrorMessage nvarchar(1000) NOT NULL,
    RawRowJson nvarchar(max) NULL,
    CreatedAtUtc datetime2(3) NOT NULL CONSTRAINT DF_ImportBatchErrors_CreatedAtUtc DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT PK_ImportBatchErrors PRIMARY KEY (ImportBatchErrorId),
    CONSTRAINT CK_ImportBatchErrors_RowNumber CHECK (RowNumber > 0),
    CONSTRAINT CK_ImportBatchErrors_RawRowJson CHECK (RawRowJson IS NULL OR ISJSON(RawRowJson) = 1),
    CONSTRAINT FK_ImportBatchErrors_ImportBatches FOREIGN KEY (ImportBatchId)
        REFERENCES dbo.ImportBatches (ImportBatchId)
);
GO

CREATE INDEX IX_ImportBatchErrors_Batch_Row
    ON dbo.ImportBatchErrors (ImportBatchId, RowNumber);
GO

CREATE TABLE dbo.SimulationRuns
(
    SimulationRunId bigint IDENTITY(1,1) NOT NULL,
    SimulationRunUid uniqueidentifier NOT NULL CONSTRAINT DF_SimulationRuns_Uid DEFAULT (NEWID()),
    Name nvarchar(120) NOT NULL,
    ScenarioCode varchar(40) NOT NULL,
    Status varchar(20) NOT NULL,
    RandomSeed int NOT NULL,
    TimeScale decimal(8,2) NOT NULL CONSTRAINT DF_SimulationRuns_TimeScale DEFAULT (1),
    UpdateIntervalMilliseconds int NOT NULL CONSTRAINT DF_SimulationRuns_Interval DEFAULT (2000),
    PlannedVehicleCount int NOT NULL,
    ConfigurationJson nvarchar(max) NULL,
    StartedAtUtc datetime2(3) NULL,
    EndedAtUtc datetime2(3) NULL,
    CreatedAtUtc datetime2(3) NOT NULL CONSTRAINT DF_SimulationRuns_CreatedAtUtc DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT PK_SimulationRuns PRIMARY KEY (SimulationRunId),
    CONSTRAINT UQ_SimulationRuns_Uid UNIQUE (SimulationRunUid),
    CONSTRAINT CK_SimulationRuns_Status CHECK
    (
        Status IN ('DRAFT', 'READY', 'RUNNING', 'PAUSED', 'COMPLETED', 'FAILED', 'CANCELLED')
    ),
    CONSTRAINT CK_SimulationRuns_TimeScale CHECK (TimeScale > 0 AND TimeScale <= 3600),
    CONSTRAINT CK_SimulationRuns_Interval CHECK (UpdateIntervalMilliseconds BETWEEN 100 AND 600000),
    CONSTRAINT CK_SimulationRuns_VehicleCount CHECK (PlannedVehicleCount > 0 AND PlannedVehicleCount <= 10000),
    CONSTRAINT CK_SimulationRuns_Times CHECK
    (
        EndedAtUtc IS NULL OR (StartedAtUtc IS NOT NULL AND EndedAtUtc >= StartedAtUtc)
    ),
    CONSTRAINT CK_SimulationRuns_ConfigurationJson CHECK
    (
        ConfigurationJson IS NULL OR ISJSON(ConfigurationJson) = 1
    )
);
GO

CREATE TABLE dbo.Customers
(
    CustomerId bigint IDENTITY(1,1) NOT NULL,
    CustomerNumber varchar(20) NOT NULL,
    CompanyName nvarchar(150) NOT NULL,
    ContactName nvarchar(120) NULL,
    Email varchar(254) NULL,
    Phone varchar(30) NULL,
    SourceImportBatchId bigint NULL,
    IsActive bit NOT NULL CONSTRAINT DF_Customers_IsActive DEFAULT (1),
    CreatedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_Customers_CreatedAtUtc DEFAULT (SYSUTCDATETIME()),
    UpdatedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_Customers_UpdatedAtUtc DEFAULT (SYSUTCDATETIME()),
    RowVersion rowversion NOT NULL,
    CONSTRAINT PK_Customers PRIMARY KEY (CustomerId),
    CONSTRAINT UQ_Customers_CustomerNumber UNIQUE (CustomerNumber),
    CONSTRAINT FK_Customers_ImportBatches FOREIGN KEY (SourceImportBatchId)
        REFERENCES dbo.ImportBatches (ImportBatchId)
);
GO

CREATE TABLE dbo.Locations
(
    LocationId bigint IDENTITY(1,1) NOT NULL,
    CustomerId bigint NULL,
    LocationCode varchar(30) NOT NULL,
    LocationType varchar(30) NOT NULL,
    LocationName nvarchar(150) NOT NULL,
    Address1 nvarchar(150) NOT NULL,
    Address2 nvarchar(150) NULL,
    City nvarchar(80) NOT NULL,
    StateCode char(2) NOT NULL,
    PostalCode varchar(10) NOT NULL,
    Latitude decimal(9,6) NULL,
    Longitude decimal(9,6) NULL,
    ContactName nvarchar(120) NULL,
    ContactPhone varchar(30) NULL,
    IsBillingLocation bit NOT NULL CONSTRAINT DF_Locations_IsBilling DEFAULT (0),
    IsActive bit NOT NULL CONSTRAINT DF_Locations_IsActive DEFAULT (1),
    SourceImportBatchId bigint NULL,
    CreatedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_Locations_CreatedAtUtc DEFAULT (SYSUTCDATETIME()),
    UpdatedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_Locations_UpdatedAtUtc DEFAULT (SYSUTCDATETIME()),
    RowVersion rowversion NOT NULL,
    CONSTRAINT PK_Locations PRIMARY KEY (LocationId),
    CONSTRAINT UQ_Locations_LocationCode UNIQUE (LocationCode),
    CONSTRAINT CK_Locations_Type CHECK
    (
        LocationType IN ('CUSTOMER', 'TERMINAL', 'FUEL', 'REST_AREA', 'OTHER')
    ),
    CONSTRAINT CK_Locations_Latitude CHECK (Latitude IS NULL OR Latitude BETWEEN -90 AND 90),
    CONSTRAINT CK_Locations_Longitude CHECK (Longitude IS NULL OR Longitude BETWEEN -180 AND 180),
    CONSTRAINT FK_Locations_Customers FOREIGN KEY (CustomerId)
        REFERENCES dbo.Customers (CustomerId),
    CONSTRAINT FK_Locations_ImportBatches FOREIGN KEY (SourceImportBatchId)
        REFERENCES dbo.ImportBatches (ImportBatchId)
);
GO

CREATE UNIQUE INDEX UX_Locations_OneBillingPerCustomer
    ON dbo.Locations (CustomerId)
    WHERE IsBillingLocation = 1 AND CustomerId IS NOT NULL;
GO

CREATE TABLE dbo.Drivers
(
    DriverId bigint IDENTITY(1,1) NOT NULL,
    DriverNumber varchar(20) NOT NULL,
    FirstName nvarchar(80) NOT NULL,
    LastName nvarchar(80) NOT NULL,
    Phone varchar(30) NULL,
    Email varchar(254) NULL,
    LicenseNumber varchar(40) NOT NULL,
    LicenseState char(2) NOT NULL,
    LicenseExpirationDate date NOT NULL,
    DriverStatusId tinyint NOT NULL CONSTRAINT DF_Drivers_Status DEFAULT (1),
    IsActive bit NOT NULL CONSTRAINT DF_Drivers_IsActive DEFAULT (1),
    SourceImportBatchId bigint NULL,
    CreatedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_Drivers_CreatedAtUtc DEFAULT (SYSUTCDATETIME()),
    UpdatedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_Drivers_UpdatedAtUtc DEFAULT (SYSUTCDATETIME()),
    RowVersion rowversion NOT NULL,
    CONSTRAINT PK_Drivers PRIMARY KEY (DriverId),
    CONSTRAINT UQ_Drivers_DriverNumber UNIQUE (DriverNumber),
    CONSTRAINT UQ_Drivers_License UNIQUE (LicenseState, LicenseNumber),
    CONSTRAINT FK_Drivers_DriverStatuses FOREIGN KEY (DriverStatusId)
        REFERENCES dbo.DriverStatuses (DriverStatusId),
    CONSTRAINT FK_Drivers_ImportBatches FOREIGN KEY (SourceImportBatchId)
        REFERENCES dbo.ImportBatches (ImportBatchId)
);
GO

CREATE TABLE dbo.Vehicles
(
    VehicleId bigint IDENTITY(1,1) NOT NULL,
    UnitNumber varchar(20) NOT NULL,
    Vin char(17) NOT NULL,
    ModelYear smallint NOT NULL,
    Make nvarchar(60) NOT NULL,
    Model nvarchar(80) NOT NULL,
    LicensePlate varchar(20) NOT NULL,
    LicenseState char(2) NOT NULL,
    MaxPayloadLbs decimal(12,2) NOT NULL,
    CurrentOdometerMiles decimal(12,1) NOT NULL CONSTRAINT DF_Vehicles_Odometer DEFAULT (0),
    FleetAssetStatusId tinyint NOT NULL CONSTRAINT DF_Vehicles_Status DEFAULT (1),
    IsActive bit NOT NULL CONSTRAINT DF_Vehicles_IsActive DEFAULT (1),
    SourceImportBatchId bigint NULL,
    CreatedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_Vehicles_CreatedAtUtc DEFAULT (SYSUTCDATETIME()),
    UpdatedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_Vehicles_UpdatedAtUtc DEFAULT (SYSUTCDATETIME()),
    RowVersion rowversion NOT NULL,
    CONSTRAINT PK_Vehicles PRIMARY KEY (VehicleId),
    CONSTRAINT UQ_Vehicles_UnitNumber UNIQUE (UnitNumber),
    CONSTRAINT UQ_Vehicles_Vin UNIQUE (Vin),
    CONSTRAINT UQ_Vehicles_Plate UNIQUE (LicenseState, LicensePlate),
    CONSTRAINT CK_Vehicles_ModelYear CHECK (ModelYear BETWEEN 1980 AND 2100),
    CONSTRAINT CK_Vehicles_MaxPayload CHECK (MaxPayloadLbs > 0),
    CONSTRAINT CK_Vehicles_Odometer CHECK (CurrentOdometerMiles >= 0),
    CONSTRAINT FK_Vehicles_FleetAssetStatuses FOREIGN KEY (FleetAssetStatusId)
        REFERENCES dbo.FleetAssetStatuses (FleetAssetStatusId),
    CONSTRAINT FK_Vehicles_ImportBatches FOREIGN KEY (SourceImportBatchId)
        REFERENCES dbo.ImportBatches (ImportBatchId)
);
GO

CREATE TABLE dbo.Trailers
(
    TrailerId bigint IDENTITY(1,1) NOT NULL,
    UnitNumber varchar(20) NOT NULL,
    Vin char(17) NOT NULL,
    TrailerType varchar(30) NOT NULL,
    LicensePlate varchar(20) NOT NULL,
    LicenseState char(2) NOT NULL,
    MaxPayloadLbs decimal(12,2) NOT NULL,
    FleetAssetStatusId tinyint NOT NULL CONSTRAINT DF_Trailers_Status DEFAULT (1),
    IsActive bit NOT NULL CONSTRAINT DF_Trailers_IsActive DEFAULT (1),
    SourceImportBatchId bigint NULL,
    CreatedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_Trailers_CreatedAtUtc DEFAULT (SYSUTCDATETIME()),
    UpdatedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_Trailers_UpdatedAtUtc DEFAULT (SYSUTCDATETIME()),
    RowVersion rowversion NOT NULL,
    CONSTRAINT PK_Trailers PRIMARY KEY (TrailerId),
    CONSTRAINT UQ_Trailers_UnitNumber UNIQUE (UnitNumber),
    CONSTRAINT UQ_Trailers_Vin UNIQUE (Vin),
    CONSTRAINT UQ_Trailers_Plate UNIQUE (LicenseState, LicensePlate),
    CONSTRAINT CK_Trailers_Type CHECK (TrailerType IN ('DRY_VAN', 'REEFER', 'FLATBED', 'TANKER', 'OTHER')),
    CONSTRAINT CK_Trailers_MaxPayload CHECK (MaxPayloadLbs > 0),
    CONSTRAINT FK_Trailers_FleetAssetStatuses FOREIGN KEY (FleetAssetStatusId)
        REFERENCES dbo.FleetAssetStatuses (FleetAssetStatusId),
    CONSTRAINT FK_Trailers_ImportBatches FOREIGN KEY (SourceImportBatchId)
        REFERENCES dbo.ImportBatches (ImportBatchId)
);
GO

CREATE TABLE dbo.Loads
(
    LoadId bigint IDENTITY(1,1) NOT NULL,
    LoadNumber varchar(30) NOT NULL,
    CustomerId bigint NOT NULL,
    Description nvarchar(300) NOT NULL,
    Commodity nvarchar(100) NULL,
    WeightLbs decimal(12,2) NOT NULL,
    Pieces int NULL,
    RevenueAmount decimal(14,2) NULL,
    SpecialInstructions nvarchar(1000) NULL,
    LoadStatusId tinyint NOT NULL CONSTRAINT DF_Loads_Status DEFAULT (1),
    SourceImportBatchId bigint NULL,
    CreatedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_Loads_CreatedAtUtc DEFAULT (SYSUTCDATETIME()),
    UpdatedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_Loads_UpdatedAtUtc DEFAULT (SYSUTCDATETIME()),
    RowVersion rowversion NOT NULL,
    CONSTRAINT PK_Loads PRIMARY KEY (LoadId),
    CONSTRAINT UQ_Loads_LoadNumber UNIQUE (LoadNumber),
    CONSTRAINT CK_Loads_Weight CHECK (WeightLbs > 0),
    CONSTRAINT CK_Loads_Pieces CHECK (Pieces IS NULL OR Pieces > 0),
    CONSTRAINT CK_Loads_Revenue CHECK (RevenueAmount IS NULL OR RevenueAmount >= 0),
    CONSTRAINT FK_Loads_Customers FOREIGN KEY (CustomerId)
        REFERENCES dbo.Customers (CustomerId),
    CONSTRAINT FK_Loads_LoadStatuses FOREIGN KEY (LoadStatusId)
        REFERENCES dbo.LoadStatuses (LoadStatusId),
    CONSTRAINT FK_Loads_ImportBatches FOREIGN KEY (SourceImportBatchId)
        REFERENCES dbo.ImportBatches (ImportBatchId)
);
GO

CREATE TABLE dbo.Trips
(
    TripId bigint IDENTITY(1,1) NOT NULL,
    TripNumber varchar(30) NOT NULL,
    LoadId bigint NOT NULL,
    TripStatusId tinyint NOT NULL CONSTRAINT DF_Trips_Status DEFAULT (1),
    ScheduledPickupUtc datetime2(0) NOT NULL,
    ScheduledDeliveryUtc datetime2(0) NOT NULL,
    ActualStartUtc datetime2(0) NULL,
    ActualDeliveryUtc datetime2(0) NULL,
    PlannedDistanceMiles decimal(10,2) NULL,
    ActualDistanceMiles decimal(10,2) NULL,
    Notes nvarchar(1000) NULL,
    SourceImportBatchId bigint NULL,
    CreatedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_Trips_CreatedAtUtc DEFAULT (SYSUTCDATETIME()),
    UpdatedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_Trips_UpdatedAtUtc DEFAULT (SYSUTCDATETIME()),
    RowVersion rowversion NOT NULL,
    CONSTRAINT PK_Trips PRIMARY KEY (TripId),
    CONSTRAINT UQ_Trips_TripNumber UNIQUE (TripNumber),
    CONSTRAINT UQ_Trips_LoadId UNIQUE (LoadId),
    CONSTRAINT CK_Trips_Schedule CHECK (ScheduledDeliveryUtc > ScheduledPickupUtc),
    CONSTRAINT CK_Trips_ActualDates CHECK
    (
        ActualDeliveryUtc IS NULL OR
        (ActualStartUtc IS NOT NULL AND ActualDeliveryUtc >= ActualStartUtc)
    ),
    CONSTRAINT CK_Trips_PlannedDistance CHECK (PlannedDistanceMiles IS NULL OR PlannedDistanceMiles >= 0),
    CONSTRAINT CK_Trips_ActualDistance CHECK (ActualDistanceMiles IS NULL OR ActualDistanceMiles >= 0),
    CONSTRAINT FK_Trips_Loads FOREIGN KEY (LoadId)
        REFERENCES dbo.Loads (LoadId),
    CONSTRAINT FK_Trips_TripStatuses FOREIGN KEY (TripStatusId)
        REFERENCES dbo.TripStatuses (TripStatusId),
    CONSTRAINT FK_Trips_ImportBatches FOREIGN KEY (SourceImportBatchId)
        REFERENCES dbo.ImportBatches (ImportBatchId)
);
GO

CREATE TABLE dbo.TripStops
(
    TripStopId bigint IDENTITY(1,1) NOT NULL,
    TripId bigint NOT NULL,
    StopSequence smallint NOT NULL,
    StopTypeId tinyint NOT NULL,
    StopStatusId tinyint NOT NULL CONSTRAINT DF_TripStops_Status DEFAULT (1),
    LocationId bigint NOT NULL,
    ScheduledArrivalUtc datetime2(0) NULL,
    ScheduledDepartureUtc datetime2(0) NULL,
    ActualArrivalUtc datetime2(0) NULL,
    ActualDepartureUtc datetime2(0) NULL,
    Instructions nvarchar(1000) NULL,
    SourceImportBatchId bigint NULL,
    CreatedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_TripStops_CreatedAtUtc DEFAULT (SYSUTCDATETIME()),
    UpdatedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_TripStops_UpdatedAtUtc DEFAULT (SYSUTCDATETIME()),
    RowVersion rowversion NOT NULL,
    CONSTRAINT PK_TripStops PRIMARY KEY (TripStopId),
    CONSTRAINT UQ_TripStops_TripSequence UNIQUE (TripId, StopSequence),
    CONSTRAINT CK_TripStops_Sequence CHECK (StopSequence > 0),
    CONSTRAINT CK_TripStops_ScheduledTimes CHECK
    (
        ScheduledDepartureUtc IS NULL OR ScheduledArrivalUtc IS NULL OR
        ScheduledDepartureUtc >= ScheduledArrivalUtc
    ),
    CONSTRAINT CK_TripStops_ActualTimes CHECK
    (
        ActualDepartureUtc IS NULL OR
        (ActualArrivalUtc IS NOT NULL AND ActualDepartureUtc >= ActualArrivalUtc)
    ),
    CONSTRAINT FK_TripStops_Trips FOREIGN KEY (TripId)
        REFERENCES dbo.Trips (TripId),
    CONSTRAINT FK_TripStops_Locations FOREIGN KEY (LocationId)
        REFERENCES dbo.Locations (LocationId),
    CONSTRAINT FK_TripStops_StopTypes FOREIGN KEY (StopTypeId)
        REFERENCES dbo.StopTypes (StopTypeId),
    CONSTRAINT FK_TripStops_StopStatuses FOREIGN KEY (StopStatusId)
        REFERENCES dbo.StopStatuses (StopStatusId),
    CONSTRAINT FK_TripStops_ImportBatches FOREIGN KEY (SourceImportBatchId)
        REFERENCES dbo.ImportBatches (ImportBatchId)
);
GO

CREATE TABLE dbo.TripRoutePoints
(
    TripRoutePointId bigint IDENTITY(1,1) NOT NULL,
    TripId bigint NOT NULL,
    PointSequence int NOT NULL,
    Latitude decimal(9,6) NOT NULL,
    Longitude decimal(9,6) NOT NULL,
    CumulativeDistanceMiles decimal(10,3) NOT NULL,
    ExpectedOffsetSeconds int NOT NULL,
    Instruction nvarchar(300) NULL,
    DataOriginId tinyint NOT NULL,
    SourceImportBatchId bigint NULL,
    CreatedAtUtc datetime2(3) NOT NULL CONSTRAINT DF_TripRoutePoints_CreatedAtUtc DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT PK_TripRoutePoints PRIMARY KEY (TripRoutePointId),
    CONSTRAINT UQ_TripRoutePoints_TripSequence UNIQUE (TripId, PointSequence),
    CONSTRAINT CK_TripRoutePoints_Sequence CHECK (PointSequence > 0),
    CONSTRAINT CK_TripRoutePoints_Latitude CHECK (Latitude BETWEEN -90 AND 90),
    CONSTRAINT CK_TripRoutePoints_Longitude CHECK (Longitude BETWEEN -180 AND 180),
    CONSTRAINT CK_TripRoutePoints_Distance CHECK (CumulativeDistanceMiles >= 0),
    CONSTRAINT CK_TripRoutePoints_Offset CHECK (ExpectedOffsetSeconds >= 0),
    CONSTRAINT CK_TripRoutePoints_Provenance CHECK
    (
        (DataOriginId <> 2 OR SourceImportBatchId IS NOT NULL)
    ),
    CONSTRAINT FK_TripRoutePoints_Trips FOREIGN KEY (TripId)
        REFERENCES dbo.Trips (TripId),
    CONSTRAINT FK_TripRoutePoints_DataOrigins FOREIGN KEY (DataOriginId)
        REFERENCES dbo.DataOrigins (DataOriginId),
    CONSTRAINT FK_TripRoutePoints_ImportBatches FOREIGN KEY (SourceImportBatchId)
        REFERENCES dbo.ImportBatches (ImportBatchId)
);
GO

CREATE TABLE dbo.TripAssignments
(
    TripAssignmentId bigint IDENTITY(1,1) NOT NULL,
    TripId bigint NOT NULL,
    DriverId bigint NOT NULL,
    VehicleId bigint NOT NULL,
    TrailerId bigint NULL,
    AssignmentStatusId tinyint NOT NULL CONSTRAINT DF_TripAssignments_Status DEFAULT (1),
    IsActive bit NOT NULL CONSTRAINT DF_TripAssignments_IsActive DEFAULT (1),
    OfferedAtUtc datetime2(0) NULL,
    RespondedAtUtc datetime2(0) NULL,
    AcceptedAtUtc datetime2(0) NULL,
    RejectedAtUtc datetime2(0) NULL,
    CompletedAtUtc datetime2(0) NULL,
    DriverResponseNotes nvarchar(500) NULL,
    AssignedBy nvarchar(120) NOT NULL,
    CreatedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_TripAssignments_CreatedAtUtc DEFAULT (SYSUTCDATETIME()),
    UpdatedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_TripAssignments_UpdatedAtUtc DEFAULT (SYSUTCDATETIME()),
    RowVersion rowversion NOT NULL,
    CONSTRAINT PK_TripAssignments PRIMARY KEY (TripAssignmentId),
    CONSTRAINT CK_TripAssignments_Response CHECK
    (
        (AcceptedAtUtc IS NULL OR RejectedAtUtc IS NULL) AND
        (RespondedAtUtc IS NOT NULL OR (AcceptedAtUtc IS NULL AND RejectedAtUtc IS NULL))
    ),
    CONSTRAINT CK_TripAssignments_ActiveCompletion CHECK
    (
        CompletedAtUtc IS NULL OR IsActive = 0
    ),
    CONSTRAINT FK_TripAssignments_Trips FOREIGN KEY (TripId)
        REFERENCES dbo.Trips (TripId),
    CONSTRAINT FK_TripAssignments_Drivers FOREIGN KEY (DriverId)
        REFERENCES dbo.Drivers (DriverId),
    CONSTRAINT FK_TripAssignments_Vehicles FOREIGN KEY (VehicleId)
        REFERENCES dbo.Vehicles (VehicleId),
    CONSTRAINT FK_TripAssignments_Trailers FOREIGN KEY (TrailerId)
        REFERENCES dbo.Trailers (TrailerId),
    CONSTRAINT FK_TripAssignments_AssignmentStatuses FOREIGN KEY (AssignmentStatusId)
        REFERENCES dbo.AssignmentStatuses (AssignmentStatusId)
);
GO

CREATE TABLE dbo.TripStatusHistory
(
    TripStatusHistoryId bigint IDENTITY(1,1) NOT NULL,
    TripId bigint NOT NULL,
    PreviousTripStatusId tinyint NULL,
    NewTripStatusId tinyint NOT NULL,
    ChangedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_TripStatusHistory_ChangedAtUtc DEFAULT (SYSUTCDATETIME()),
    ChangedBy nvarchar(120) NOT NULL,
    Source varchar(30) NOT NULL,
    Notes nvarchar(500) NULL,
    CONSTRAINT PK_TripStatusHistory PRIMARY KEY (TripStatusHistoryId),
    CONSTRAINT CK_TripStatusHistory_Source CHECK (Source IN ('DISPATCH', 'DRIVER_APP', 'SIMULATOR', 'SYSTEM')),
    CONSTRAINT FK_TripStatusHistory_Trips FOREIGN KEY (TripId)
        REFERENCES dbo.Trips (TripId),
    CONSTRAINT FK_TripStatusHistory_PreviousStatus FOREIGN KEY (PreviousTripStatusId)
        REFERENCES dbo.TripStatuses (TripStatusId),
    CONSTRAINT FK_TripStatusHistory_NewStatus FOREIGN KEY (NewTripStatusId)
        REFERENCES dbo.TripStatuses (TripStatusId)
);
GO

CREATE TABLE dbo.TripEvents
(
    TripEventId bigint IDENTITY(1,1) NOT NULL,
    EventId uniqueidentifier NOT NULL CONSTRAINT DF_TripEvents_EventId DEFAULT (NEWID()),
    TripId bigint NOT NULL,
    VehicleId bigint NOT NULL,
    TripStopId bigint NULL,
    EventType varchar(40) NOT NULL,
    OccurredAtUtc datetime2(3) NOT NULL,
    ReceivedAtUtc datetime2(3) NOT NULL CONSTRAINT DF_TripEvents_ReceivedAtUtc DEFAULT (SYSUTCDATETIME()),
    CorrelationId uniqueidentifier NULL,
    DataOriginId tinyint NOT NULL,
    ImportBatchId bigint NULL,
    SimulationRunId bigint NULL,
    Message nvarchar(500) NULL,
    PayloadJson nvarchar(max) NULL,
    CreatedAtUtc datetime2(3) NOT NULL CONSTRAINT DF_TripEvents_CreatedAtUtc DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT PK_TripEvents PRIMARY KEY (TripEventId),
    CONSTRAINT UQ_TripEvents_EventId UNIQUE (EventId),
    CONSTRAINT CK_TripEvents_EventType CHECK
    (
        EventType IN
        (
            'STATUS_CHANGE', 'ARRIVAL', 'DEPARTURE',
            'DELAY', 'LOW_FUEL', 'BREAKDOWN', 'CONNECTION_LOST',
            'CONNECTION_RESTORED', 'PICKUP_CONFIRMED', 'DELIVERY_CONFIRMED'
        )
    ),
    CONSTRAINT CK_TripEvents_PayloadJson CHECK (PayloadJson IS NULL OR ISJSON(PayloadJson) = 1),
    CONSTRAINT CK_TripEvents_Provenance CHECK
    (
        NOT (ImportBatchId IS NOT NULL AND SimulationRunId IS NOT NULL) AND
        (DataOriginId <> 2 OR ImportBatchId IS NOT NULL) AND
        (DataOriginId <> 3 OR SimulationRunId IS NOT NULL)
    ),
    CONSTRAINT FK_TripEvents_Trips FOREIGN KEY (TripId)
        REFERENCES dbo.Trips (TripId),
    CONSTRAINT FK_TripEvents_Vehicles FOREIGN KEY (VehicleId)
        REFERENCES dbo.Vehicles (VehicleId),
    CONSTRAINT FK_TripEvents_TripStops FOREIGN KEY (TripStopId)
        REFERENCES dbo.TripStops (TripStopId),
    CONSTRAINT FK_TripEvents_DataOrigins FOREIGN KEY (DataOriginId)
        REFERENCES dbo.DataOrigins (DataOriginId),
    CONSTRAINT FK_TripEvents_ImportBatches FOREIGN KEY (ImportBatchId)
        REFERENCES dbo.ImportBatches (ImportBatchId),
    CONSTRAINT FK_TripEvents_SimulationRuns FOREIGN KEY (SimulationRunId)
        REFERENCES dbo.SimulationRuns (SimulationRunId)
);
GO

CREATE TABLE dbo.VehicleTelemetry
(
    VehicleTelemetryId bigint IDENTITY(1,1) NOT NULL,
    TelemetryId uniqueidentifier NOT NULL CONSTRAINT DF_VehicleTelemetry_TelemetryId DEFAULT (NEWID()),
    VehicleId bigint NOT NULL,
    TripId bigint NULL,
    RecordedAtUtc datetime2(3) NOT NULL,
    ReceivedAtUtc datetime2(3) NOT NULL CONSTRAINT DF_VehicleTelemetry_ReceivedAtUtc DEFAULT (SYSUTCDATETIME()),
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
    CONSTRAINT PK_VehicleTelemetry PRIMARY KEY (VehicleTelemetryId),
    CONSTRAINT UQ_VehicleTelemetry_TelemetryId UNIQUE (TelemetryId),
    CONSTRAINT CK_VehicleTelemetry_Sequence CHECK (SequenceNumber IS NULL OR SequenceNumber >= 0),
    CONSTRAINT CK_VehicleTelemetry_Latitude CHECK (Latitude BETWEEN -90 AND 90),
    CONSTRAINT CK_VehicleTelemetry_Longitude CHECK (Longitude BETWEEN -180 AND 180),
    CONSTRAINT CK_VehicleTelemetry_Speed CHECK (SpeedMph IS NULL OR SpeedMph BETWEEN 0 AND 120),
    CONSTRAINT CK_VehicleTelemetry_Fuel CHECK (FuelPercent IS NULL OR FuelPercent BETWEEN 0 AND 100),
    CONSTRAINT CK_VehicleTelemetry_Odometer CHECK (OdometerMiles IS NULL OR OdometerMiles >= 0),
    CONSTRAINT CK_VehicleTelemetry_Heading CHECK (HeadingDegrees IS NULL OR HeadingDegrees BETWEEN 0 AND 360),
    CONSTRAINT CK_VehicleTelemetry_PayloadJson CHECK (PayloadJson IS NULL OR ISJSON(PayloadJson) = 1),
    CONSTRAINT CK_VehicleTelemetry_Provenance CHECK
    (
        NOT (ImportBatchId IS NOT NULL AND SimulationRunId IS NOT NULL) AND
        (DataOriginId <> 2 OR ImportBatchId IS NOT NULL) AND
        (DataOriginId <> 3 OR SimulationRunId IS NOT NULL)
    ),
    CONSTRAINT FK_VehicleTelemetry_Vehicles FOREIGN KEY (VehicleId)
        REFERENCES dbo.Vehicles (VehicleId),
    CONSTRAINT FK_VehicleTelemetry_Trips FOREIGN KEY (TripId)
        REFERENCES dbo.Trips (TripId),
    CONSTRAINT FK_VehicleTelemetry_DataOrigins FOREIGN KEY (DataOriginId)
        REFERENCES dbo.DataOrigins (DataOriginId),
    CONSTRAINT FK_VehicleTelemetry_ImportBatches FOREIGN KEY (ImportBatchId)
        REFERENCES dbo.ImportBatches (ImportBatchId),
    CONSTRAINT FK_VehicleTelemetry_SimulationRuns FOREIGN KEY (SimulationRunId)
        REFERENCES dbo.SimulationRuns (SimulationRunId)
);
GO

CREATE UNIQUE INDEX UX_TripAssignments_ActiveTrip
    ON dbo.TripAssignments (TripId)
    WHERE IsActive = 1;
GO

CREATE UNIQUE INDEX UX_TripAssignments_ActiveDriver
    ON dbo.TripAssignments (DriverId)
    WHERE IsActive = 1;
GO

CREATE UNIQUE INDEX UX_TripAssignments_ActiveVehicle
    ON dbo.TripAssignments (VehicleId)
    WHERE IsActive = 1;
GO

CREATE UNIQUE INDEX UX_TripAssignments_ActiveTrailer
    ON dbo.TripAssignments (TrailerId)
    WHERE IsActive = 1 AND TrailerId IS NOT NULL;
GO

CREATE INDEX IX_Trips_Status_ScheduledPickup
    ON dbo.Trips (TripStatusId, ScheduledPickupUtc);
GO

CREATE INDEX IX_TripStops_Trip_Sequence
    ON dbo.TripStops (TripId, StopSequence);
GO

CREATE INDEX IX_TripRoutePoints_Trip_Sequence
    ON dbo.TripRoutePoints (TripId, PointSequence)
    INCLUDE (Latitude, Longitude, CumulativeDistanceMiles, ExpectedOffsetSeconds);
GO

CREATE INDEX IX_TripEvents_Trip_OccurredAt
    ON dbo.TripEvents (TripId, OccurredAtUtc DESC);
GO

CREATE INDEX IX_TripEvents_Vehicle_OccurredAt
    ON dbo.TripEvents (VehicleId, OccurredAtUtc DESC);
GO

CREATE INDEX IX_VehicleTelemetry_Vehicle_RecordedAt
    ON dbo.VehicleTelemetry (VehicleId, RecordedAtUtc DESC)
    INCLUDE (TripId, Latitude, Longitude, SpeedMph, FuelPercent, OdometerMiles);
GO

CREATE INDEX IX_VehicleTelemetry_Trip_RecordedAt
    ON dbo.VehicleTelemetry (TripId, RecordedAtUtc DESC)
    INCLUDE (VehicleId, Latitude, Longitude, SpeedMph, FuelPercent)
    WHERE TripId IS NOT NULL;
GO

CREATE UNIQUE INDEX UX_VehicleTelemetry_Vehicle_Sequence
    ON dbo.VehicleTelemetry (VehicleId, SequenceNumber)
    WHERE SequenceNumber IS NOT NULL;
GO

CREATE INDEX IX_VehicleTelemetry_SimulationRun_RecordedAt
    ON dbo.VehicleTelemetry (SimulationRunId, RecordedAtUtc)
    INCLUDE (VehicleId, TripId, Latitude, Longitude, SpeedMph, FuelPercent)
    WHERE SimulationRunId IS NOT NULL;
GO

CREATE INDEX IX_TripStatusHistory_Trip_ChangedAt
    ON dbo.TripStatusHistory (TripId, ChangedAtUtc DESC);
GO
