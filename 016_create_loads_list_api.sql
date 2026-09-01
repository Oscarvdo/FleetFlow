USE FleetFlowDb;
GO

CREATE OR ALTER PROCEDURE operations.Load_GetList
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
        l.LoadId,
        l.LoadNumber,
        l.CustomerId,
        c.CustomerNumber,
        c.CompanyName AS Customer,
        l.Description,
        l.Commodity,
        l.WeightLbs,
        l.Pieces,
        l.RevenueAmount,
        ls.Code AS LoadStatusCode,
        ls.DisplayName AS LoadStatus,
        t.TripId,
        t.TripNumber,
        t.ScheduledPickupUtc,
        t.ScheduledDeliveryUtc,
        l.CreatedAtUtc,
        l.UpdatedAtUtc,
        l.RowVersion
    FROM dbo.Loads AS l
    INNER JOIN dbo.Customers AS c
        ON c.CustomerId = l.CustomerId
    INNER JOIN dbo.LoadStatuses AS ls
        ON ls.LoadStatusId = l.LoadStatusId
    LEFT JOIN dbo.Trips AS t
        ON t.LoadId = l.LoadId
    WHERE
        (
            @StatusCode IS NULL
            OR ls.Code = @StatusCode
        )
        AND
        (
            @SearchText IS NULL
            OR l.LoadNumber LIKE
                '%' + @SearchText + '%'
            OR c.CustomerNumber LIKE
                '%' + @SearchText + '%'
            OR c.CompanyName LIKE
                '%' + @SearchText + '%'
            OR l.Description LIKE
                '%' + @SearchText + '%'
            OR l.Commodity LIKE
                '%' + @SearchText + '%'
            OR t.TripNumber LIKE
                '%' + @SearchText + '%'
        )
    ORDER BY
        l.CreatedAtUtc DESC,
        l.LoadNumber DESC;
END;
GO

EXEC operations.Load_GetList;
GO