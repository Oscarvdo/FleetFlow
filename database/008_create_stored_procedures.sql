/* FleetFlow command API. Application writes should call these procedures. */
USE FleetFlowDb;
GO

CREATE OR ALTER PROCEDURE security.Permission_Check
    @AppUserId bigint,
    @PermissionCode varchar(80)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT HasPermission FROM security.fn_UserHasPermission(@AppUserId, @PermissionCode);
END;
GO

CREATE OR ALTER PROCEDURE security.AppUser_GetForLogin
    @NormalizedUsername nvarchar(80)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT AppUserId, Username, NormalizedUsername, Email, PasswordHash, SecurityStamp,
           DriverId, IsActive, MustChangePassword, FailedLoginAttempts, LockoutEndUtc,
           LastLoginAtUtc, RowVersion
    FROM dbo.AppUsers
    WHERE NormalizedUsername = @NormalizedUsername;
END;
GO

CREATE OR ALTER PROCEDURE security.AppUser_Create
    @Username nvarchar(80),
    @NormalizedUsername nvarchar(80),
    @Email varchar(254),
    @NormalizedEmail varchar(254),
    @PasswordHash nvarchar(500),
    @DriverId bigint = NULL,
    @RoleCode varchar(40),
    @CreatedByAppUserId bigint = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRANSACTION;
    DECLARE @RoleId smallint = (SELECT RoleId FROM dbo.Roles WHERE Code = @RoleCode AND IsActive = 1);
    IF @RoleId IS NULL THROW 51000, 'Unknown or inactive role.', 1;
    INSERT dbo.AppUsers
        (Username, NormalizedUsername, Email, NormalizedEmail, PasswordHash, DriverId)
    VALUES
        (@Username, @NormalizedUsername, @Email, @NormalizedEmail, @PasswordHash, @DriverId);
    DECLARE @AppUserId bigint = SCOPE_IDENTITY();
    INSERT dbo.UserRoles (AppUserId, RoleId, AssignedByAppUserId)
    VALUES (@AppUserId, @RoleId, @CreatedByAppUserId);
    INSERT dbo.SecurityAuditLog
        (AppUserId, UsernameAttempted, EventType, WasSuccessful, ClientApplication, Details)
    VALUES (@CreatedByAppUserId, @Username, 'USER_CREATED', 1, 'SYSTEM', CONCAT(N'Created AppUserId ', @AppUserId));
    COMMIT TRANSACTION;
    SELECT @AppUserId AS AppUserId;
END;
GO

CREATE OR ALTER PROCEDURE security.AppUser_RecordLogin
    @AppUserId bigint = NULL,
    @UsernameAttempted nvarchar(80),
    @WasSuccessful bit,
    @ClientApplication varchar(40),
    @DeviceIdentifier nvarchar(120) = NULL,
    @IpAddress varchar(45) = NULL,
    @LockoutMinutes int = 15,
    @MaximumFailedAttempts smallint = 5
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRANSACTION;
    IF @AppUserId IS NOT NULL
    BEGIN
        IF @WasSuccessful = 1
            UPDATE dbo.AppUsers
            SET FailedLoginAttempts = 0, LockoutEndUtc = NULL,
                LastLoginAtUtc = SYSUTCDATETIME(), UpdatedAtUtc = SYSUTCDATETIME()
            WHERE AppUserId = @AppUserId;
        ELSE
        BEGIN
            UPDATE dbo.AppUsers
            SET FailedLoginAttempts = FailedLoginAttempts + 1,
                LockoutEndUtc = CASE WHEN FailedLoginAttempts + 1 >= @MaximumFailedAttempts
                                     THEN DATEADD(minute, @LockoutMinutes, SYSUTCDATETIME())
                                     ELSE LockoutEndUtc END,
                UpdatedAtUtc = SYSUTCDATETIME()
            WHERE AppUserId = @AppUserId;
        END;
    END;
    INSERT dbo.SecurityAuditLog
        (AppUserId, UsernameAttempted, EventType, WasSuccessful, ClientApplication,
         DeviceIdentifier, IpAddress, Details)
    VALUES
        (@AppUserId, @UsernameAttempted,
         CASE WHEN @WasSuccessful = 1 THEN 'LOGIN' ELSE 'LOGIN_FAILED' END,
         @WasSuccessful, @ClientApplication, @DeviceIdentifier, @IpAddress, NULL);
    COMMIT TRANSACTION;
END;
GO

CREATE OR ALTER PROCEDURE security.Audit_Write
    @SchemaName sysname,
    @TableName sysname,
    @RecordKey nvarchar(200),
    @Action varchar(10),
    @ChangedByAppUserId bigint = NULL,
    @ChangedBy nvarchar(120),
    @ClientApplication varchar(40),
    @BeforeJson nvarchar(max) = NULL,
    @AfterJson nvarchar(max) = NULL,
    @CorrelationId uniqueidentifier = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT dbo.ChangeAuditLog
        (SchemaName, TableName, RecordKey, Action, ChangedByAppUserId, ChangedBy,
         ClientApplication, BeforeJson, AfterJson, CorrelationId)
    VALUES
        (@SchemaName, @TableName, @RecordKey, @Action, @ChangedByAppUserId, @ChangedBy,
         @ClientApplication, @BeforeJson, @AfterJson, @CorrelationId);
END;
GO

