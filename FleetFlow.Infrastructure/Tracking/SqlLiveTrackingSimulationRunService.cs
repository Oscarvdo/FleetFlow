using System.Data;
using System.Data.Common;
using System.Text.Json;
using Dapper;
using FleetFlow.Application.Abstractions.Persistence;
using FleetFlow.Application.Abstractions.Tracking;
using FleetFlow.Application.Tracking;

namespace FleetFlow.Infrastructure.Tracking;

public sealed class SqlLiveTrackingSimulationRunService
    : ILiveTrackingSimulationRunService
{
    private static readonly HashSet<string> SupportedStatuses =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "RUNNING",
            "PAUSED",
            "COMPLETED",
            "FAILED",
            "CANCELLED"
        };

    private readonly IDbConnectionFactory _connectionFactory;

    public SqlLiveTrackingSimulationRunService(
        IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<SimulationRunCommandResult> CreateAsync(
        CreateSimulationRunRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ValidateRequest(request);

        string? configurationJson =
            NormalizeConfigurationJson(
                request.ConfigurationJson);

        await using DbConnection connection =
            _connectionFactory.CreateConnection();

        SimulationRunCommandResult result =
            await connection.QuerySingleAsync<
                SimulationRunCommandResult>(
                new CommandDefinition(
                    commandText:
                        "simulation.Run_Create",
                    parameters: new
                    {
                        Name = request.Name.Trim(),
                        ScenarioCode =
                            request.ScenarioCode
                                .Trim()
                                .ToUpperInvariant(),
                        RandomSeed = request.RandomSeed,
                        TimeScale = request.TimeScale,
                        UpdateIntervalMilliseconds =
                            request.UpdateIntervalMilliseconds,
                        PlannedVehicleCount =
                            request.PlannedVehicleCount,
                        ConfigurationJson =
                            configurationJson,
                        CreatedByAppUserId =
                            request.CreatedByAppUserId
                    },
                    commandType:
                        CommandType.StoredProcedure,
                    cancellationToken:
                        cancellationToken));

        return result;
    }

    public async Task<LiveTrackingSimulationRun> SetStatusAsync(
        long simulationRunId,
        string status,
        CancellationToken cancellationToken = default)
    {
        if (simulationRunId <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(simulationRunId),
                simulationRunId,
                "SimulationRunId must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(status))
        {
            throw new ArgumentException(
                "Simulation status is required.",
                nameof(status));
        }

        string normalizedStatus =
            status.Trim().ToUpperInvariant();

        if (!SupportedStatuses.Contains(normalizedStatus))
        {
            throw new ArgumentException(
                $"Unsupported simulation status: {status}.",
                nameof(status));
        }

        await using DbConnection connection =
            _connectionFactory.CreateConnection();

        LiveTrackingSimulationRun result =
            await connection.QuerySingleAsync<
                LiveTrackingSimulationRun>(
                new CommandDefinition(
                    commandText:
                        "simulation.Run_SetStatus",
                    parameters: new
                    {
                        SimulationRunId = simulationRunId,
                        Status = normalizedStatus
                    },
                    commandType:
                        CommandType.StoredProcedure,
                    cancellationToken:
                        cancellationToken));

        return result;
    }

    private static void ValidateRequest(
        CreateSimulationRunRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException(
                "Simulation name is required.",
                nameof(request));
        }

        if (request.Name.Trim().Length > 120)
        {
            throw new ArgumentException(
                "Simulation name cannot exceed 120 characters.",
                nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.ScenarioCode))
        {
            throw new ArgumentException(
                "Scenario code is required.",
                nameof(request));
        }

        if (request.ScenarioCode.Trim().Length > 40)
        {
            throw new ArgumentException(
                "Scenario code cannot exceed 40 characters.",
                nameof(request));
        }

        if (request.TimeScale <= 0M ||
            request.TimeScale > 3600M)
        {
            throw new ArgumentOutOfRangeException(
                nameof(request),
                "TimeScale must be greater than zero and no greater than 3600.");
        }

        if (request.UpdateIntervalMilliseconds < 100 ||
            request.UpdateIntervalMilliseconds > 600000)
        {
            throw new ArgumentOutOfRangeException(
                nameof(request),
                "Update interval must be between 100 and 600000 milliseconds.");
        }

        if (request.PlannedVehicleCount < 1 ||
            request.PlannedVehicleCount > 10000)
        {
            throw new ArgumentOutOfRangeException(
                nameof(request),
                "Planned vehicle count must be between 1 and 10000.");
        }

        if (request.CreatedByAppUserId.HasValue &&
            request.CreatedByAppUserId.Value <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(request),
                "CreatedByAppUserId must be greater than zero when provided.");
        }
    }

    private static string? NormalizeConfigurationJson(
        string? configurationJson)
    {
        if (string.IsNullOrWhiteSpace(configurationJson))
        {
            return null;
        }

        using JsonDocument document =
            JsonDocument.Parse(configurationJson);

        return JsonSerializer.Serialize(
            document.RootElement);
    }
}