/*
    FleetFlow extended demonstration dataset.

    Purpose:
    - Exercise every Trips and Loads status filter.
    - Populate Dashboard and Dispatch Board with varied records.
    - Provide trips with different stop progress and assignments.
    - Remain safe to run repeatedly without duplicating demo records.
*/
USE FleetFlowDb;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    /* ---------------------------------------------------------
       Customers
       --------------------------------------------------------- */
    ;WITH CustomerSeed AS
    (
        SELECT *
        FROM (VALUES
            ('CUS-2001', N'Copper State Retail Distribution', N'Ana Torres',     'ana.torres@example.test',     '520-555-1101'),
            ('CUS-2002', N'Arizona Clinical Supply',           N'Marcus Lee',     'marcus.lee@example.test',     '602-555-1102'),
            ('CUS-2003', N'Grand Canyon Office Products',      N'Sofia Martinez', 'sofia.martinez@example.test', '928-555-1103'),
            ('CUS-2004', N'Sonoran Fresh Markets',             N'Luis Herrera',   'luis.herrera@example.test',   '520-555-1104'),
            ('CUS-2005', N'Mesa Industrial Components',        N'Rachel Morgan',  'rachel.morgan@example.test',  '480-555-1105'),
            ('CUS-2006', N'Borderland Building Supply',        N'Diego Chavez',   'diego.chavez@example.test',   '520-555-1106'),
            ('CUS-2007', N'Northern Arizona Hospitality',      N'Emily Foster',   'emily.foster@example.test',   '928-555-1107'),
            ('CUS-2008', N'Valley Technology Logistics',       N'Omar Salazar',   'omar.salazar@example.test',   '623-555-1108')
        ) AS value(CustomerNumber, CompanyName, ContactName, Email, Phone)
    )
    INSERT dbo.Customers
        (CustomerNumber, CompanyName, ContactName, Email, Phone)
    SELECT
        seed.CustomerNumber,
        seed.CompanyName,
        seed.ContactName,
        seed.Email,
        seed.Phone
    FROM CustomerSeed AS seed
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.Customers AS existing
        WHERE existing.CustomerNumber = seed.CustomerNumber
    );

    /* ---------------------------------------------------------
       Customer locations
       --------------------------------------------------------- */
    ;WITH LocationSeed AS
    (
        SELECT *
        FROM (VALUES
            ('LOC-TUS-CSR-01', 'CUS-2001', N'Copper State Tucson Warehouse',     N'4450 S Country Club Rd', N'Tucson',      'AZ', '85714', CAST(32.170900 AS decimal(9,6)), CAST(-110.927500 AS decimal(9,6))),
            ('LOC-PHX-ACS-01', 'CUS-2002', N'Arizona Clinical Phoenix Depot',    N'3400 E Sky Harbor Blvd', N'Phoenix',     'AZ', '85034', CAST(33.435300 AS decimal(9,6)), CAST(-112.006900 AS decimal(9,6))),
            ('LOC-FLG-GCO-01', 'CUS-2003', N'Grand Canyon Flagstaff Center',     N'2100 E Butler Ave',      N'Flagstaff',   'AZ', '86004', CAST(35.193100 AS decimal(9,6)), CAST(-111.615300 AS decimal(9,6))),
            ('LOC-NOG-SFM-01', 'CUS-2004', N'Sonoran Fresh Nogales Facility',    N'1650 N Industrial Park', N'Nogales',     'AZ', '85621', CAST(31.364000 AS decimal(9,6)), CAST(-110.933000 AS decimal(9,6))),
            ('LOC-MES-MIC-01', 'CUS-2005', N'Mesa Components Distribution',      N'7250 E Main St',         N'Mesa',        'AZ', '85207', CAST(33.415200 AS decimal(9,6)), CAST(-111.674200 AS decimal(9,6))),
            ('LOC-SIE-BBS-01', 'CUS-2006', N'Borderland Sierra Vista Yard',       N'3900 E Fry Blvd',        N'Sierra Vista','AZ', '85635', CAST(31.554500 AS decimal(9,6)), CAST(-110.259200 AS decimal(9,6))),
            ('LOC-PRE-NAH-01', 'CUS-2007', N'Northern Arizona Prescott Center',   N'1800 Commerce Dr',       N'Prescott',    'AZ', '86301', CAST(34.570000 AS decimal(9,6)), CAST(-112.430000 AS decimal(9,6))),
            ('LOC-GLD-VTL-01', 'CUS-2008', N'Valley Technology Goodyear Campus',  N'1500 S Litchfield Rd',   N'Goodyear',    'AZ', '85338', CAST(33.421000 AS decimal(9,6)), CAST(-112.358000 AS decimal(9,6)))
        ) AS value(LocationCode, CustomerNumber, LocationName, Address1, City, StateCode, PostalCode, Latitude, Longitude)
    )
    INSERT dbo.Locations
        (CustomerId, LocationCode, LocationType, LocationName, Address1,
         City, StateCode, PostalCode, Latitude, Longitude, IsBillingLocation)
    SELECT
        customer.CustomerId,
        seed.LocationCode,
        'CUSTOMER',
        seed.LocationName,
        seed.Address1,
        seed.City,
        seed.StateCode,
        seed.PostalCode,
        seed.Latitude,
        seed.Longitude,
        1
    FROM LocationSeed AS seed
    JOIN dbo.Customers AS customer
        ON customer.CustomerNumber = seed.CustomerNumber
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.Locations AS existing
        WHERE existing.LocationCode = seed.LocationCode
    );

    /* ---------------------------------------------------------
       Drivers
       --------------------------------------------------------- */
    ;WITH DriverSeed AS
    (
        SELECT *
        FROM (VALUES
            ('DRV-201', N'Isabel', N'Ramirez', 'AZCDL200001'),
            ('DRV-202', N'Noah',   N'Wilson',  'AZCDL200002'),
            ('DRV-203', N'Emilio', N'Garcia',  'AZCDL200003'),
            ('DRV-204', N'Maya',   N'Patel',   'AZCDL200004'),
            ('DRV-205', N'Carlos', N'Mendoza', 'AZCDL200005'),
            ('DRV-206', N'Olivia', N'Brooks',  'AZCDL200006'),
            ('DRV-207', N'Javier', N'Flores',  'AZCDL200007'),
            ('DRV-208', N'Sarah',  N'Kim',     'AZCDL200008'),
            ('DRV-209', N'Adrian', N'Vega',    'AZCDL200009'),
            ('DRV-210', N'Natalie',N'Price',   'AZCDL200010'),
            ('DRV-211', N'Rafael', N'Castro',  'AZCDL200011'),
            ('DRV-212', N'Grace',  N'Howard',  'AZCDL200012')
        ) AS value(DriverNumber, FirstName, LastName, LicenseNumber)
    )
    INSERT dbo.Drivers
        (DriverNumber, FirstName, LastName, Phone, Email,
         LicenseNumber, LicenseState, LicenseExpirationDate)
    SELECT
        seed.DriverNumber,
        seed.FirstName,
        seed.LastName,
        CONCAT('520-555-', RIGHT(seed.DriverNumber, 4)),
        CONCAT(LOWER(seed.FirstName), '.', LOWER(seed.LastName), '@example.test'),
        seed.LicenseNumber,
        'AZ',
        '2029-12-31'
    FROM DriverSeed AS seed
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.Drivers AS existing
        WHERE existing.DriverNumber = seed.DriverNumber
    );

    /* ---------------------------------------------------------
       Vehicles
       --------------------------------------------------------- */
    ;WITH VehicleSeed AS
    (
        SELECT *
        FROM (VALUES
            ('TRK-201', '1FUJHHDR5PLBB0201', N'Freightliner', N'Cascadia', 'AZF201', CAST(84210.5 AS decimal(12,1))),
            ('TRK-202', '1XKYDP9X8PJBB0202', N'Kenworth',     N'T680',     'AZK202', CAST(91342.1 AS decimal(12,1))),
            ('TRK-203', '3AKJHHDR0PSBB0203', N'Freightliner', N'Cascadia', 'AZF203', CAST(45680.0 AS decimal(12,1))),
            ('TRK-204', '1FUJHHDR2PLBB0204', N'Freightliner', N'Cascadia', 'AZF204', CAST(75600.0 AS decimal(12,1))),
            ('TRK-205', '1XKYDP9X5PJBB0205', N'Kenworth',     N'T680',     'AZK205', CAST(68420.0 AS decimal(12,1))),
            ('TRK-206', '3AKJHHDR6PSBB0206', N'Freightliner', N'Cascadia', 'AZF206', CAST(51230.0 AS decimal(12,1))),
            ('TRK-207', '1FUJHHDR8PLBB0207', N'Freightliner', N'Cascadia', 'AZF207', CAST(97780.0 AS decimal(12,1))),
            ('TRK-208', '1XKYDP9X2PJBB0208', N'Kenworth',     N'T680',     'AZK208', CAST(80210.0 AS decimal(12,1))),
            ('TRK-209', '3AKJHHDR3PSBB0209', N'Freightliner', N'Cascadia', 'AZF209', CAST(43890.0 AS decimal(12,1))),
            ('TRK-210', '1FUJHHDR4PLBB0210', N'Freightliner', N'Cascadia', 'AZF210', CAST(62115.0 AS decimal(12,1))),
            ('TRK-211', '1XKYDP9X9PJBB0211', N'Kenworth',     N'T680',     'AZK211', CAST(70440.0 AS decimal(12,1))),
            ('TRK-212', '3AKJHHDR9PSBB0212', N'Freightliner', N'Cascadia', 'AZF212', CAST(39500.0 AS decimal(12,1)))
        ) AS value(UnitNumber, Vin, Make, Model, LicensePlate, OdometerMiles)
    )
    INSERT dbo.Vehicles
        (UnitNumber, Vin, ModelYear, Make, Model, LicensePlate,
         LicenseState, MaxPayloadLbs, CurrentOdometerMiles)
    SELECT
        seed.UnitNumber,
        seed.Vin,
        2023,
        seed.Make,
        seed.Model,
        seed.LicensePlate,
        'AZ',
        45000,
        seed.OdometerMiles
    FROM VehicleSeed AS seed
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.Vehicles AS existing
        WHERE existing.UnitNumber = seed.UnitNumber
    );

    /* ---------------------------------------------------------
       Trailers
       --------------------------------------------------------- */
    ;WITH TrailerSeed AS
    (
        SELECT *
        FROM (VALUES
            ('TRL-301', '1UYVS2537PPA00301', 'DRY_VAN', 'AZT301'),
            ('TRL-302', '1JJV532D8PL003302', 'REEFER',   'AZT302'),
            ('TRL-303', '1UYVS2539PPA00303', 'FLATBED',  'AZT303'),
            ('TRL-304', '1JJV532D1PL003304', 'DRY_VAN', 'AZT304'),
            ('TRL-305', '1UYVS2532PPA00305', 'REEFER',   'AZT305'),
            ('TRL-306', '1JJV532D3PL003306', 'DRY_VAN', 'AZT306'),
            ('TRL-307', '1UYVS2534PPA00307', 'FLATBED',  'AZT307'),
            ('TRL-308', '1JJV532D5PL003308', 'REEFER',   'AZT308'),
            ('TRL-309', '1UYVS2536PPA00309', 'DRY_VAN', 'AZT309'),
            ('TRL-310', '1JJV532D7PL003310', 'FLATBED',  'AZT310'),
            ('TRL-311', '1UYVS2538PPA00311', 'REEFER',   'AZT311')
        ) AS value(UnitNumber, Vin, TrailerType, LicensePlate)
    )
    INSERT dbo.Trailers
        (UnitNumber, Vin, TrailerType, LicensePlate,
         LicenseState, MaxPayloadLbs)
    SELECT
        seed.UnitNumber,
        seed.Vin,
        seed.TrailerType,
        seed.LicensePlate,
        'AZ',
        CASE WHEN seed.TrailerType = 'FLATBED' THEN 48000 ELSE 44000 END
    FROM TrailerSeed AS seed
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.Trailers AS existing
        WHERE existing.UnitNumber = seed.UnitNumber
    );

    /* ---------------------------------------------------------
       Loads: includes every load status and loads without trips.
       --------------------------------------------------------- */
    DECLARE @LoadSeed TABLE
    (
        LoadNumber varchar(30) PRIMARY KEY,
        CustomerNumber varchar(20) NOT NULL,
        Description nvarchar(300) NOT NULL,
        Commodity nvarchar(100) NULL,
        WeightLbs decimal(12,2) NOT NULL,
        Pieces int NULL,
        RevenueAmount decimal(14,2) NULL,
        LoadStatusCode varchar(30) NOT NULL
    );

    INSERT @LoadSeed
    VALUES
        ('LD-2026-0201','CUS-2001',N'Retail inventory transfer',          N'Household goods',    26500,22,1925,'PLANNED'),
        ('LD-2026-0202','CUS-2002',N'Temperature-controlled supplies',   N'Medical supplies',   18400,14,2450,'PLANNED'),
        ('LD-2026-0203','CUS-2003',N'Office furniture shipment',         N'Office furniture',   32000,36,2780,'PLANNED'),
        ('LD-2026-0204','CUS-2004',N'Fresh produce transfer',            N'Fresh produce',      29800,26,2160,'IN_TRANSIT'),
        ('LD-2026-0205','CUS-2005',N'Industrial pump components',        N'Machinery parts',    33800,18,3240,'IN_TRANSIT'),
        ('LD-2026-0206','CUS-2006',N'Construction material delivery',    N'Building materials', 41000,42,2980,'IN_TRANSIT'),
        ('LD-2026-0207','CUS-2007',N'Hotel linen replenishment',         N'Hospitality linens', 21400,30,1880,'IN_TRANSIT'),
        ('LD-2026-0208','CUS-2008',N'Data center equipment',             N'IT equipment',       19600,12,4120,'IN_TRANSIT'),
        ('LD-2026-0209','CUS-2001',N'Completed retail replenishment',    N'Retail goods',       25000,20,1810,'DELIVERED'),
        ('LD-2026-0210','CUS-2002',N'Completed clinical supply delivery',N'Medical supplies',   17200,16,2375,'DELIVERED'),
        ('LD-2026-0211','CUS-2003',N'Delayed paper products shipment',   N'Paper products',     28600,28,2050,'PLANNED'),
        ('LD-2026-0212','CUS-2004',N'Produce shipment under review',     N'Fresh produce',      27400,24,2240,'PLANNED'),
        ('LD-2026-0213','CUS-2005',N'Components delayed by breakdown',   N'Machinery parts',    35200,19,3360,'PLANNED'),
        ('LD-2026-0214','CUS-2006',N'Cancelled construction shipment',   N'Building materials', 30500,31,2210,'CANCELLED'),
        ('LD-2026-0215','CUS-2007',N'New hospitality supply request',    N'Guest supplies',     12800,44,1420,'NEW'),
        ('LD-2026-0216','CUS-2008',N'New network hardware request',      N'IT equipment',       9200, 8, 2860,'NEW'),
        ('LD-2026-0217','CUS-2001',N'Planned load awaiting a trip',      N'Retail goods',       23100,21,1740,'PLANNED'),
        ('LD-2026-0218','CUS-2003',N'Cancelled customer order',          N'Office products',    11800,15,980, 'CANCELLED');

    INSERT dbo.Loads
        (LoadNumber, CustomerId, Description, Commodity, WeightLbs,
         Pieces, RevenueAmount, LoadStatusId, SpecialInstructions)
    SELECT
        seed.LoadNumber,
        customer.CustomerId,
        seed.Description,
        seed.Commodity,
        seed.WeightLbs,
        seed.Pieces,
        seed.RevenueAmount,
        status.LoadStatusId,
        N'Extended FleetFlow demonstration data.'
    FROM @LoadSeed AS seed
    JOIN dbo.Customers AS customer
        ON customer.CustomerNumber = seed.CustomerNumber
    JOIN dbo.LoadStatuses AS status
        ON status.Code = seed.LoadStatusCode
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.Loads AS existing
        WHERE existing.LoadNumber = seed.LoadNumber
    );

    /* ---------------------------------------------------------
       Trips: one example for every trip status.
       --------------------------------------------------------- */
    DECLARE @TripSeed TABLE
    (
        TripNumber varchar(30) PRIMARY KEY,
        LoadNumber varchar(30) NOT NULL,
        TripStatusCode varchar(30) NOT NULL,
        PickupLocationCode varchar(30) NOT NULL,
        DeliveryLocationCode varchar(30) NOT NULL,
        ScheduledPickupUtc datetime2(0) NOT NULL,
        ScheduledDeliveryUtc datetime2(0) NOT NULL,
        PlannedDistanceMiles decimal(10,2) NOT NULL
    );

    INSERT @TripSeed
    VALUES
        ('TRIP-2026-0201','LD-2026-0201','PLANNED',              'LOC-TUS-CSR-01','LOC-PHX-ACS-01','2026-09-02T15:00:00','2026-09-02T19:00:00',118.40),
        ('TRIP-2026-0202','LD-2026-0202','OFFERED',              'LOC-PHX-ACS-01','LOC-FLG-GCO-01','2026-09-03T14:00:00','2026-09-03T18:30:00',145.20),
        ('TRIP-2026-0203','LD-2026-0203','ASSIGNED',             'LOC-FLG-GCO-01','LOC-PRE-NAH-01','2026-09-01T13:00:00','2026-09-01T16:00:00', 96.00),
        ('TRIP-2026-0204','LD-2026-0204','EN_ROUTE_TO_PICKUP',   'LOC-NOG-SFM-01','LOC-TUS-CSR-01','2026-08-29T17:00:00','2026-08-29T19:00:00', 66.00),
        ('TRIP-2026-0205','LD-2026-0205','AT_PICKUP',            'LOC-MES-MIC-01','LOC-GLD-VTL-01','2026-08-29T15:00:00','2026-08-29T18:30:00', 48.00),
        ('TRIP-2026-0206','LD-2026-0206','LOADED',               'LOC-SIE-BBS-01','LOC-PHX-ACS-01','2026-08-29T13:00:00','2026-08-29T18:00:00',190.00),
        ('TRIP-2026-0207','LD-2026-0207','EN_ROUTE_TO_DELIVERY', 'LOC-PRE-NAH-01','LOC-PHX-ACS-01','2026-08-29T10:00:00','2026-08-29T15:00:00',101.00),
        ('TRIP-2026-0208','LD-2026-0208','AT_DELIVERY',          'LOC-GLD-VTL-01','LOC-MES-MIC-01','2026-08-29T08:00:00','2026-08-29T12:00:00', 55.00),
        ('TRIP-2026-0209','LD-2026-0209','DELIVERED',            'LOC-TUS-CSR-01','LOC-NOG-SFM-01','2026-08-28T13:00:00','2026-08-28T16:00:00', 66.00),
        ('TRIP-2026-0210','LD-2026-0210','COMPLETED',            'LOC-PHX-ACS-01','LOC-MES-MIC-01','2026-08-27T11:00:00','2026-08-27T14:00:00', 31.00),
        ('TRIP-2026-0211','LD-2026-0211','DELAYED',              'LOC-FLG-GCO-01','LOC-PHX-ACS-01','2026-08-29T09:00:00','2026-08-29T14:00:00',145.20),
        ('TRIP-2026-0212','LD-2026-0212','INCIDENT_REPORTED',    'LOC-NOG-SFM-01','LOC-MES-MIC-01','2026-08-29T07:00:00','2026-08-29T13:00:00',168.00),
        ('TRIP-2026-0213','LD-2026-0213','VEHICLE_BREAKDOWN',    'LOC-MES-MIC-01','LOC-FLG-GCO-01','2026-08-29T06:00:00','2026-08-29T13:30:00',181.00),
        ('TRIP-2026-0214','LD-2026-0214','CANCELLED',            'LOC-SIE-BBS-01','LOC-TUS-CSR-01','2026-08-30T16:00:00','2026-08-30T18:00:00', 75.00);

    INSERT dbo.Trips
        (TripNumber, LoadId, TripStatusId, ScheduledPickupUtc,
         ScheduledDeliveryUtc, ActualStartUtc, ActualDeliveryUtc,
         PlannedDistanceMiles, ActualDistanceMiles, Notes)
    SELECT
        seed.TripNumber,
        load.LoadId,
        status.TripStatusId,
        seed.ScheduledPickupUtc,
        seed.ScheduledDeliveryUtc,
        CASE
            WHEN seed.TripStatusCode IN ('PLANNED','OFFERED','ASSIGNED','CANCELLED') THEN NULL
            ELSE DATEADD(minute, 10, seed.ScheduledPickupUtc)
        END,
        CASE
            WHEN seed.TripStatusCode IN ('DELIVERED','COMPLETED') THEN seed.ScheduledDeliveryUtc
            ELSE NULL
        END,
        seed.PlannedDistanceMiles,
        CASE
            WHEN seed.TripStatusCode IN ('DELIVERED','COMPLETED') THEN seed.PlannedDistanceMiles + 2.50
            ELSE NULL
        END,
        CONCAT(N'Extended demo trip in status ', seed.TripStatusCode, N'.')
    FROM @TripSeed AS seed
    JOIN dbo.Loads AS load
        ON load.LoadNumber = seed.LoadNumber
    JOIN dbo.TripStatuses AS status
        ON status.Code = seed.TripStatusCode
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.Trips AS existing
        WHERE existing.TripNumber = seed.TripNumber
    );

    /* ---------------------------------------------------------
       Two stops per trip with progress appropriate to its state.
       --------------------------------------------------------- */
    INSERT dbo.TripStops
        (TripId, StopSequence, StopTypeId, StopStatusId, LocationId,
         ScheduledArrivalUtc, ScheduledDepartureUtc, Instructions)
    SELECT
        trip.TripId,
        stop.StopSequence,
        stopType.StopTypeId,
        stopStatus.StopStatusId,
        location.LocationId,
        CASE WHEN stop.StopSequence = 1
             THEN seed.ScheduledPickupUtc
             ELSE seed.ScheduledDeliveryUtc END,
        CASE WHEN stop.StopSequence = 1
             THEN DATEADD(minute, 30, seed.ScheduledPickupUtc)
             ELSE DATEADD(minute, 30, seed.ScheduledDeliveryUtc) END,
        CASE WHEN stop.StopSequence = 1
             THEN N'Check in with shipping office.'
             ELSE N'Contact receiving before arrival.' END
    FROM @TripSeed AS seed
    JOIN dbo.Trips AS trip
        ON trip.TripNumber = seed.TripNumber
    CROSS JOIN (VALUES (1), (2)) AS stop(StopSequence)
    JOIN dbo.StopTypes AS stopType
        ON stopType.Code = CASE WHEN stop.StopSequence = 1 THEN 'PICKUP' ELSE 'DELIVERY' END
    JOIN dbo.StopStatuses AS stopStatus
        ON stopStatus.Code =
            CASE
                WHEN seed.TripStatusCode IN ('DELIVERED','COMPLETED') THEN 'COMPLETED'
                WHEN seed.TripStatusCode = 'CANCELLED' THEN 'SKIPPED'
                WHEN stop.StopSequence = 1 AND seed.TripStatusCode IN
                    ('LOADED','EN_ROUTE_TO_DELIVERY','AT_DELIVERY','DELAYED','INCIDENT_REPORTED','VEHICLE_BREAKDOWN')
                    THEN 'COMPLETED'
                WHEN stop.StopSequence = 1 AND seed.TripStatusCode = 'AT_PICKUP' THEN 'ARRIVED'
                WHEN stop.StopSequence = 1 AND seed.TripStatusCode = 'EN_ROUTE_TO_PICKUP' THEN 'EN_ROUTE'
                WHEN stop.StopSequence = 2 AND seed.TripStatusCode = 'AT_DELIVERY' THEN 'ARRIVED'
                WHEN stop.StopSequence = 2 AND seed.TripStatusCode = 'EN_ROUTE_TO_DELIVERY' THEN 'EN_ROUTE'
                ELSE 'PLANNED'
            END
    JOIN dbo.Locations AS location
        ON location.LocationCode = CASE WHEN stop.StopSequence = 1
                                       THEN seed.PickupLocationCode
                                       ELSE seed.DeliveryLocationCode END
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.TripStops AS existing
        WHERE existing.TripId = trip.TripId
          AND existing.StopSequence = stop.StopSequence
    );

    /* One timeline entry guarantees useful Trip Details history. */
    INSERT dbo.TripStatusHistory
        (TripId, PreviousTripStatusId, NewTripStatusId,
         ChangedAtUtc, ChangedBy, Source, Notes)
    SELECT
        trip.TripId,
        CASE WHEN seed.TripStatusCode = 'PLANNED' THEN NULL ELSE planned.TripStatusId END,
        currentStatus.TripStatusId,
        DATEADD(hour, -2, seed.ScheduledPickupUtc),
        N'Extended Demo Seed',
        'SYSTEM',
        CONCAT(N'Demo trip initialized in status ', currentStatus.DisplayName, N'.')
    FROM @TripSeed AS seed
    JOIN dbo.Trips AS trip
        ON trip.TripNumber = seed.TripNumber
    JOIN dbo.TripStatuses AS currentStatus
        ON currentStatus.Code = seed.TripStatusCode
    CROSS JOIN
    (
        SELECT TripStatusId
        FROM dbo.TripStatuses
        WHERE Code = 'PLANNED'
    ) AS planned
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.TripStatusHistory AS existing
        WHERE existing.TripId = trip.TripId
    );

    /* ---------------------------------------------------------
       Active assignments for dispatchable trips.
       Each active trip uses a different driver and asset.
       --------------------------------------------------------- */
    DECLARE @AssignmentSeed TABLE
    (
        TripNumber varchar(30) PRIMARY KEY,
        DriverNumber varchar(20) NOT NULL,
        VehicleUnitNumber varchar(20) NOT NULL,
        TrailerUnitNumber varchar(20) NOT NULL,
        AssignmentStatusCode varchar(30) NOT NULL
    );

    INSERT @AssignmentSeed
    VALUES
        ('TRIP-2026-0202','DRV-201','TRK-201','TRL-301','OFFERED'),
        ('TRIP-2026-0203','DRV-202','TRK-202','TRL-302','ACCEPTED'),
        ('TRIP-2026-0204','DRV-203','TRK-203','TRL-303','ACTIVE'),
        ('TRIP-2026-0205','DRV-204','TRK-204','TRL-304','ACTIVE'),
        ('TRIP-2026-0206','DRV-205','TRK-205','TRL-305','ACTIVE'),
        ('TRIP-2026-0207','DRV-206','TRK-206','TRL-306','ACTIVE'),
        ('TRIP-2026-0208','DRV-207','TRK-207','TRL-307','ACTIVE'),
        ('TRIP-2026-0209','DRV-208','TRK-208','TRL-308','ACTIVE'),
        ('TRIP-2026-0211','DRV-209','TRK-209','TRL-309','ACTIVE'),
        ('TRIP-2026-0212','DRV-210','TRK-210','TRL-310','ACTIVE'),
        ('TRIP-2026-0213','DRV-211','TRK-211','TRL-311','ACTIVE');

    INSERT dbo.TripAssignments
        (TripId, DriverId, VehicleId, TrailerId, AssignmentStatusId,
         IsActive, OfferedAtUtc, RespondedAtUtc, AcceptedAtUtc, AssignedBy)
    SELECT
        trip.TripId,
        driver.DriverId,
        vehicle.VehicleId,
        trailer.TrailerId,
        assignmentStatus.AssignmentStatusId,
        1,
        DATEADD(hour, -4, trip.ScheduledPickupUtc),
        CASE WHEN seed.AssignmentStatusCode <> 'OFFERED'
             THEN DATEADD(hour, -3, trip.ScheduledPickupUtc) END,
        CASE WHEN seed.AssignmentStatusCode <> 'OFFERED'
             THEN DATEADD(hour, -3, trip.ScheduledPickupUtc) END,
        N'Extended Demo Dispatcher'
    FROM @AssignmentSeed AS seed
    JOIN dbo.Trips AS trip
        ON trip.TripNumber = seed.TripNumber
    JOIN dbo.Drivers AS driver
        ON driver.DriverNumber = seed.DriverNumber
    JOIN dbo.Vehicles AS vehicle
        ON vehicle.UnitNumber = seed.VehicleUnitNumber
    JOIN dbo.Trailers AS trailer
        ON trailer.UnitNumber = seed.TrailerUnitNumber
    JOIN dbo.AssignmentStatuses AS assignmentStatus
        ON assignmentStatus.Code = seed.AssignmentStatusCode
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.TripAssignments AS existing
        WHERE existing.TripId = trip.TripId
          AND existing.IsActive = 1
    );

    /* Reflect assignment state on drivers and fleet assets. */
    UPDATE driver
    SET DriverStatusId = status.DriverStatusId,
        UpdatedAtUtc = SYSUTCDATETIME()
    FROM dbo.Drivers AS driver
    JOIN @AssignmentSeed AS seed
        ON seed.DriverNumber = driver.DriverNumber
    JOIN dbo.Trips AS trip
        ON trip.TripNumber = seed.TripNumber
    JOIN dbo.TripStatuses AS tripStatus
        ON tripStatus.TripStatusId = trip.TripStatusId
    JOIN dbo.DriverStatuses AS status
        ON status.Code = CASE
            WHEN tripStatus.Code = 'OFFERED' THEN 'OFFERED'
            WHEN tripStatus.Code = 'ASSIGNED' THEN 'ASSIGNED'
            ELSE 'ON_TRIP'
        END;

    UPDATE vehicle
    SET FleetAssetStatusId = status.FleetAssetStatusId,
        UpdatedAtUtc = SYSUTCDATETIME()
    FROM dbo.Vehicles AS vehicle
    JOIN @AssignmentSeed AS seed
        ON seed.VehicleUnitNumber = vehicle.UnitNumber
    JOIN dbo.Trips AS trip
        ON trip.TripNumber = seed.TripNumber
    JOIN dbo.TripStatuses AS tripStatus
        ON tripStatus.TripStatusId = trip.TripStatusId
    JOIN dbo.FleetAssetStatuses AS status
        ON status.Code = CASE
            WHEN tripStatus.Code = 'VEHICLE_BREAKDOWN' THEN 'MAINTENANCE'
            WHEN tripStatus.Code IN ('OFFERED','ASSIGNED') THEN 'ASSIGNED'
            ELSE 'IN_TRANSIT'
        END;

    UPDATE trailer
    SET FleetAssetStatusId = status.FleetAssetStatusId,
        UpdatedAtUtc = SYSUTCDATETIME()
    FROM dbo.Trailers AS trailer
    JOIN @AssignmentSeed AS seed
        ON seed.TrailerUnitNumber = trailer.UnitNumber
    JOIN dbo.Trips AS trip
        ON trip.TripNumber = seed.TripNumber
    JOIN dbo.TripStatuses AS tripStatus
        ON tripStatus.TripStatusId = trip.TripStatusId
    JOIN dbo.FleetAssetStatuses AS status
        ON status.Code = CASE
            WHEN tripStatus.Code IN ('OFFERED','ASSIGNED') THEN 'ASSIGNED'
            ELSE 'IN_TRANSIT'
        END;

    COMMIT TRANSACTION;

    /* Return validation summaries after a successful seed. */
    SELECT ls.Code AS LoadStatusCode, COUNT(*) AS LoadCount
    FROM dbo.Loads AS load
    JOIN dbo.LoadStatuses AS ls ON ls.LoadStatusId = load.LoadStatusId
    GROUP BY ls.Code, ls.SortOrder
    ORDER BY ls.SortOrder;

    SELECT ts.Code AS TripStatusCode, COUNT(*) AS TripCount
    FROM dbo.Trips AS trip
    JOIN dbo.TripStatuses AS ts ON ts.TripStatusId = trip.TripStatusId
    GROUP BY ts.Code, ts.SortOrder
    ORDER BY ts.SortOrder;

    SELECT
        (SELECT COUNT(*) FROM dbo.Customers) AS Customers,
        (SELECT COUNT(*) FROM dbo.Loads) AS Loads,
        (SELECT COUNT(*) FROM dbo.Trips) AS Trips,
        (SELECT COUNT(*) FROM dbo.TripStops) AS TripStops,
        (SELECT COUNT(*) FROM dbo.TripAssignments WHERE IsActive = 1) AS ActiveAssignments;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
GO