CREATE OR ALTER PROCEDURE catalog.Customer_Create
    @CustomerNumber varchar(20),
    @CompanyName nvarchar(150),
    @ContactName nvarchar(120) = NULL,
    @Email varchar(254) = NULL,
    @Phone varchar(30) = NULL,
    @SourceImportBatchId bigint = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT dbo.Customers
        (CustomerNumber, CompanyName, ContactName, Email, Phone, SourceImportBatchId)
    VALUES
        (@CustomerNumber, @CompanyName, @ContactName, @Email, @Phone, @SourceImportBatchId);
    SELECT CustomerId, RowVersion FROM dbo.Customers WHERE CustomerId = SCOPE_IDENTITY();
END;
GO

CREATE OR ALTER PROCEDURE catalog.Customer_Update
    @CustomerId bigint,
    @CompanyName nvarchar(150),
    @ContactName nvarchar(120) = NULL,
    @Email varchar(254) = NULL,
    @Phone varchar(30) = NULL,
    @ExpectedRowVersion binary(8)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Customers
    SET CompanyName = @CompanyName, ContactName = @ContactName, Email = @Email,
        Phone = @Phone, UpdatedAtUtc = SYSUTCDATETIME()
    WHERE CustomerId = @CustomerId AND RowVersion = @ExpectedRowVersion;
    IF @@ROWCOUNT = 0 THROW 51001, 'Customer was changed by another user or does not exist.', 1;
    SELECT CustomerId, RowVersion FROM dbo.Customers WHERE CustomerId = @CustomerId;
END;
GO

CREATE OR ALTER PROCEDURE catalog.Customer_SetActive
    @CustomerId bigint,
    @IsActive bit,
    @ExpectedRowVersion binary(8)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Customers SET IsActive = @IsActive, UpdatedAtUtc = SYSUTCDATETIME()
    WHERE CustomerId = @CustomerId AND RowVersion = @ExpectedRowVersion;
    IF @@ROWCOUNT = 0 THROW 51001, 'Customer was changed by another user or does not exist.', 1;
    SELECT CustomerId, RowVersion FROM dbo.Customers WHERE CustomerId = @CustomerId;
END;
GO

CREATE OR ALTER PROCEDURE catalog.Customer_Search
    @SearchText nvarchar(150) = NULL,
    @IncludeInactive bit = 0
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CustomerId, CustomerNumber, CompanyName, ContactName, Email, Phone, IsActive, RowVersion
    FROM dbo.Customers
    WHERE (@IncludeInactive = 1 OR IsActive = 1)
      AND (@SearchText IS NULL OR CustomerNumber LIKE '%' + @SearchText + '%'
           OR CompanyName LIKE '%' + @SearchText + '%')
    ORDER BY CompanyName;
END;
GO

CREATE OR ALTER PROCEDURE catalog.Location_Create
    @CustomerId bigint = NULL,
    @LocationCode varchar(30),
    @LocationType varchar(30),
    @LocationName nvarchar(150),
    @Address1 nvarchar(150),
    @Address2 nvarchar(150) = NULL,
    @City nvarchar(80),
    @StateCode char(2),
    @PostalCode varchar(10),
    @Latitude decimal(9,6) = NULL,
    @Longitude decimal(9,6) = NULL,
    @ContactName nvarchar(120) = NULL,
    @ContactPhone varchar(30) = NULL,
    @IsBillingLocation bit = 0,
    @SourceImportBatchId bigint = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT dbo.Locations
        (CustomerId, LocationCode, LocationType, LocationName, Address1, Address2,
         City, StateCode, PostalCode, Latitude, Longitude, ContactName, ContactPhone,
         IsBillingLocation, SourceImportBatchId)
    VALUES
        (@CustomerId, @LocationCode, @LocationType, @LocationName, @Address1, @Address2,
         @City, @StateCode, @PostalCode, @Latitude, @Longitude, @ContactName, @ContactPhone,
         @IsBillingLocation, @SourceImportBatchId);
    SELECT LocationId, RowVersion FROM dbo.Locations WHERE LocationId = SCOPE_IDENTITY();
END;
GO

CREATE OR ALTER PROCEDURE catalog.Driver_Create
    @DriverNumber varchar(20), @FirstName nvarchar(80), @LastName nvarchar(80),
    @Phone varchar(30) = NULL, @Email varchar(254) = NULL,
    @LicenseNumber varchar(40), @LicenseState char(2), @LicenseExpirationDate date,
    @DriverStatusCode varchar(30) = 'AVAILABLE', @SourceImportBatchId bigint = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @StatusId tinyint = (SELECT DriverStatusId FROM dbo.DriverStatuses WHERE Code = @DriverStatusCode);
    IF @StatusId IS NULL THROW 51002, 'Unknown driver status.', 1;
    INSERT dbo.Drivers
        (DriverNumber, FirstName, LastName, Phone, Email, LicenseNumber, LicenseState,
         LicenseExpirationDate, DriverStatusId, SourceImportBatchId)
    VALUES
        (@DriverNumber, @FirstName, @LastName, @Phone, @Email, @LicenseNumber, @LicenseState,
         @LicenseExpirationDate, @StatusId, @SourceImportBatchId);
    SELECT DriverId, RowVersion FROM dbo.Drivers WHERE DriverId = SCOPE_IDENTITY();
END;
GO

