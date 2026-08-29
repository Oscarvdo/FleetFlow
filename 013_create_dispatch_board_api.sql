/*
    FleetFlow Dispatch Board API
    Returns active trips for the WinForms dispatch board.
*/

USE FleetFlowDb;
GO

CREATE OR ALTER PROCEDURE dispatch.DispatchBoard_GetActive
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        TripId,
        TripNumber,
        TripStatusCode,
        TripStatus,
        LoadId,
        LoadNumber,
        CustomerId,
        Customer,
        TripAssignmentId,
        AssignmentStatusCode,
        DriverId,
        DriverNumber,
        DriverName,
        VehicleId,
        VehicleUnitNumber,
        TrailerId,
        TrailerUnitNumber,
        ScheduledPickupUtc,
        ScheduledDeliveryUtc,
        PickupLocation,
        DeliveryLocation,
        LastTelemetryUtc,
        Latitude,
        Longitude,
        SpeedMph,
        FuelPercent,
        ProgressPercent,
        RowVersion
    FROM dispatch.vw_DispatchBoard
    ORDER BY
        ScheduledPickupUtc,
        TripNumber;
END;
GO

GRANT EXECUTE
ON OBJECT::dispatch.DispatchBoard_GetActive
TO FleetFlowAppExecutor;
GO