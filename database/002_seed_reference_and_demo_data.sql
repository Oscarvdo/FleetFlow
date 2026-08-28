USE FleetFlowDb;
GO

SET XACT_ABORT ON;
BEGIN TRANSACTION;

INSERT dbo.FleetAssetStatuses (FleetAssetStatusId, Code, DisplayName, IsAvailable, SortOrder)
VALUES
    (1, 'AVAILABLE', 'Available', 1, 10),
    (2, 'ASSIGNED', 'Assigned', 0, 20),
    (3, 'IN_TRANSIT', 'In Transit', 0, 30),
    (4, 'MAINTENANCE', 'Maintenance', 0, 40),
    (5, 'OUT_OF_SERVICE', 'Out of Service', 0, 50);

INSERT dbo.DriverStatuses (DriverStatusId, Code, DisplayName, IsAvailable, SortOrder)
VALUES
    (1, 'AVAILABLE', 'Available', 1, 10),
    (2, 'OFFERED', 'Trip Offered', 0, 20),
    (3, 'ASSIGNED', 'Assigned', 0, 30),
    (4, 'ON_TRIP', 'On Trip', 0, 40),
    (5, 'OFF_DUTY', 'Off Duty', 0, 50);

INSERT dbo.LoadStatuses (LoadStatusId, Code, DisplayName, SortOrder)
VALUES
    (1, 'NEW', 'New', 10),
    (2, 'PLANNED', 'Planned', 20),
    (3, 'IN_TRANSIT', 'In Transit', 30),
    (4, 'DELIVERED', 'Delivered', 40),
    (5, 'CANCELLED', 'Cancelled', 50);

INSERT dbo.TripStatuses (TripStatusId, Code, DisplayName, IsTerminal, SortOrder)
VALUES
    (1, 'PLANNED', 'Planned', 0, 10),
    (2, 'OFFERED', 'Offered to Driver', 0, 20),
    (3, 'ASSIGNED', 'Assigned', 0, 30),
    (4, 'EN_ROUTE_TO_PICKUP', 'En Route to Pickup', 0, 40),
    (5, 'AT_PICKUP', 'At Pickup', 0, 50),
    (6, 'LOADED', 'Loaded', 0, 60),
    (7, 'EN_ROUTE_TO_DELIVERY', 'En Route to Delivery', 0, 70),
    (8, 'AT_DELIVERY', 'At Delivery', 0, 80),
    (9, 'DELIVERED', 'Delivered', 0, 90),
    (10, 'COMPLETED', 'Completed', 1, 100),
    (11, 'DELAYED', 'Delayed', 0, 110),
    (12, 'INCIDENT_REPORTED', 'Incident Reported', 0, 120),
    (13, 'VEHICLE_BREAKDOWN', 'Vehicle Breakdown', 0, 130),
    (14, 'CANCELLED', 'Cancelled', 1, 140);

INSERT dbo.AssignmentStatuses (AssignmentStatusId, Code, DisplayName, IsTerminal, SortOrder)
VALUES
    (1, 'DRAFT', 'Draft', 0, 10),
    (2, 'OFFERED', 'Offered', 0, 20),
    (3, 'ACCEPTED', 'Accepted', 0, 30),
    (4, 'REJECTED', 'Rejected', 1, 40),
    (5, 'ACTIVE', 'Active', 0, 50),
    (6, 'COMPLETED', 'Completed', 1, 60),
    (7, 'CANCELLED', 'Cancelled', 1, 70);

INSERT dbo.StopTypes (StopTypeId, Code, DisplayName, SortOrder)
VALUES
    (1, 'PICKUP', 'Pickup', 10),
    (2, 'DELIVERY', 'Delivery', 20),
    (3, 'WAYPOINT', 'Waypoint', 30),
    (4, 'FUEL', 'Fuel', 40),
    (5, 'REST', 'Rest', 50);

INSERT dbo.StopStatuses (StopStatusId, Code, DisplayName, SortOrder)
VALUES
    (1, 'PLANNED', 'Planned', 10),
    (2, 'EN_ROUTE', 'En Route', 20),
    (3, 'ARRIVED', 'Arrived', 30),
    (4, 'COMPLETED', 'Completed', 40),
    (5, 'SKIPPED', 'Skipped', 50);