CREATE OR ALTER PROCEDURE catalog.Driver_SetStatus
    @DriverId bigint, @DriverStatusCode varchar(30), @ExpectedRowVersion binary(8)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @StatusId tinyint = (SELECT DriverStatusId FROM dbo.DriverStatuses WHERE Code = @DriverStatusCode);
    IF @StatusId IS NULL THROW 51002, 'Unknown driver status.', 1;
    UPDATE dbo.Drivers SET DriverStatusId = @StatusId, UpdatedAtUtc = SYSUTCDATETIME()
    WHERE DriverId = @DriverId AND RowVersion = @ExpectedRowVersion;
    IF @@ROWCOUNT = 0 THROW 51003, 'Driver was changed by another user or does not exist.', 1;
    SELECT DriverId, RowVersion FROM dbo.Drivers WHERE DriverId = @DriverId;
END;
GO

CREATE OR ALTER PROCEDURE catalog.Vehicle_Create
    @UnitNumber varchar(20), @Vin char(17), @ModelYear smallint,
    @Make nvarchar(60), @Model nvarchar(80), @LicensePlate varchar(20),
    @LicenseState char(2), @MaxPayloadLbs decimal(12,2),
    @CurrentOdometerMiles decimal(12,1) = 0,
    @FleetAssetStatusCode varchar(30) = 'AVAILABLE', @SourceImportBatchId bigint = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @StatusId tinyint = (SELECT FleetAssetStatusId FROM dbo.FleetAssetStatuses WHERE Code = @FleetAssetStatusCode);
    IF @StatusId IS NULL THROW 51004, 'Unknown fleet asset status.', 1;
    INSERT dbo.Vehicles
        (UnitNumber, Vin, ModelYear, Make, Model, LicensePlate, LicenseState,
         MaxPayloadLbs, CurrentOdometerMiles, FleetAssetStatusId, SourceImportBatchId)
    VALUES
        (@UnitNumber, @Vin, @ModelYear, @Make, @Model, @LicensePlate, @LicenseState,
         @MaxPayloadLbs, @CurrentOdometerMiles, @StatusId, @SourceImportBatchId);
    SELECT VehicleId, RowVersion FROM dbo.Vehicles WHERE VehicleId = SCOPE_IDENTITY();
END;
GO

CREATE OR ALTER PROCEDURE catalog.Vehicle_SetStatus
    @VehicleId bigint, @FleetAssetStatusCode varchar(30), @ExpectedRowVersion binary(8)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @StatusId tinyint = (SELECT FleetAssetStatusId FROM dbo.FleetAssetStatuses WHERE Code = @FleetAssetStatusCode);
    IF @StatusId IS NULL THROW 51004, 'Unknown fleet asset status.', 1;
    UPDATE dbo.Vehicles SET FleetAssetStatusId = @StatusId, UpdatedAtUtc = SYSUTCDATETIME()
    WHERE VehicleId = @VehicleId AND RowVersion = @ExpectedRowVersion;
    IF @@ROWCOUNT = 0 THROW 51005, 'Vehicle was changed by another user or does not exist.', 1;
    SELECT VehicleId, RowVersion FROM dbo.Vehicles WHERE VehicleId = @VehicleId;
END;
GO

CREATE OR ALTER PROCEDURE catalog.Trailer_Create
    @UnitNumber varchar(20), @Vin char(17), @TrailerType varchar(30),
    @LicensePlate varchar(20), @LicenseState char(2), @MaxPayloadLbs decimal(12,2),
    @FleetAssetStatusCode varchar(30) = 'AVAILABLE', @SourceImportBatchId bigint = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @StatusId tinyint = (SELECT FleetAssetStatusId FROM dbo.FleetAssetStatuses WHERE Code = @FleetAssetStatusCode);
    IF @StatusId IS NULL THROW 51004, 'Unknown fleet asset status.', 1;
    INSERT dbo.Trailers
        (UnitNumber, Vin, TrailerType, LicensePlate, LicenseState, MaxPayloadLbs,
         FleetAssetStatusId, SourceImportBatchId)
    VALUES
        (@UnitNumber, @Vin, @TrailerType, @LicensePlate, @LicenseState, @MaxPayloadLbs,
         @StatusId, @SourceImportBatchId);
    SELECT TrailerId, RowVersion FROM dbo.Trailers WHERE TrailerId = SCOPE_IDENTITY();
END;
GO

CREATE OR ALTER PROCEDURE catalog.Trailer_SetStatus
    @TrailerId bigint, @FleetAssetStatusCode varchar(30), @ExpectedRowVersion binary(8)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @StatusId tinyint = (SELECT FleetAssetStatusId FROM dbo.FleetAssetStatuses WHERE Code = @FleetAssetStatusCode);
    IF @StatusId IS NULL THROW 51004, 'Unknown fleet asset status.', 1;
    UPDATE dbo.Trailers SET FleetAssetStatusId = @StatusId, UpdatedAtUtc = SYSUTCDATETIME()
    WHERE TrailerId = @TrailerId AND RowVersion = @ExpectedRowVersion;
    IF @@ROWCOUNT = 0 THROW 51006, 'Trailer was changed by another user or does not exist.', 1;
    SELECT TrailerId, RowVersion FROM dbo.Trailers WHERE TrailerId = @TrailerId;
END;
GO

