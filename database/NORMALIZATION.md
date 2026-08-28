# FleetFlow normalization notes

The MVP schema targets third normal form (3NF). Normalization is applied to
operational master data and transactional relationships before the WinForms or
mobile interfaces are built.

## First normal form (1NF)

- Every table has a primary key.
- Every column stores one value; there are no comma-separated lists.
- Trip stops are separate rows identified by `(TripId, StopSequence)`.
- Statuses, assignments, and events are not embedded as repeating columns on a
  trip.

## Second normal form (2NF)

- Non-key attributes describe the complete key of their table.
- A stop's schedule and completion timestamps belong to `TripStops`, not to
  locations.
- A driver's response belongs to `TripAssignments`, not to `Drivers` or
  `Trips`.
- Event measurements belong to `TripEvents`, keyed by the individual event.

## Third normal form (3NF)

- Status descriptions and behavior flags are kept in reference tables.
- Customer identity is stored in `Customers`; reusable physical addresses are
  stored in `Locations`.
- `TripStops` references `Locations` instead of repeating the location name,
  address, coordinates, and contact information.
- Loads reference customers, trips reference loads, and assignments connect a
  trip to the driver, truck, and optional trailer.
- Trip history references the old and new status records rather than copying
  their display names.
- Users, roles, and permissions use separate entities with normalized
  many-to-many bridge tables: `UserRoles` and `RolePermissions`.
- CSV lineage is represented by `ImportBatches` and `ImportBatchErrors` rather
  than repeating file metadata and error text on every imported business row.
- Simulation metadata is stored once in `SimulationRuns`; telemetry and events
  reference that execution.
- A route is a normalized ordered collection of `TripRoutePoints`, not a list
  of coordinates embedded in `Trips`.

## Intentional operational fields

The following fields are deliberate and are not accidental duplication:

- `IsActive` on an assignment enables filtered unique indexes that prevent two
  active assignments for the same trip, driver, truck, or trailer.
- `UpdatedAtUtc` and `rowversion` support auditing and optimistic concurrency
  across WinForms, the API, and the MAUI application.
- `TripStatusHistory` is an immutable business audit trail, `TripEvents`
  captures domain events, and `VehicleTelemetry` isolates high-volume
  time-series measurements.
- Human-readable numbers such as `TripNumber` and `UnitNumber` are alternate
  business keys; identity columns remain the relational keys.

## Historical location snapshots

The normalized MVP references the current row in `Locations`. If the product
later permits users to change a location after a trip has been completed, add
effective-dated location versions or a `TripStopAddressSnapshot` table. Do not
copy address columns back into `TripStops` without making that historical
snapshot behavior explicit.

## Deferred normalization decisions

These are intentionally postponed until their modules exist:

- refresh tokens, password resets, and trusted mobile devices;
- multiple customer contacts;
- fuel vendors and fuel transactions;
- incident categories and attachments;
- maintenance work orders;
- proof-of-delivery documents;
- invoices, driver settlements, and accounting dimensions.

This avoids designing empty tables for features that are outside the first
working dispatch workflow.