INSERT dbo.DataOrigins (DataOriginId, Code, DisplayName, IsSynthetic, SortOrder)
VALUES
    (1, 'MANUAL', 'Manual Entry', 0, 10),
    (2, 'CSV_IMPORT', 'CSV Import', 0, 20),
    (3, 'SIMULATOR', 'Simulator', 1, 30),
    (4, 'DRIVER_APP', 'Driver Mobile App', 0, 40),
    (5, 'DEVICE', 'Physical Device', 0, 50),
    (6, 'SYSTEM', 'System Generated', 0, 60);

INSERT dbo.Customers
    (CustomerNumber, CompanyName, ContactName, Email, Phone)
VALUES
    ('CUS-1001', N'Sonoran Foods Distribution', N'Elena Ruiz', 'elena.ruiz@example.test', '520-555-0101'),
    ('CUS-1002', N'Desert Medical Supply', N'Daniel Harper', 'daniel.harper@example.test', '602-555-0102');

DECLARE @SonoranCustomerId bigint =
    (SELECT CustomerId FROM dbo.Customers WHERE CustomerNumber = 'CUS-1001');
DECLARE @DesertMedicalCustomerId bigint =
    (SELECT CustomerId FROM dbo.Customers WHERE CustomerNumber = 'CUS-1002');

INSERT dbo.Locations
    (CustomerId, LocationCode, LocationType, LocationName, Address1, City, StateCode, PostalCode, Latitude, Longitude, ContactName, ContactPhone, IsBillingLocation)
VALUES
    (@SonoranCustomerId, 'LOC-TUS-SON-01', 'CUSTOMER', N'Sonoran Foods Warehouse', N'1900 S Country Club Rd', N'Tucson', 'AZ', '85713', 32.199500, -110.926000, N'Elena Ruiz', '520-555-0101', 1),
    (@DesertMedicalCustomerId, 'LOC-PHX-DMS-01', 'CUSTOMER', N'Phoenix Distribution Center', N'4100 E Washington St', N'Phoenix', 'AZ', '85034', 33.449800, -111.990000, N'Daniel Harper', '602-555-0102', 1);

INSERT dbo.Drivers
    (DriverNumber, FirstName, LastName, Phone, Email, LicenseNumber, LicenseState, LicenseExpirationDate)
VALUES
    ('DRV-101', N'Miguel', N'Santos', '520-555-0201', 'miguel.santos@example.test', 'AZCDL100001', 'AZ', '2028-04-30'),
    ('DRV-102', N'Laura', N'Benitez', '520-555-0202', 'laura.benitez@example.test', 'AZCDL100002', 'AZ', '2027-11-30');

INSERT dbo.Vehicles
    (UnitNumber, Vin, ModelYear, Make, Model, LicensePlate, LicenseState, MaxPayloadLbs, CurrentOdometerMiles)
VALUES
    ('TRK-101', '1FUJHHDR1LLAA0001', 2022, N'Freightliner', N'Cascadia', 'AZF101', 'AZ', 45000, 185240.5),
    ('TRK-102', '1XKYDP9X1NJAA0002', 2023, N'Kenworth', N'T680', 'AZK102', 'AZ', 45000, 122810.0);

INSERT dbo.Trailers
    (UnitNumber, Vin, TrailerType, LicensePlate, LicenseState, MaxPayloadLbs)
VALUES
    ('TRL-201', '1UYVS2536NPA00001', 'DRY_VAN', 'AZT201', 'AZ', 44000),
    ('TRL-202', '1JJV532D7NL000002', 'REEFER', 'AZT202', 'AZ', 43000);

DECLARE @CustomerId bigint =
    @SonoranCustomerId;

INSERT dbo.Loads
    (LoadNumber, CustomerId, Description, Commodity, WeightLbs, Pieces, RevenueAmount, LoadStatusId, SpecialInstructions)
VALUES
    ('LD-2026-0001', @CustomerId, N'Packaged food shipment', N'Dry packaged food', 28000, 24, 1850.00, 2, N'Deliver during receiving hours.');

DECLARE @LoadId bigint =
    (SELECT LoadId FROM dbo.Loads WHERE LoadNumber = 'LD-2026-0001');

INSERT dbo.Trips
    (TripNumber, LoadId, TripStatusId, ScheduledPickupUtc, ScheduledDeliveryUtc, PlannedDistanceMiles, Notes)
