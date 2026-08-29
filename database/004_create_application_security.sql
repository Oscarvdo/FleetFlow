/*
    FleetFlow application security (RBAC) for SQL Server.

    This script creates FleetFlow application accounts, roles, permissions,
    role assignments, and security auditing. It does not create SQL Server
    logins and it never inserts a plaintext or demonstration password.
*/

USE FleetFlowDb;
GO

SET XACT_ABORT ON;
GO

CREATE TABLE dbo.AppUsers
(
    AppUserId bigint IDENTITY(1,1) NOT NULL,
    Username nvarchar(80) NOT NULL,
    NormalizedUsername nvarchar(80) NOT NULL,
    Email varchar(254) NOT NULL,
    NormalizedEmail varchar(254) NOT NULL,
    PasswordHash nvarchar(500) NOT NULL,
    SecurityStamp uniqueidentifier NOT NULL CONSTRAINT DF_AppUsers_SecurityStamp DEFAULT (NEWID()),
    DriverId bigint NULL,
    IsActive bit NOT NULL CONSTRAINT DF_AppUsers_IsActive DEFAULT (1),
    MustChangePassword bit NOT NULL CONSTRAINT DF_AppUsers_MustChangePassword DEFAULT (0),
    FailedLoginAttempts smallint NOT NULL CONSTRAINT DF_AppUsers_FailedLoginAttempts DEFAULT (0),
    LockoutEndUtc datetime2(0) NULL,
    LastLoginAtUtc datetime2(0) NULL,
    CreatedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_AppUsers_CreatedAtUtc DEFAULT (SYSUTCDATETIME()),
    UpdatedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_AppUsers_UpdatedAtUtc DEFAULT (SYSUTCDATETIME()),
    RowVersion rowversion NOT NULL,
    CONSTRAINT PK_AppUsers PRIMARY KEY (AppUserId),
    CONSTRAINT UQ_AppUsers_NormalizedUsername UNIQUE (NormalizedUsername),
    CONSTRAINT UQ_AppUsers_NormalizedEmail UNIQUE (NormalizedEmail),
    CONSTRAINT CK_AppUsers_FailedLoginAttempts CHECK (FailedLoginAttempts >= 0),
    CONSTRAINT CK_AppUsers_Lockout CHECK
    (
        LockoutEndUtc IS NULL OR LockoutEndUtc >= CreatedAtUtc
    ),
    CONSTRAINT FK_AppUsers_Drivers FOREIGN KEY (DriverId)
        REFERENCES dbo.Drivers (DriverId)
);
GO

CREATE UNIQUE INDEX UX_AppUsers_DriverId
    ON dbo.AppUsers (DriverId)
    WHERE DriverId IS NOT NULL;
GO

CREATE TABLE dbo.Roles
(
    RoleId smallint NOT NULL,
    Code varchar(40) NOT NULL,
    DisplayName nvarchar(80) NOT NULL,
    Description nvarchar(300) NULL,
    IsSystemRole bit NOT NULL CONSTRAINT DF_Roles_IsSystemRole DEFAULT (1),
    IsActive bit NOT NULL CONSTRAINT DF_Roles_IsActive DEFAULT (1),
    CONSTRAINT PK_Roles PRIMARY KEY (RoleId),
    CONSTRAINT UQ_Roles_Code UNIQUE (Code)
);
GO

CREATE TABLE dbo.Permissions
(
    PermissionId smallint NOT NULL,
    Code varchar(80) NOT NULL,
    DisplayName nvarchar(120) NOT NULL,
    Module varchar(40) NOT NULL,
    Description nvarchar(300) NULL,
    CONSTRAINT PK_Permissions PRIMARY KEY (PermissionId),
    CONSTRAINT UQ_Permissions_Code UNIQUE (Code)
);
GO

CREATE TABLE dbo.UserRoles
(
    AppUserId bigint NOT NULL,
    RoleId smallint NOT NULL,
    AssignedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_UserRoles_AssignedAtUtc DEFAULT (SYSUTCDATETIME()),
    AssignedByAppUserId bigint NULL,
    CONSTRAINT PK_UserRoles PRIMARY KEY (AppUserId, RoleId),
    CONSTRAINT FK_UserRoles_AppUsers FOREIGN KEY (AppUserId)
        REFERENCES dbo.AppUsers (AppUserId),
    CONSTRAINT FK_UserRoles_Roles FOREIGN KEY (RoleId)
        REFERENCES dbo.Roles (RoleId),
    CONSTRAINT FK_UserRoles_AssignedBy FOREIGN KEY (AssignedByAppUserId)
        REFERENCES dbo.AppUsers (AppUserId)
);
GO