CREATE OR ALTER PROCEDURE operations.Load_Create
    @LoadNumber varchar(30), @CustomerId bigint, @Description nvarchar(300),
    @Commodity nvarchar(100) = NULL, @WeightLbs decimal(12,2), @Pieces int = NULL,
    @RevenueAmount decimal(14,2) = NULL, @SpecialInstructions nvarchar(1000) = NULL,
    @LoadStatusCode varchar(30) = 'NEW', @SourceImportBatchId bigint = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @StatusId tinyint = (SELECT LoadStatusId FROM dbo.LoadStatuses WHERE Code = @LoadStatusCode);
    IF @StatusId IS NULL THROW 51007, 'Unknown load status.', 1;
    INSERT dbo.Loads
        (LoadNumber, CustomerId, Description, Commodity, WeightLbs, Pieces,
         RevenueAmount, SpecialInstructions, LoadStatusId, SourceImportBatchId)
    VALUES
        (@LoadNumber, @CustomerId, @Description, @Commodity, @WeightLbs, @Pieces,
         @RevenueAmount, @SpecialInstructions, @StatusId, @SourceImportBatchId);
    SELECT LoadId, RowVersion FROM dbo.Loads WHERE LoadId = SCOPE_IDENTITY();
END;
GO

CREATE OR ALTER PROCEDURE operations.Trip_Create
    @TripNumber varchar(30), @LoadId bigint,
    @ScheduledPickupUtc datetime2(0), @ScheduledDeliveryUtc datetime2(0),
    @PlannedDistanceMiles decimal(10,2) = NULL, @Notes nvarchar(1000) = NULL,
    @SourceImportBatchId bigint = NULL, @ChangedBy nvarchar(120),
    @Stops operations.TripStopTableType READONLY
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    IF @ScheduledDeliveryUtc <= @ScheduledPickupUtc THROW 51008, 'Delivery must be after pickup.', 1;
    IF NOT EXISTS (SELECT 1 FROM @Stops WHERE StopTypeCode = 'PICKUP') THROW 51009, 'At least one pickup is required.', 1;
    IF NOT EXISTS (SELECT 1 FROM @Stops WHERE StopTypeCode = 'DELIVERY') THROW 51010, 'At least one delivery is required.', 1;
    IF EXISTS
    (
        SELECT 1 FROM @Stops AS s
        LEFT JOIN dbo.StopTypes AS st ON st.Code = s.StopTypeCode
        LEFT JOIN dbo.Locations AS loc ON loc.LocationCode = s.LocationCode AND loc.IsActive = 1
        WHERE st.StopTypeId IS NULL OR loc.LocationId IS NULL
    ) THROW 51011, 'A stop contains an unknown type or location.', 1;
    BEGIN TRANSACTION;
    INSERT dbo.Trips
        (TripNumber, LoadId, TripStatusId, ScheduledPickupUtc, ScheduledDeliveryUtc,
         PlannedDistanceMiles, Notes, SourceImportBatchId)
    VALUES
        (@TripNumber, @LoadId, 1, @ScheduledPickupUtc, @ScheduledDeliveryUtc,
         @PlannedDistanceMiles, @Notes, @SourceImportBatchId);
    DECLARE @TripId bigint = SCOPE_IDENTITY();
    INSERT dbo.TripStops
        (TripId, StopSequence, StopTypeId, StopStatusId, LocationId,
         ScheduledArrivalUtc, ScheduledDepartureUtc, Instructions, SourceImportBatchId)
    SELECT @TripId, s.StopSequence, st.StopTypeId, 1, loc.LocationId,
           s.ScheduledArrivalUtc, s.ScheduledDepartureUtc, s.Instructions, @SourceImportBatchId
    FROM @Stops AS s
    JOIN dbo.StopTypes AS st ON st.Code = s.StopTypeCode
    JOIN dbo.Locations AS loc ON loc.LocationCode = s.LocationCode;
    INSERT dbo.TripStatusHistory
        (TripId, PreviousTripStatusId, NewTripStatusId, ChangedBy, Source, Notes)
    VALUES (@TripId, NULL, 1, @ChangedBy, 'DISPATCH', N'Trip created.');
    UPDATE dbo.Loads SET LoadStatusId = 2, UpdatedAtUtc = SYSUTCDATETIME() WHERE LoadId = @LoadId;
    COMMIT TRANSACTION;
    SELECT TripId, RowVersion FROM dbo.Trips WHERE TripId = @TripId;
END;
GO

