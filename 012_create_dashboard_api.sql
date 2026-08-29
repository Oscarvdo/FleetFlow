/*
    FleetFlow Dashboard API
    Provides operational summary indicators for WinForms.
*/

USE FleetFlowDb;
GO

CREATE OR ALTER PROCEDURE dispatch.Dashboard_GetSummary
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NowUtc datetime2(3) = SYSUTCDATETIME();

    SELECT
        ActiveTrips =
        (
            SELECT COUNT(*)
            FROM dbo.Trips AS trip
            JOIN dbo.TripStatuses AS status
                ON status.TripStatusId = trip.TripStatusId
            WHERE status.IsTerminal = 0
        ),

        AvailableDrivers =
        (
            SELECT COUNT(*)
            FROM dbo.Drivers AS driver
            JOIN dbo.DriverStatuses AS status
                ON status.DriverStatusId = driver.DriverStatusId
            WHERE driver.IsActive = 1
              AND status.IsAvailable = 1
              AND driver.LicenseExpirationDate >=
                  CONVERT(date, @NowUtc)
        ),

        AvailableVehicles =
        (
            SELECT COUNT(*)
            FROM dbo.Vehicles AS vehicle
            JOIN dbo.FleetAssetStatuses AS status
                ON status.FleetAssetStatusId =
                   vehicle.FleetAssetStatusId
            WHERE vehicle.IsActive = 1
              AND status.IsAvailable = 1
        ),

        PendingLoads =
        (
            SELECT COUNT(*)
            FROM dbo.Loads AS loadRecord
            JOIN dbo.LoadStatuses AS status
                ON status.LoadStatusId =
                   loadRecord.LoadStatusId
            WHERE status.Code IN
            (
                'NEW',
                'PLANNED'
            )
        ),

        DelayedTrips =
        (
            SELECT COUNT(*)
            FROM dbo.Trips AS trip
            JOIN dbo.TripStatuses AS status
                ON status.TripStatusId = trip.TripStatusId
            WHERE status.IsTerminal = 0
              AND
              (
                  status.Code = 'DELAYED'
                  OR trip.ScheduledDeliveryUtc < @NowUtc
              )
        ),

        ActiveIncidents =
        (
            SELECT COUNT(*)
            FROM dbo.Trips AS trip
            JOIN dbo.TripStatuses AS status
                ON status.TripStatusId = trip.TripStatusId
            WHERE status.IsTerminal = 0
              AND status.Code IN
              (
                  'INCIDENT_REPORTED',
                  'VEHICLE_BREAKDOWN'
              )
        ),

        TrackedVehicles =
        (
            SELECT COUNT(*)
            FROM dbo.VehicleCurrentState AS currentState
            JOIN dbo.Vehicles AS vehicle
                ON vehicle.VehicleId = currentState.VehicleId
            WHERE vehicle.IsActive = 1
              AND currentState.RecordedAtUtc >=
                  DATEADD(minute, -15, @NowUtc)
        ),

        GeneratedAtUtc = @NowUtc;
END;
GO

GRANT EXECUTE
ON OBJECT::dispatch.Dashboard_GetSummary
TO FleetFlowAppExecutor;
GO