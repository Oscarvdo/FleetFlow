USE FleetFlowDb;
GO

DECLARE @TripId bigint =
(
    SELECT TripId
    FROM dbo.Trips
    WHERE TripNumber = 'TRIP-2026-0001'
);

EXEC operations.Trip_GetDetails
    @TripId = @TripId;
GO