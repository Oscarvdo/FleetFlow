# FleetFlow database API version 1

FleetFlow uses a stored-procedure-first write model. WinForms, ASP.NET Core,
the simulator, and the future MAUI application do not issue ad-hoc writes to
operational tables. The Infrastructure project calls procedures and maps views
to read-only DTOs.

## Boundaries

- `dbo` owns normalized tables and reference data.
- `catalog` owns customer, location, driver, vehicle, and trailer commands.
- `operations` owns loads, trips, stops, and state transitions.
- `dispatch` owns assignment commands and dispatch read models.
- `tracking` owns routes, events, current positions, and telemetry ingestion.
- `import` owns CSV batch lifecycle and row errors.
- `simulation` owns reproducible simulation runs.
- `security` owns application accounts, authorization, and audit access.
- `reporting` owns read-only operational KPIs.

## Design rules

1. Approximately 90 percent of writes should use stored procedures.
2. Every multi-table business action is one explicit SQL transaction.
3. `rowversion` is passed back on edits to detect concurrent changes.
4. Status codes cross the application boundary; numeric status keys do not.
5. Events and telemetry use client-generated GUIDs for idempotent retries.
6. Telemetry uses a table-valued parameter and updates `VehicleCurrentState`
   in the same transaction.
7. Authentication password hashing and verification remain in .NET.
8. Triggers protect append-only history; they do not orchestrate workflows.
9. Views are read models and contain no presentation-specific formatting.
10. The analytics Data Mart remains separate from the OLTP database.

## .NET direction

EF Core maps normalized entities where useful, keyless DTOs for views, and
calls procedures through parameterized commands. The WinForms project references
the Application layer, never the SQL project directly. The MAUI application
will call ASP.NET Core rather than connect to SQL Server.

The first implementation slice will use `dispatch.vw_DispatchBoard`,
`operations.vw_TripDetails`, `catalog.Customer_Search`, and the login procedures.
