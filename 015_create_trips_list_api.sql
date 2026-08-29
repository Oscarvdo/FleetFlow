USE FleetFlowDb;
GO

CREATE OR ALTER PROCEDURE operations.Trip_GetList
    @StatusCode varchar(30) = NULL,
    @SearchText nvarchar(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SET @StatusCode =
        NULLIF(LTRIM(RTRIM(@StatusCode)), '');

    SET @SearchText =
        NULLIF(LTRIM(RTRIM(@SearchText)), '');

    SELECT
        TripId,
        TripNumber,
        LoadId,
        LoadNumber,
        CustomerId,
        CustomerNumber,
        CompanyName AS Customer,
        TripStatusCode,
        TripStatus,
        ScheduledPickupUtc,
        ScheduledDeliveryUtc,
        ActualStartUtc,
        ActualDeliveryUtc,
        PlannedDistanceMiles,
        ActualDistanceMiles,
        TotalStops,
        CompletedStops,
        ProgressPercent,
        RowVersion
    FROM operations.vw_TripDetails
    WHERE
        (
            @StatusCode IS NULL
            OR TripStatusCode = @StatusCode
        )
        AND
        (
            @SearchText IS NULL
            OR TripNumber LIKE '%' + @SearchText + '%'
            OR LoadNumber LIKE '%' + @SearchText + '%'
            OR CustomerNumber LIKE '%' + @SearchText + '%'
            OR CompanyName LIKE '%' + @SearchText + '%'
        )
    ORDER BY
        ScheduledPickupUtc DESC,
        TripNumber DESC;
END;
GO

EXEC operations.Trip_GetList;
GO