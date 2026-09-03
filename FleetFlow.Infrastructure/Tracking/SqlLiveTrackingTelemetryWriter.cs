using System.Data;
using System.Data.Common;
using Dapper;
using FleetFlow.Application.Abstractions.Persistence;
using FleetFlow.Application.Abstractions.Tracking;
using FleetFlow.Application.Tracking;

namespace FleetFlow.Infrastructure.Tracking;

public sealed class SqlLiveTrackingTelemetryWriter
    : ILiveTrackingTelemetryWriter
{
    private readonly IDbConnectionFactory _connectionFactory;

    public SqlLiveTrackingTelemetryWriter(
        IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<TelemetryBatchResult> AppendBatchAsync(
        IReadOnlyCollection<VehicleTelemetryUpdate> telemetry,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(telemetry);

        if (telemetry.Count == 0)
        {
            return new TelemetryBatchResult
            {
                InsertedRows = 0,
                SubmittedRows = 0
            };
        }

        ValidateTelemetry(telemetry);

        DataTable table = CreateTelemetryTable();
        int clientRowId = 1;

        foreach (VehicleTelemetryUpdate update in telemetry)
        {
            DataRow row = table.NewRow();

            row["ClientRowId"] = clientRowId;
            row["TelemetryId"] = update.TelemetryId;
            row["VehicleId"] = update.VehicleId;
            row["TripId"] = DatabaseValue(update.TripId);
            row["RecordedAtUtc"] = EnsureUtc(update.RecordedAtUtc);
            row["SequenceNumber"] =
                DatabaseValue(update.SequenceNumber);
            row["Latitude"] = update.Latitude;
            row["Longitude"] = update.Longitude;
            row["SpeedMph"] = DatabaseValue(update.SpeedMph);
            row["FuelPercent"] =
                DatabaseValue(update.FuelPercent);
            row["OdometerMiles"] =
                DatabaseValue(update.OdometerMiles);
            row["HeadingDegrees"] =
                DatabaseValue(update.HeadingDegrees);
            row["DataOriginId"] = update.DataOriginId;
            row["ImportBatchId"] =
                DatabaseValue(update.ImportBatchId);
            row["SimulationRunId"] =
                DatabaseValue(update.SimulationRunId);
            row["PayloadJson"] =
                DatabaseValue(update.PayloadJson);

            table.Rows.Add(row);
            clientRowId++;
        }

        DynamicParameters parameters = new();

        parameters.Add(
            "Telemetry",
            table.AsTableValuedParameter(
                "tracking.VehicleTelemetryTableType"));

        await using DbConnection connection =
            _connectionFactory.CreateConnection();

        TelemetryBatchResult result =
            await connection.QuerySingleAsync<TelemetryBatchResult>(
                new CommandDefinition(
                    commandText:
                        "tracking.VehicleTelemetry_AppendBatch",
                    parameters:
                        parameters,
                    commandType:
                        CommandType.StoredProcedure,
                    cancellationToken:
                        cancellationToken));

        return result;
    }

    private static DataTable CreateTelemetryTable()
    {
        DataTable table = new();

        table.Columns.Add(
            "ClientRowId",
            typeof(int));

        table.Columns.Add(
            "TelemetryId",
            typeof(Guid));

        table.Columns.Add(
            "VehicleId",
            typeof(long));

        AddNullableColumn(
            table,
            "TripId",
            typeof(long));

        table.Columns.Add(
            "RecordedAtUtc",
            typeof(DateTime));

        AddNullableColumn(
            table,
            "SequenceNumber",
            typeof(long));

        table.Columns.Add(
            "Latitude",
            typeof(decimal));

        table.Columns.Add(
            "Longitude",
            typeof(decimal));

        AddNullableColumn(
            table,
            "SpeedMph",
            typeof(decimal));

        AddNullableColumn(
            table,
            "FuelPercent",
            typeof(decimal));

        AddNullableColumn(
            table,
            "OdometerMiles",
            typeof(decimal));

        AddNullableColumn(
            table,
            "HeadingDegrees",
            typeof(decimal));

        table.Columns.Add(
            "DataOriginId",
            typeof(byte));

        AddNullableColumn(
            table,
            "ImportBatchId",
            typeof(long));

        AddNullableColumn(
            table,
            "SimulationRunId",
            typeof(long));

        AddNullableColumn(
            table,
            "PayloadJson",
            typeof(string));

        return table;
    }

    private static void AddNullableColumn(
        DataTable table,
        string columnName,
        Type dataType)
    {
        DataColumn column = table.Columns.Add(
            columnName,
            dataType);

        column.AllowDBNull = true;
    }

    private static object DatabaseValue<T>(T? value)
        where T : struct
    {
        return value.HasValue
            ? value.Value
            : DBNull.Value;
    }

    private static object DatabaseValue(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? DBNull.Value
            : value;
    }

    private static DateTime EnsureUtc(DateTime value)
    {
        if (value == default)
        {
            return DateTime.UtcNow;
        }

        return value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(
                value,
                DateTimeKind.Utc)
        };
    }

    private static void ValidateTelemetry(
        IReadOnlyCollection<VehicleTelemetryUpdate> telemetry)
    {
        HashSet<Guid> telemetryIds = new();

        foreach (VehicleTelemetryUpdate update in telemetry)
        {
            if (update.VehicleId <= 0)
            {
                throw new ArgumentException(
                    "Every telemetry update requires a valid VehicleId.",
                    nameof(telemetry));
            }

            if (update.TripId.HasValue &&
                update.TripId.Value <= 0)
            {
                throw new ArgumentException(
                    "TripId must be greater than zero when provided.",
                    nameof(telemetry));
            }

            if (update.Latitude is < -90M or > 90M)
            {
                throw new ArgumentException(
                    "Telemetry latitude must be between -90 and 90.",
                    nameof(telemetry));
            }

            if (update.Longitude is < -180M or > 180M)
            {
                throw new ArgumentException(
                    "Telemetry longitude must be between -180 and 180.",
                    nameof(telemetry));
            }

            if (update.SpeedMph is < 0M or > 120M)
            {
                throw new ArgumentException(
                    "Telemetry speed must be between 0 and 120 MPH.",
                    nameof(telemetry));
            }

            if (update.FuelPercent is < 0M or > 100M)
            {
                throw new ArgumentException(
                    "Fuel percentage must be between 0 and 100.",
                    nameof(telemetry));
            }

            if (update.OdometerMiles < 0M)
            {
                throw new ArgumentException(
                    "Odometer mileage cannot be negative.",
                    nameof(telemetry));
            }

            if (update.HeadingDegrees is < 0M or > 360M)
            {
                throw new ArgumentException(
                    "Heading must be between 0 and 360 degrees.",
                    nameof(telemetry));
            }

            if (update.DataOriginId == 3 &&
                !update.SimulationRunId.HasValue)
            {
                throw new ArgumentException(
                    "Simulated telemetry requires a SimulationRunId.",
                    nameof(telemetry));
            }

            if (!telemetryIds.Add(update.TelemetryId))
            {
                throw new ArgumentException(
                    "The telemetry batch contains duplicate TelemetryId values.",
                    nameof(telemetry));
            }
        }
    }
}