VALUES
    ('TRIP-2026-0001', @LoadId, 2, '2026-09-01T15:00:00', '2026-09-01T19:00:00', 118.40, N'Tucson to Phoenix demonstration trip.');

DECLARE @TripId bigint =
    (SELECT TripId FROM dbo.Trips WHERE TripNumber = 'TRIP-2026-0001');
DECLARE @PickupLocationId bigint =
    (SELECT LocationId FROM dbo.Locations WHERE LocationCode = 'LOC-TUS-SON-01');
DECLARE @DeliveryLocationId bigint =
    (SELECT LocationId FROM dbo.Locations WHERE LocationCode = 'LOC-PHX-DMS-01');

INSERT dbo.TripStops
    (TripId, StopSequence, StopTypeId, StopStatusId, LocationId, ScheduledArrivalUtc, ScheduledDepartureUtc, Instructions)
VALUES
    (@TripId, 1, 1, 1, @PickupLocationId, '2026-09-01T15:00:00', '2026-09-01T15:30:00', N'Check in at receiving office.'),
    (@TripId, 2, 2, 1, @DeliveryLocationId, '2026-09-01T19:00:00', '2026-09-01T19:30:00', N'Use dock 12.');

INSERT dbo.TripRoutePoints
    (TripId, PointSequence, Latitude, Longitude, CumulativeDistanceMiles, ExpectedOffsetSeconds, Instruction, DataOriginId)
VALUES
    (@TripId, 1, 32.199500, -110.926000, 0.000, 0, N'Depart Sonoran Foods Warehouse.', 1),
    (@TripId, 2, 32.280100, -110.971200, 8.200, 600, N'Continue north through Tucson.', 1),
    (@TripId, 3, 32.510300, -111.120400, 28.500, 1800, N'Continue toward Marana.', 1),
    (@TripId, 4, 32.880500, -111.350700, 61.300, 4200, N'Continue north toward Casa Grande.', 1),
    (@TripId, 5, 33.449800, -111.990000, 118.400, 9000, N'Arrive at Phoenix Distribution Center.', 1);

INSERT dbo.SimulationRuns
    (Name, ScenarioCode, Status, RandomSeed, TimeScale, UpdateIntervalMilliseconds, PlannedVehicleCount, ConfigurationJson)
VALUES
    (N'Tucson to Phoenix Demonstration', 'NORMAL_OPERATION', 'READY', 20260828, 10.00, 2000, 1,
     N'{"failureProbability":0.0,"connectionLossProbability":0.0,"description":"Reproducible initial FleetFlow route demonstration"}');

DECLARE @DriverId bigint =
    (SELECT DriverId FROM dbo.Drivers WHERE DriverNumber = 'DRV-101');
DECLARE @VehicleId bigint =
    (SELECT VehicleId FROM dbo.Vehicles WHERE UnitNumber = 'TRK-101');
DECLARE @TrailerId bigint =
    (SELECT TrailerId FROM dbo.Trailers WHERE UnitNumber = 'TRL-201');

INSERT dbo.TripAssignments
    (TripId, DriverId, VehicleId, TrailerId, AssignmentStatusId, IsActive, OfferedAtUtc, AssignedBy)
VALUES
    (@TripId, @DriverId, @VehicleId, @TrailerId, 2, 1, SYSUTCDATETIME(), N'Demo Dispatcher');

UPDATE dbo.Drivers
SET DriverStatusId = 2,
    UpdatedAtUtc = SYSUTCDATETIME()
WHERE DriverId = @DriverId;

UPDATE dbo.Vehicles
SET FleetAssetStatusId = 2,
    UpdatedAtUtc = SYSUTCDATETIME()
WHERE VehicleId = @VehicleId;

UPDATE dbo.Trailers
SET FleetAssetStatusId = 2,
    UpdatedAtUtc = SYSUTCDATETIME()
WHERE TrailerId = @TrailerId;

INSERT dbo.TripStatusHistory
    (TripId, PreviousTripStatusId, NewTripStatusId, ChangedBy, Source, Notes)
VALUES
    (@TripId, 1, 2, N'Demo Dispatcher', 'DISPATCH', N'Trip offered to driver DRV-101.');

COMMIT TRANSACTION;
GO