CREATE OR ALTER PROCEDURE operations.Trip_TransitionStatus
    @TripId bigint, @ToStatusCode varchar(30), @ChangedBy nvarchar(120),
    @Source varchar(30), @Notes nvarchar(500) = NULL,
    @ExpectedRowVersion binary(8)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRANSACTION;
    DECLARE @FromStatusId tinyint, @ToStatusId tinyint;
    SELECT @FromStatusId = TripStatusId FROM dbo.Trips WITH (UPDLOCK, HOLDLOCK)
    WHERE TripId = @TripId AND RowVersion = @ExpectedRowVersion;
    IF @FromStatusId IS NULL THROW 51012, 'Trip was changed by another user or does not exist.', 1;
    SELECT @ToStatusId = TripStatusId FROM dbo.TripStatuses WHERE Code = @ToStatusCode;
    IF @ToStatusId IS NULL THROW 51013, 'Unknown trip status.', 1;
    IF NOT EXISTS
    (
        SELECT 1 FROM dbo.TripStatusTransitions
        WHERE FromTripStatusId = @FromStatusId AND ToTripStatusId = @ToStatusId
          AND ((@Source = 'DRIVER_APP' AND IsDriverAllowed = 1)
            OR (@Source = 'DISPATCH' AND IsDispatchAllowed = 1)
            OR (@Source IN ('SYSTEM', 'SIMULATOR') AND IsSystemAllowed = 1))
    ) THROW 51014, 'Trip status transition is not allowed for this source.', 1;
    UPDATE dbo.Trips
    SET TripStatusId = @ToStatusId,
        ActualStartUtc = CASE WHEN @ToStatusCode = 'EN_ROUTE_TO_PICKUP' AND ActualStartUtc IS NULL
                              THEN SYSUTCDATETIME() ELSE ActualStartUtc END,
        ActualDeliveryUtc = CASE WHEN @ToStatusCode IN ('DELIVERED', 'COMPLETED') AND ActualDeliveryUtc IS NULL
                                 THEN SYSUTCDATETIME() ELSE ActualDeliveryUtc END,
        UpdatedAtUtc = SYSUTCDATETIME()
    WHERE TripId = @TripId;
    INSERT dbo.TripStatusHistory
        (TripId, PreviousTripStatusId, NewTripStatusId, ChangedBy, Source, Notes)
    VALUES (@TripId, @FromStatusId, @ToStatusId, @ChangedBy, @Source, @Notes);
    IF @ToStatusCode IN ('DELIVERED', 'COMPLETED')
        UPDATE l SET LoadStatusId = 4, UpdatedAtUtc = SYSUTCDATETIME()
        FROM dbo.Loads AS l JOIN dbo.Trips AS t ON t.LoadId = l.LoadId
        WHERE t.TripId = @TripId;
    IF @ToStatusCode = 'COMPLETED'
    BEGIN
        DECLARE @CompletedDriverId bigint, @CompletedVehicleId bigint, @CompletedTrailerId bigint;
        SELECT @CompletedDriverId = DriverId, @CompletedVehicleId = VehicleId,
               @CompletedTrailerId = TrailerId
        FROM dbo.TripAssignments
        WHERE TripId = @TripId AND IsActive = 1;
        UPDATE dbo.TripAssignments
        SET AssignmentStatusId = 6, IsActive = 0, CompletedAtUtc = SYSUTCDATETIME(),
            UpdatedAtUtc = SYSUTCDATETIME()
        WHERE TripId = @TripId AND IsActive = 1;
        UPDATE dbo.Drivers SET DriverStatusId = 1, UpdatedAtUtc = SYSUTCDATETIME()
        WHERE DriverId = @CompletedDriverId;
        UPDATE dbo.Vehicles SET FleetAssetStatusId = 1, UpdatedAtUtc = SYSUTCDATETIME()
        WHERE VehicleId = @CompletedVehicleId;
        UPDATE dbo.Trailers SET FleetAssetStatusId = 1, UpdatedAtUtc = SYSUTCDATETIME()
        WHERE TrailerId = @CompletedTrailerId;
    END;
    COMMIT TRANSACTION;
    SELECT TripId, TripStatusId, RowVersion FROM dbo.Trips WHERE TripId = @TripId;
END;
GO

CREATE OR ALTER PROCEDURE dispatch.Assignment_Offer
    @TripId bigint, @DriverId bigint, @VehicleId bigint, @TrailerId bigint = NULL,
    @AssignedBy nvarchar(120)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRANSACTION;
    DECLARE @CurrentTripStatusId tinyint;
    SELECT @CurrentTripStatusId = TripStatusId
    FROM dbo.Trips WITH (UPDLOCK, HOLDLOCK) WHERE TripId = @TripId;
    IF @CurrentTripStatusId IS NULL THROW 51027, 'Trip does not exist.', 1;
    IF @CurrentTripStatusId <> 1 THROW 51028, 'Only a planned trip can be offered.', 1;
    IF NOT EXISTS (SELECT 1 FROM dispatch.vw_AvailableDrivers WHERE DriverId = @DriverId)
        THROW 51015, 'Driver is not available.', 1;
    IF NOT EXISTS (SELECT 1 FROM dispatch.vw_AvailableVehicles WHERE VehicleId = @VehicleId)
        THROW 51016, 'Vehicle is not available.', 1;
    IF @TrailerId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dispatch.vw_AvailableTrailers WHERE TrailerId = @TrailerId)
        THROW 51017, 'Trailer is not available.', 1;
    IF EXISTS (SELECT 1 FROM dbo.TripAssignments WHERE TripId = @TripId AND IsActive = 1)
        THROW 51018, 'Trip already has an active assignment.', 1;
    INSERT dbo.TripAssignments
        (TripId, DriverId, VehicleId, TrailerId, AssignmentStatusId, IsActive, OfferedAtUtc, AssignedBy)
    VALUES (@TripId, @DriverId, @VehicleId, @TrailerId, 2, 1, SYSUTCDATETIME(), @AssignedBy);
    DECLARE @AssignmentId bigint = SCOPE_IDENTITY();
    UPDATE dbo.Drivers SET DriverStatusId = 2, UpdatedAtUtc = SYSUTCDATETIME() WHERE DriverId = @DriverId;
    UPDATE dbo.Vehicles SET FleetAssetStatusId = 2, UpdatedAtUtc = SYSUTCDATETIME() WHERE VehicleId = @VehicleId;
    IF @TrailerId IS NOT NULL UPDATE dbo.Trailers SET FleetAssetStatusId = 2, UpdatedAtUtc = SYSUTCDATETIME() WHERE TrailerId = @TrailerId;
    UPDATE dbo.Trips SET TripStatusId = 2, UpdatedAtUtc = SYSUTCDATETIME() WHERE TripId = @TripId;
    INSERT dbo.TripStatusHistory
        (TripId, PreviousTripStatusId, NewTripStatusId, ChangedBy, Source, Notes)
    VALUES (@TripId, 1, 2, @AssignedBy, 'DISPATCH', N'Trip offered to driver.');
    COMMIT TRANSACTION;
    SELECT TripAssignmentId, RowVersion FROM dbo.TripAssignments WHERE TripAssignmentId = @AssignmentId;
