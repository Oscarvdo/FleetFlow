# FleetFlow analytics and data-science architecture

FleetFlow uses separate models for transactional processing and analytics. The
WinForms, API, and MAUI applications write to the normalized `FleetFlowDb`
OLTP database. Reporting and machine-learning workloads will read from a
separate star-schema Data Mart populated incrementally.

## Why the models stay separate

- Dispatch requires short, predictable transactions and relational integrity.
- Telemetry ingestion requires append-oriented, high-volume writes.
- Power BI and data science require wide, scan-friendly fact tables.
- Training models must not lock or slow the operational application.
- Historical dimensions must preserve what was true when a trip occurred.

## Operational sources

| OLTP source | Analytical use |
| --- | --- |
| `Trips` | trip duration, distance, completion, and delay measures |
| `TripStops` | dwell time, on-time pickup, and on-time delivery |
| `TripAssignments` | driver and equipment utilization |
| `TripStatusHistory` | lifecycle and transition-duration analysis |
| `TripEvents` | incidents, delays, breakdowns, and connectivity events |
| `VehicleTelemetry` | route, speed, fuel, distance, and communication quality |
| `SimulationRuns` | scenario, seed, speed, and synthetic-data segmentation |
| `ImportBatches` | CSV lineage, quality, rejection, and completeness metrics |

## Planned Data Mart

```text
FleetFlowAnalytics
├── Dimension.Date
├── Dimension.Time
├── Dimension.Driver
├── Dimension.Vehicle
├── Dimension.Trailer
├── Dimension.Customer
├── Dimension.Location
├── Dimension.TripStatus
├── Fact.Trip
├── Fact.TripStop
├── Fact.Assignment
├── Fact.TripEvent
└── Fact.VehicleTelemetry
```

Dimensions will use analytical surrogate keys and effective-date columns when
history becomes necessary. Operational identity keys remain as durable source
references.

## Candidate KPIs

- on-time pickup and delivery percentage;
- average trip and dwell duration;
- planned versus actual distance;
- truck, trailer, and driver utilization;
- empty or idle time;
- delay and breakdown frequency;
- communication-loss duration;
- fuel consumed per mile;
- route deviation;
- customer service level;
- acceptance and rejection rates for mobile trip offers.

## Data-science path

Potential models are introduced only after sufficient validated data exists:

1. estimated time of arrival and delay risk;
2. fuel-consumption anomaly detection;
3. trip-duration prediction;
4. driver or vehicle utilization forecasting;
5. breakdown-risk indicators;
6. route and stop clustering.

Training datasets will include a reproducible extraction date, feature version,
target definition, and train/validation/test split. Predictions will be stored
separately from observed outcomes so model output is never confused with fact.

## Ingestion and retention

- Every timestamp is UTC.
- `OccurredAtUtc` or `RecordedAtUtc` represents source time.
- `ReceivedAtUtc` measures ingestion latency.
- Source clocks can drift, so the OLTP does not reject an event merely because
  source time is later than server receipt time; analytics flags clock-quality
  anomalies instead.
- GUID event identifiers provide idempotency across retries.
- `DataOriginId`, `ImportBatchId`, and `SimulationRunId` separate operational,
  imported, and synthetic observations.
- Optional device sequence numbers reveal gaps or duplicates.
- JSON payloads retain source-specific attributes without weakening the typed
  relational columns used by the application.
- Telemetry retention, aggregation, partitioning, and columnstore indexes will
  be added after actual volume is measured; they are not required for the MVP.

## .NET implementation direction

- EF Core handles transactional entities and migrations.
- `BackgroundService` and `Channel<T>` handle simulator/event ingestion.
- Bulk telemetry ingestion will use batched writes instead of one transaction
  per point.
- An analytics worker will use incremental watermarks and idempotent loads.
- Python notebooks or ML.NET can consume curated Data Mart extracts.
- Power BI connects to analytical views or the Data Mart, not directly to
  high-frequency operational queries.
