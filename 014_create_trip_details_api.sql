/*
    FleetFlow Trip Details API

    Returns three result sets:
    1. General trip information
    2. Ordered trip stops
    3. Trip status timeline
*/

USE FleetFlowDb;
GO

CREATE OR ALTER PROCEDURE operations.Trip_GetDetails
    @TripId bigint
AS
BEGIN
    SET NOCOUNT ON;

    /* Result set 1: general trip information. */
    SELECT
        TripId,
        TripNumber,
        LoadId,
        LoadNumber,
        LoadDescription,
        WeightLbs,
        CustomerId,
        CustomerNumber,
        CompanyName,
        TripStatusCode,
        TripStatus,
        ScheduledPickupUtc,
        ScheduledDeliveryUtc,
        ActualStartUtc,
        ActualDeliveryUtc,
        PlannedDistanceMiles,
        ActualDistanceMiles,
        Notes,
        TotalStops,
        CompletedStops,
        ProgressPercent,
        RowVersion
    FROM operations.vw_TripDetails
    WHERE TripId = @TripId;

    /* Result set 2: ordered stops. */
    SELECT
        TripStopId,
        TripId,
        TripNumber,
        StopSequence,
        StopTypeCode,
        StopStatusCode,
        LocationId,
        LocationCode,
        LocationName,
        Address1,
        Address2,
        City,
        StateCode,
        PostalCode,
        Latitude,
        Longitude,
        ScheduledArrivalUtc,
        ScheduledDepartureUtc,
        ActualArrivalUtc,
        ActualDepartureUtc,
        Instructions,
        RowVersion
    FROM operations.vw_TripStops
    WHERE TripId = @TripId
    ORDER BY StopSequence;

    /* Result set 3: status history. */
    SELECT
        TripStatusHistoryId,
        TripId,
        TripNumber,
        PreviousStatusCode,
        NewStatusCode,
        ChangedAtUtc,
        ChangedBy,
        Source,
        Notes
    FROM operations.vw_TripStatusTimeline
    WHERE TripId = @TripId
    ORDER BY ChangedAtUtc DESC,
             TripStatusHistoryId DESC;
END;
GO

GRANT EXECUTE
ON OBJECT::operations.Trip_GetDetails
TO FleetFlowAppExecutor;
GO