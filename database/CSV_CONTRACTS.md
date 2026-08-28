# FleetFlow CSV import contracts

These files are intentionally small and fictional. They are test fixtures for
the future WinForms import screen, not direct `BULK INSERT` scripts. The C#
application owns parsing, validation, key resolution, authorization, and the
transaction.

## Import order

1. `customers.csv`
2. `locations.csv`
3. `drivers.csv`
4. `vehicles.csv`
5. `trailers.csv`
6. `loads.csv`
7. `trips.csv`
8. `trip_stops.csv`
9. `route_points.csv`

## General rules

- Encoding: UTF-8 with a header row.
- Delimiter: comma; quoted fields follow RFC 4180 conventions.
- Dates and timestamps: ISO 8601; operational timestamps are UTC and end in
  `Z`.
- Decimal separator: period. Do not use thousands separators.
- Booleans: `true` or `false`.
- Empty optional values are `NULL`; whitespace-only keys are invalid.
- Business keys are case-insensitive after trimming and normalization.
- The import service calculates SHA-256 from the original file bytes.
- Each file produces one `ImportBatches` record with `DataOriginId = 2`.
- Do not accept database identity keys, status IDs, user IDs, or audit dates
  from a CSV file.

## Key resolution

| CSV value | Database lookup |
| --- | --- |
| `CustomerNumber` | `Customers.CustomerNumber` |
| `LocationCode` | `Locations.LocationCode` |
| `DriverNumber` | `Drivers.DriverNumber` |
| `UnitNumber` | `Vehicles.UnitNumber` or `Trailers.UnitNumber` |
| `LoadNumber` | `Loads.LoadNumber` |
| `TripNumber` | `Trips.TripNumber` |
| Status/type code | Corresponding reference table `Code` |

## Transaction and error behavior

The application first parses every row into a typed DTO, validates field
lengths and ranges, and resolves referenced business keys. A file with valid
and invalid rows may be imported partially only after the user confirms it.
Accepted rows and the final batch counters are committed in one SQL transaction.
Rejected rows are written to `ImportBatchErrors` with row number, column,
stable error code, safe raw value, message, and optional JSON representation.

Use idempotency checks before import: the tuple of SHA-256, `EntityType`, and
the target environment should warn about accidental duplicate uploads. A
deliberate re-import must still respect unique business keys.

## Simulation use

`route_points.csv` defines the planned polyline and timing offsets. The
simulator interpolates between adjacent points, creates a `SimulationRuns`
record, and appends `VehicleTelemetry` rows with `DataOriginId = 3` and the
corresponding `SimulationRunId`. The current truck marker is the most recent
telemetry position; the route itself remains immutable.