CREATE TABLE dbo.RolePermissions
(
    RoleId smallint NOT NULL,
    PermissionId smallint NOT NULL,
    GrantedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_RolePermissions_GrantedAtUtc DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT PK_RolePermissions PRIMARY KEY (RoleId, PermissionId),
    CONSTRAINT FK_RolePermissions_Roles FOREIGN KEY (RoleId)
        REFERENCES dbo.Roles (RoleId),
    CONSTRAINT FK_RolePermissions_Permissions FOREIGN KEY (PermissionId)
        REFERENCES dbo.Permissions (PermissionId)
);
GO

CREATE TABLE dbo.SecurityAuditLog
(
    SecurityAuditLogId bigint IDENTITY(1,1) NOT NULL,
    AppUserId bigint NULL,
    UsernameAttempted nvarchar(80) NULL,
    EventType varchar(40) NOT NULL,
    OccurredAtUtc datetime2(3) NOT NULL CONSTRAINT DF_SecurityAuditLog_OccurredAtUtc DEFAULT (SYSUTCDATETIME()),
    WasSuccessful bit NOT NULL,
    ClientApplication varchar(40) NOT NULL,
    DeviceIdentifier nvarchar(120) NULL,
    IpAddress varchar(45) NULL,
    Details nvarchar(1000) NULL,
    CONSTRAINT PK_SecurityAuditLog PRIMARY KEY (SecurityAuditLogId),
    CONSTRAINT CK_SecurityAuditLog_EventType CHECK
    (
        EventType IN
        (
            'LOGIN', 'LOGOUT', 'LOGIN_FAILED', 'ACCOUNT_LOCKED',
            'PASSWORD_CHANGED', 'USER_CREATED', 'USER_UPDATED',
            'ROLE_ASSIGNED', 'ROLE_REMOVED', 'ACCESS_DENIED'
        )
    ),
    CONSTRAINT CK_SecurityAuditLog_ClientApplication CHECK
    (
        ClientApplication IN ('DISPATCH_WINFORMS', 'DRIVER_MAUI', 'API', 'SYSTEM')
    ),
    CONSTRAINT FK_SecurityAuditLog_AppUsers FOREIGN KEY (AppUserId)
        REFERENCES dbo.AppUsers (AppUserId)
);
GO

CREATE INDEX IX_SecurityAuditLog_User_OccurredAt
    ON dbo.SecurityAuditLog (AppUserId, OccurredAtUtc DESC);
GO

CREATE INDEX IX_SecurityAuditLog_Event_OccurredAt
    ON dbo.SecurityAuditLog (EventType, OccurredAtUtc DESC);
GO

ALTER TABLE dbo.ImportBatches
    ADD CreatedByAppUserId bigint NULL;
GO

ALTER TABLE dbo.ImportBatches
    ADD CONSTRAINT FK_ImportBatches_CreatedByAppUsers
        FOREIGN KEY (CreatedByAppUserId) REFERENCES dbo.AppUsers (AppUserId);
GO

ALTER TABLE dbo.SimulationRuns
    ADD CreatedByAppUserId bigint NULL;
GO

ALTER TABLE dbo.SimulationRuns
    ADD CONSTRAINT FK_SimulationRuns_CreatedByAppUsers
        FOREIGN KEY (CreatedByAppUserId) REFERENCES dbo.AppUsers (AppUserId);
GO

BEGIN TRANSACTION;

INSERT dbo.Roles (RoleId, Code, DisplayName, Description)
VALUES
    (1, 'ADMINISTRATOR', N'Administrator', N'Full FleetFlow administration and security access.'),
    (2, 'FLEET_MANAGER', N'Fleet Manager', N'Manages fleet resources, operations, and reports.'),
    (3, 'DISPATCHER', N'Dispatcher', N'Creates and assigns trips and monitors active operations.'),
    (4, 'DRIVER', N'Driver', N'Uses the mobile application to respond to assignments and update trip progress.'),
    (5, 'READ_ONLY', N'Read Only', N'Can view operational records but cannot modify them.');

