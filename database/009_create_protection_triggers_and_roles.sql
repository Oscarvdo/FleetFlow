/* Append-only protection and deployable SQL database roles. */
USE FleetFlowDb;
GO

CREATE OR ALTER TRIGGER dbo.TR_TripStatusHistory_Immutable
ON dbo.TripStatusHistory
INSTEAD OF UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    THROW 51100, 'Trip status history is append-only and cannot be updated or deleted.', 1;
END;
GO

CREATE OR ALTER TRIGGER dbo.TR_TripEvents_NoUpdate
ON dbo.TripEvents
INSTEAD OF UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    THROW 51101, 'Trip events are immutable. Append a corrective event instead.', 1;
END;
GO

CREATE OR ALTER TRIGGER dbo.TR_SecurityAuditLog_Immutable
ON dbo.SecurityAuditLog
INSTEAD OF UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    THROW 51102, 'Security audit records are append-only.', 1;
END;
GO

CREATE OR ALTER TRIGGER dbo.TR_ChangeAuditLog_Immutable
ON dbo.ChangeAuditLog
INSTEAD OF UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    THROW 51103, 'Operational audit records are append-only.', 1;
END;
GO

IF DATABASE_PRINCIPAL_ID(N'FleetFlowAppExecutor') IS NULL
    CREATE ROLE FleetFlowAppExecutor AUTHORIZATION dbo;
IF DATABASE_PRINCIPAL_ID(N'FleetFlowReportReader') IS NULL
    CREATE ROLE FleetFlowReportReader AUTHORIZATION dbo;
GO

GRANT EXECUTE ON SCHEMA::catalog TO FleetFlowAppExecutor;
GRANT EXECUTE ON SCHEMA::operations TO FleetFlowAppExecutor;
GRANT EXECUTE ON SCHEMA::dispatch TO FleetFlowAppExecutor;
GRANT EXECUTE ON SCHEMA::tracking TO FleetFlowAppExecutor;
GRANT EXECUTE ON SCHEMA::import TO FleetFlowAppExecutor;
GRANT EXECUTE ON SCHEMA::simulation TO FleetFlowAppExecutor;
GRANT EXECUTE ON SCHEMA::security TO FleetFlowAppExecutor;
GRANT SELECT ON SCHEMA::catalog TO FleetFlowAppExecutor;
GRANT SELECT ON SCHEMA::operations TO FleetFlowAppExecutor;
GRANT SELECT ON SCHEMA::dispatch TO FleetFlowAppExecutor;
GRANT SELECT ON SCHEMA::tracking TO FleetFlowAppExecutor;
GRANT SELECT ON SCHEMA::import TO FleetFlowAppExecutor;
GRANT SELECT ON SCHEMA::simulation TO FleetFlowAppExecutor;
GRANT SELECT ON SCHEMA::security TO FleetFlowAppExecutor;
GRANT SELECT ON SCHEMA::reporting TO FleetFlowAppExecutor;
GRANT SELECT ON SCHEMA::reporting TO FleetFlowReportReader;
GO

/* Production service accounts are added to these roles during deployment.
   LocalDB development normally runs as the developer/database owner. */
