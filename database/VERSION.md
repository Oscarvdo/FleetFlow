# FleetFlow database v1.0.0

This is the first portfolio-ready database contract for FleetFlow.

## Included

- normalized SQL Server OLTP model;
- application users, RBAC permissions, and security audit;
- customers, locations, drivers, vehicles, trailers, loads, trips, stops, and assignments;
- CSV lineage and row-level import errors;
- routes, reproducible simulation runs, events, historical telemetry, and current vehicle state;
- 8 domain schemas, 14 read views, 30 command procedures, 5 table-valued parameter types,
  4 inline functions, and 4 protection triggers;
- allowed trip-status transitions and initial retention policies;
- fictional Arizona demonstration and CSV data;
- OLTP, normalization, and future analytics documentation.

## Runtime verification

Static dependency, delimiter, reference, CSV, and archive checks are included in
the delivered package. Execute the numbered scripts against SQL Server LocalDB
and run `011_database_api_validation.sql` for engine-level verification.

Future modules such as fuel, incidents, maintenance, proof of delivery,
invoicing, and settlements will extend this contract in later releases.