INSERT dbo.Permissions (PermissionId, Code, DisplayName, Module, Description)
VALUES
    (1,  'SECURITY.USERS.VIEW', N'View users', 'SECURITY', N'View FleetFlow user accounts.'),
    (2,  'SECURITY.USERS.MANAGE', N'Manage users', 'SECURITY', N'Create, activate, deactivate, and update users.'),
    (3,  'SECURITY.ROLES.MANAGE', N'Manage roles', 'SECURITY', N'Assign or remove application roles.'),
    (4,  'SECURITY.AUDIT.VIEW', N'View security audit', 'SECURITY', N'View authentication and authorization events.'),
    (5,  'FLEET.VIEW', N'View fleet', 'FLEET', N'View trucks, trailers, and drivers.'),
    (6,  'FLEET.MANAGE', N'Manage fleet', 'FLEET', N'Create and update trucks, trailers, and drivers.'),
    (7,  'CUSTOMERS.VIEW', N'View customers', 'CUSTOMERS', N'View customers and locations.'),
    (8,  'CUSTOMERS.MANAGE', N'Manage customers', 'CUSTOMERS', N'Create and update customers and locations.'),
    (9,  'LOADS.VIEW', N'View loads', 'LOADS', N'View customer loads.'),
    (10, 'LOADS.MANAGE', N'Manage loads', 'LOADS', N'Create and update customer loads.'),
    (11, 'TRIPS.VIEW', N'View trips', 'TRIPS', N'View trips, stops, and status history.'),
    (12, 'TRIPS.CREATE', N'Create trips', 'TRIPS', N'Create trips and ordered stops.'),
    (13, 'TRIPS.ASSIGN', N'Assign trips', 'TRIPS', N'Offer or assign trips to drivers and equipment.'),
    (14, 'TRIPS.UPDATE', N'Update trips', 'TRIPS', N'Update trip information and operational status.'),
    (15, 'DISPATCH.VIEW', N'View dispatch board', 'DISPATCH', N'View active assignments and events.'),
    (16, 'REPORTS.VIEW', N'View reports', 'REPORTS', N'View and export operational reports.'),
    (17, 'DRIVER.ASSIGNMENTS.VIEW', N'View driver assignments', 'DRIVER_APP', N'View assignments for the authenticated driver.'),
    (18, 'DRIVER.ASSIGNMENTS.RESPOND', N'Respond to assignments', 'DRIVER_APP', N'Accept or reject an offered trip.'),
    (19, 'DRIVER.TRIP_STATUS.UPDATE', N'Update driver trip status', 'DRIVER_APP', N'Update pickup, transit, and delivery progress.'),
    (20, 'DRIVER.INCIDENTS.CREATE', N'Report driver incident', 'DRIVER_APP', N'Report an operational incident from the mobile app.');

/* Administrator receives every permission. */
INSERT dbo.RolePermissions (RoleId, PermissionId)
SELECT 1, PermissionId
FROM dbo.Permissions;

/* Fleet Manager receives all operational permissions and read-only security visibility. */
INSERT dbo.RolePermissions (RoleId, PermissionId)
SELECT 2, PermissionId
FROM dbo.Permissions
WHERE Code IN
(
    'SECURITY.USERS.VIEW', 'SECURITY.AUDIT.VIEW',
    'FLEET.VIEW', 'FLEET.MANAGE',
    'CUSTOMERS.VIEW', 'CUSTOMERS.MANAGE',
    'LOADS.VIEW', 'LOADS.MANAGE',
    'TRIPS.VIEW', 'TRIPS.CREATE', 'TRIPS.ASSIGN', 'TRIPS.UPDATE',
    'DISPATCH.VIEW', 'REPORTS.VIEW'
);

/* Dispatcher operates trips but does not administer security or fleet records. */
INSERT dbo.RolePermissions (RoleId, PermissionId)
SELECT 3, PermissionId
FROM dbo.Permissions
WHERE Code IN
(
    'FLEET.VIEW', 'CUSTOMERS.VIEW',
    'LOADS.VIEW', 'LOADS.MANAGE',
    'TRIPS.VIEW', 'TRIPS.CREATE', 'TRIPS.ASSIGN', 'TRIPS.UPDATE',
    'DISPATCH.VIEW', 'REPORTS.VIEW'
);

/* Driver receives only mobile self-service permissions. */
INSERT dbo.RolePermissions (RoleId, PermissionId)
SELECT 4, PermissionId
FROM dbo.Permissions
WHERE Code IN
(
    'DRIVER.ASSIGNMENTS.VIEW',
    'DRIVER.ASSIGNMENTS.RESPOND',
    'DRIVER.TRIP_STATUS.UPDATE',
    'DRIVER.INCIDENTS.CREATE'
);

/* Read-only receives view permissions only. */
INSERT dbo.RolePermissions (RoleId, PermissionId)
SELECT 5, PermissionId
FROM dbo.Permissions
WHERE Code IN
(
    'FLEET.VIEW', 'CUSTOMERS.VIEW', 'LOADS.VIEW',
    'TRIPS.VIEW', 'DISPATCH.VIEW', 'REPORTS.VIEW'
);

COMMIT TRANSACTION;
GO

/* Verify the role-permission matrix. */
SELECT
    r.Code AS RoleCode,
    p.Module,
    p.Code AS PermissionCode
FROM dbo.RolePermissions AS rp
JOIN dbo.Roles AS r ON r.RoleId = rp.RoleId
JOIN dbo.Permissions AS p ON p.PermissionId = rp.PermissionId
ORDER BY r.RoleId, p.Module, p.PermissionId;
GO