END;
GO

CREATE OR ALTER PROCEDURE dispatch.Assignment_Respond
    @TripAssignmentId bigint, @Accept bit, @DriverResponseNotes nvarchar(500) = NULL,
    @ExpectedRowVersion binary(8)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRANSACTION;
    DECLARE @TripId bigint, @DriverId bigint, @VehicleId bigint, @TrailerId bigint;
    SELECT @TripId = TripId, @DriverId = DriverId, @VehicleId = VehicleId, @TrailerId = TrailerId
    FROM dbo.TripAssignments WITH (UPDLOCK, HOLDLOCK)
    WHERE TripAssignmentId = @TripAssignmentId AND IsActive = 1
      AND AssignmentStatusId = 2 AND RowVersion = @ExpectedRowVersion;
    IF @DriverId IS NULL THROW 51019, 'Assignment is no longer available for response.', 1;
    UPDATE dbo.TripAssignments
    SET AssignmentStatusId = CASE WHEN @Accept = 1 THEN 3 ELSE 4 END,
        RespondedAtUtc = SYSUTCDATETIME(),
        AcceptedAtUtc = CASE WHEN @Accept = 1 THEN SYSUTCDATETIME() ELSE NULL END,
        RejectedAtUtc = CASE WHEN @Accept = 0 THEN SYSUTCDATETIME() ELSE NULL END,
        DriverResponseNotes = @DriverResponseNotes,
        IsActive = CASE WHEN @Accept = 1 THEN 1 ELSE 0 END,
        UpdatedAtUtc = SYSUTCDATETIME()
    WHERE TripAssignmentId = @TripAssignmentId;
    IF @Accept = 1
        UPDATE dbo.Drivers SET DriverStatusId = 3, UpdatedAtUtc = SYSUTCDATETIME() WHERE DriverId = @DriverId;
    ELSE
    BEGIN
        UPDATE dbo.Drivers SET DriverStatusId = 1, UpdatedAtUtc = SYSUTCDATETIME() WHERE DriverId = @DriverId;
        UPDATE dbo.Vehicles SET FleetAssetStatusId = 1, UpdatedAtUtc = SYSUTCDATETIME() WHERE VehicleId = @VehicleId;
        IF @TrailerId IS NOT NULL UPDATE dbo.Trailers SET FleetAssetStatusId = 1, UpdatedAtUtc = SYSUTCDATETIME() WHERE TrailerId = @TrailerId;
    END;
    UPDATE dbo.Trips
    SET TripStatusId = CASE WHEN @Accept = 1 THEN 3 ELSE 1 END,
        UpdatedAtUtc = SYSUTCDATETIME()
    WHERE TripId = @TripId AND TripStatusId = 2;
    INSERT dbo.TripStatusHistory
        (TripId, PreviousTripStatusId, NewTripStatusId, ChangedBy, Source, Notes)
    VALUES
        (@TripId, 2, CASE WHEN @Accept = 1 THEN 3 ELSE 1 END,
         N'Driver', 'DRIVER_APP',
         CASE WHEN @Accept = 1 THEN N'Driver accepted assignment.' ELSE N'Driver rejected assignment.' END);
    COMMIT TRANSACTION;
    SELECT TripAssignmentId, AssignmentStatusId, IsActive, RowVersion
    FROM dbo.TripAssignments WHERE TripAssignmentId = @TripAssignmentId;
END;
GO

CREATE OR ALTER PROCEDURE tracking.TripRoute_Replace
    @TripId bigint, @DataOriginId tinyint, @SourceImportBatchId bigint = NULL,
    @Points tracking.RoutePointTableType READONLY
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    IF NOT EXISTS (SELECT 1 FROM @Points) THROW 51020, 'At least one route point is required.', 1;
    IF EXISTS (SELECT 1 FROM @Points WHERE Latitude NOT BETWEEN -90 AND 90 OR Longitude NOT BETWEEN -180 AND 180)
        THROW 51021, 'Route contains an invalid coordinate.', 1;
    BEGIN TRANSACTION;
    DELETE dbo.TripRoutePoints WHERE TripId = @TripId;
    INSERT dbo.TripRoutePoints
        (TripId, PointSequence, Latitude, Longitude, CumulativeDistanceMiles,
         ExpectedOffsetSeconds, Instruction, DataOriginId, SourceImportBatchId)
    SELECT @TripId, PointSequence, Latitude, Longitude, CumulativeDistanceMiles,
           ExpectedOffsetSeconds, Instruction, @DataOriginId, @SourceImportBatchId
    FROM @Points;
    COMMIT TRANSACTION;
END;
GO

CREATE OR ALTER PROCEDURE tracking.TripEvent_Append
    @EventId uniqueidentifier, @TripId bigint, @VehicleId bigint, @TripStopId bigint = NULL,
    @EventType varchar(40), @OccurredAtUtc datetime2(3), @CorrelationId uniqueidentifier = NULL,
    @DataOriginId tinyint, @ImportBatchId bigint = NULL, @SimulationRunId bigint = NULL,
    @Message nvarchar(500) = NULL, @PayloadJson nvarchar(max) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM dbo.TripEvents WHERE EventId = @EventId)
    BEGIN
        SELECT TripEventId, EventId FROM dbo.TripEvents WHERE EventId = @EventId;
        RETURN;
    END;
    INSERT dbo.TripEvents
        (EventId, TripId, VehicleId, TripStopId, EventType, OccurredAtUtc, CorrelationId,
         DataOriginId, ImportBatchId, SimulationRunId, Message, PayloadJson)
    VALUES
        (@EventId, @TripId, @VehicleId, @TripStopId, @EventType, @OccurredAtUtc, @CorrelationId,
         @DataOriginId, @ImportBatchId, @SimulationRunId, @Message, @PayloadJson);
    SELECT TripEventId, EventId FROM dbo.TripEvents WHERE TripEventId = SCOPE_IDENTITY();
