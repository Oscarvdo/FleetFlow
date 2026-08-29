USE FleetFlowDb;
GO

/*
    Devuelve la información completa de una carga y,
    cuando exista, los datos de su viaje relacionado.
*/
CREATE OR ALTER PROCEDURE operations.Load_GetDetails
    @LoadId bigint
AS
BEGIN
    SET NOCOUNT ON;

    IF @LoadId <= 0
    BEGIN
        THROW 51000, 'A valid LoadId is required.', 1;
    END;

    SELECT
        load.LoadId,
        load.LoadNumber,

        load.CustomerId,
        customer.CustomerNumber,
        customer.CompanyName AS Customer,
        customer.ContactName AS CustomerContactName,
        customer.Email AS CustomerEmail,
        customer.Phone AS CustomerPhone,

        load.Description,
        load.Commodity,
        load.WeightLbs,
        load.Pieces,
        load.RevenueAmount,
        load.SpecialInstructions,

        loadStatus.Code AS LoadStatusCode,
        loadStatus.DisplayName AS LoadStatus,

        trip.TripId,
        trip.TripNumber,
        tripStatus.Code AS TripStatusCode,
        tripStatus.DisplayName AS TripStatus,
        trip.ScheduledPickupUtc,
        trip.ScheduledDeliveryUtc,

        load.CreatedAtUtc,
        load.UpdatedAtUtc,
        load.RowVersion
    FROM dbo.Loads AS load
    INNER JOIN dbo.Customers AS customer
        ON customer.CustomerId = load.CustomerId
    INNER JOIN dbo.LoadStatuses AS loadStatus
        ON loadStatus.LoadStatusId =
            load.LoadStatusId
    LEFT JOIN dbo.Trips AS trip
        ON trip.LoadId = load.LoadId
    LEFT JOIN dbo.TripStatuses AS tripStatus
        ON tripStatus.TripStatusId =
            trip.TripStatusId
    WHERE load.LoadId = @LoadId;
END;
GO

/*
    Validación con la primera carga disponible.
*/
DECLARE @TestLoadId bigint =
(
    SELECT TOP (1) LoadId
    FROM dbo.Loads
    ORDER BY LoadId
);

EXEC operations.Load_GetDetails
    @LoadId = @TestLoadId;
GO