END;
GO

CREATE OR ALTER PROCEDURE tracking.VehicleTelemetry_AppendBatch
    @Telemetry tracking.VehicleTelemetryTableType READONLY
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    IF EXISTS (SELECT 1 FROM @Telemetry WHERE Latitude NOT BETWEEN -90 AND 90 OR Longitude NOT BETWEEN -180 AND 180)
        THROW 51022, 'Telemetry contains an invalid coordinate.', 1;
    BEGIN TRANSACTION;
    INSERT dbo.VehicleTelemetry
        (TelemetryId, VehicleId, TripId, RecordedAtUtc, SequenceNumber, Latitude, Longitude,
         SpeedMph, FuelPercent, OdometerMiles, HeadingDegrees, DataOriginId,
         ImportBatchId, SimulationRunId, PayloadJson)
    SELECT src.TelemetryId, src.VehicleId, src.TripId, src.RecordedAtUtc, src.SequenceNumber,
           src.Latitude, src.Longitude, src.SpeedMph, src.FuelPercent, src.OdometerMiles,
           src.HeadingDegrees, src.DataOriginId, src.ImportBatchId, src.SimulationRunId, src.PayloadJson
    FROM @Telemetry AS src
    WHERE NOT EXISTS (SELECT 1 FROM dbo.VehicleTelemetry AS existing WHERE existing.TelemetryId = src.TelemetryId);
    DECLARE @InsertedRows int = @@ROWCOUNT;

    DECLARE @Latest TABLE
    (
        VehicleId bigint PRIMARY KEY,
        TripId bigint NULL,
        VehicleTelemetryId bigint NOT NULL,
        SimulationRunId bigint NULL,
        RecordedAtUtc datetime2(3) NOT NULL,
        Latitude decimal(9,6) NOT NULL,
        Longitude decimal(9,6) NOT NULL,
        SpeedMph decimal(6,2) NULL,
        FuelPercent decimal(5,2) NULL,
        OdometerMiles decimal(12,1) NULL,
        HeadingDegrees decimal(6,2) NULL
    );
    ;WITH ranked AS
    (
        SELECT tm.*, ROW_NUMBER() OVER
            (PARTITION BY tm.VehicleId ORDER BY tm.RecordedAtUtc DESC, tm.VehicleTelemetryId DESC) AS rn
        FROM dbo.VehicleTelemetry AS tm
        JOIN @Telemetry AS src ON src.TelemetryId = tm.TelemetryId
    )
    INSERT @Latest
        (VehicleId, TripId, VehicleTelemetryId, SimulationRunId, RecordedAtUtc,
         Latitude, Longitude, SpeedMph, FuelPercent, OdometerMiles, HeadingDegrees)
    SELECT VehicleId, TripId, VehicleTelemetryId, SimulationRunId, RecordedAtUtc,
           Latitude, Longitude, SpeedMph, FuelPercent, OdometerMiles, HeadingDegrees
    FROM ranked WHERE rn = 1;

    UPDATE target
    SET TripId = source.TripId, LastVehicleTelemetryId = source.VehicleTelemetryId,
        SimulationRunId = source.SimulationRunId, RecordedAtUtc = source.RecordedAtUtc,
        Latitude = source.Latitude, Longitude = source.Longitude,
        SpeedMph = source.SpeedMph, FuelPercent = source.FuelPercent,
        OdometerMiles = source.OdometerMiles, HeadingDegrees = source.HeadingDegrees,
        UpdatedAtUtc = SYSUTCDATETIME()
    FROM dbo.VehicleCurrentState AS target WITH (UPDLOCK, SERIALIZABLE)
    JOIN @Latest AS source ON source.VehicleId = target.VehicleId
    WHERE source.RecordedAtUtc >= target.RecordedAtUtc;

    INSERT dbo.VehicleCurrentState
        (VehicleId, TripId, LastVehicleTelemetryId, SimulationRunId, RecordedAtUtc,
         Latitude, Longitude, SpeedMph, FuelPercent, OdometerMiles, HeadingDegrees)
    SELECT source.VehicleId, source.TripId, source.VehicleTelemetryId, source.SimulationRunId,
           source.RecordedAtUtc, source.Latitude, source.Longitude, source.SpeedMph,
           source.FuelPercent, source.OdometerMiles, source.HeadingDegrees
    FROM @Latest AS source
    WHERE NOT EXISTS
    (
        SELECT 1 FROM dbo.VehicleCurrentState WITH (UPDLOCK, SERIALIZABLE)
        WHERE VehicleId = source.VehicleId
    );
    COMMIT TRANSACTION;
    SELECT @InsertedRows AS InsertedRows, COUNT_BIG(*) AS SubmittedRows FROM @Telemetry;
END;
GO

CREATE OR ALTER PROCEDURE tracking.TripEvent_AppendBatch
    @Events tracking.TripEventTableType READONLY
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    INSERT dbo.TripEvents
        (EventId, TripId, VehicleId, TripStopId, EventType, OccurredAtUtc, CorrelationId,
         DataOriginId, ImportBatchId, SimulationRunId, Message, PayloadJson)
    SELECT source.EventId, source.TripId, source.VehicleId, source.TripStopId,
           source.EventType, source.OccurredAtUtc, source.CorrelationId,
           source.DataOriginId, source.ImportBatchId, source.SimulationRunId,
           source.Message, source.PayloadJson
    FROM @Events AS source
    WHERE NOT EXISTS
    (
        SELECT 1 FROM dbo.TripEvents AS existing WHERE existing.EventId = source.EventId
    );
    DECLARE @InsertedEventRows int = @@ROWCOUNT;
    SELECT @InsertedEventRows AS InsertedRows, COUNT_BIG(*) AS SubmittedRows FROM @Events;
END;
GO

CREATE OR ALTER PROCEDURE import.Batch_Begin
    @EntityType varchar(40), @FileName nvarchar(260), @FileSha256 char(64),
    @TotalRows int, @CreatedByAppUserId bigint = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS
    (
        SELECT 1 FROM dbo.ImportBatches
        WHERE EntityType = @EntityType AND FileSha256 = @FileSha256
          AND Status IN ('READY', 'IMPORTING', 'COMPLETED', 'COMPLETED_WITH_ERRORS')
    ) THROW 51023, 'This file was already imported or is being processed.', 1;
    INSERT dbo.ImportBatches
        (DataOriginId, EntityType, FileName, FileSha256, Status, TotalRows, CreatedByAppUserId)
    VALUES (2, @EntityType, @FileName, @FileSha256, 'VALIDATING', @TotalRows, @CreatedByAppUserId);
    SELECT ImportBatchId, ImportBatchUid FROM dbo.ImportBatches WHERE ImportBatchId = SCOPE_IDENTITY();
END;
GO

CREATE OR ALTER PROCEDURE import.Batch_AddErrors
    @ImportBatchId bigint,
    @Errors import.ImportErrorTableType READONLY
AS
BEGIN
    SET NOCOUNT ON;
    INSERT dbo.ImportBatchErrors
        (ImportBatchId, RowNumber, ColumnName, RawValue, ErrorCode, ErrorMessage, RawRowJson)
    SELECT @ImportBatchId, RowNumber, ColumnName, RawValue, ErrorCode, ErrorMessage, RawRowJson
    FROM @Errors;
END;
GO

CREATE OR ALTER PROCEDURE import.Batch_Complete
    @ImportBatchId bigint, @ValidRows int, @InvalidRows int, @ImportedRows int,
    @Notes nvarchar(1000) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.ImportBatches
    SET Status = CASE WHEN @InvalidRows > 0 THEN 'COMPLETED_WITH_ERRORS' ELSE 'COMPLETED' END,
        CompletedAtUtc = SYSUTCDATETIME(), ValidRows = @ValidRows,
        InvalidRows = @InvalidRows, ImportedRows = @ImportedRows, Notes = @Notes
    WHERE ImportBatchId = @ImportBatchId AND Status IN ('VALIDATING', 'READY', 'IMPORTING');
    IF @@ROWCOUNT = 0 THROW 51024, 'Import batch cannot be completed from its current state.', 1;
END;
GO

CREATE OR ALTER PROCEDURE simulation.Run_Create
    @Name nvarchar(120), @ScenarioCode varchar(40), @RandomSeed int,
    @TimeScale decimal(8,2), @UpdateIntervalMilliseconds int,
    @PlannedVehicleCount int, @ConfigurationJson nvarchar(max) = NULL,
    @CreatedByAppUserId bigint = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT dbo.SimulationRuns
        (Name, ScenarioCode, Status, RandomSeed, TimeScale, UpdateIntervalMilliseconds,
         PlannedVehicleCount, ConfigurationJson, CreatedByAppUserId)
    VALUES
        (@Name, @ScenarioCode, 'READY', @RandomSeed, @TimeScale, @UpdateIntervalMilliseconds,
         @PlannedVehicleCount, @ConfigurationJson, @CreatedByAppUserId);
    SELECT SimulationRunId, SimulationRunUid FROM dbo.SimulationRuns WHERE SimulationRunId = SCOPE_IDENTITY();
END;
GO

CREATE OR ALTER PROCEDURE simulation.Run_SetStatus
    @SimulationRunId bigint, @Status varchar(20)
AS
BEGIN
    SET NOCOUNT ON;
    IF @Status NOT IN ('RUNNING', 'PAUSED', 'COMPLETED', 'FAILED', 'CANCELLED')
        THROW 51025, 'Unsupported simulation status.', 1;
    UPDATE dbo.SimulationRuns
    SET Status = @Status,
        StartedAtUtc = CASE WHEN @Status = 'RUNNING' AND StartedAtUtc IS NULL THEN SYSUTCDATETIME() ELSE StartedAtUtc END,
        EndedAtUtc = CASE WHEN @Status IN ('COMPLETED', 'FAILED', 'CANCELLED') THEN SYSUTCDATETIME() ELSE EndedAtUtc END
    WHERE SimulationRunId = @SimulationRunId
      AND Status NOT IN ('COMPLETED', 'FAILED', 'CANCELLED');
    IF @@ROWCOUNT = 0 THROW 51026, 'Simulation run does not exist or is already terminal.', 1;
    SELECT * FROM dbo.SimulationRuns WHERE SimulationRunId = @SimulationRunId;
END;
GO
