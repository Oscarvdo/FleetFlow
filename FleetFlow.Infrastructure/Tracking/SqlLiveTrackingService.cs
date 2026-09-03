using System.Data;
using System.Data.Common;
using Dapper;
using FleetFlow.Application.Abstractions.Persistence;
using FleetFlow.Application.Abstractions.Tracking;
using FleetFlow.Application.Tracking;

namespace FleetFlow.Infrastructure.Tracking;

public sealed class SqlLiveTrackingService
    : ILiveTrackingService
{
    private readonly IDbConnectionFactory _connectionFactory;

    public SqlLiveTrackingService(
        IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<LiveTrackingVehicleItem>>
        GetMapStateAsync(
            bool includeOffline = true,
            int offlineAfterSeconds = 60,
            long? simulationRunId = null,
            CancellationToken cancellationToken = default)
    {
        if (offlineAfterSeconds < 5)
        {
            throw new ArgumentOutOfRangeException(
                nameof(offlineAfterSeconds),
                offlineAfterSeconds,
                "The offline threshold must be at least five seconds.");
        }

        await using DbConnection connection =
            _connectionFactory.CreateConnection();

        IEnumerable<LiveTrackingVehicleItem> records =
            await connection.QueryAsync<LiveTrackingVehicleItem>(
                new CommandDefinition(
                    commandText:
                        "tracking.LiveTracking_GetMapState",
                    parameters: new
                    {
                        IncludeOffline = includeOffline,
                        OfflineAfterSeconds = offlineAfterSeconds,
                        SimulationRunId = simulationRunId
                    },
                    commandType:
                        CommandType.StoredProcedure,
                    cancellationToken:
                        cancellationToken));

        return records.AsList();
    }

    public async Task<IReadOnlyList<LiveTrackingRoutePoint>>
        GetTripRouteAsync(
            long tripId,
            CancellationToken cancellationToken = default)
    {
        if (tripId <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(tripId),
                tripId,
                "TripId must be greater than zero.");
        }

        await using DbConnection connection =
            _connectionFactory.CreateConnection();

        IEnumerable<LiveTrackingRoutePoint> records =
            await connection.QueryAsync<LiveTrackingRoutePoint>(
                new CommandDefinition(
                    commandText:
                        "tracking.LiveTracking_GetTripRoute",
                    parameters: new
                    {
                        TripId = tripId
                    },
                    commandType:
                        CommandType.StoredProcedure,
                    cancellationToken:
                        cancellationToken));

        return records.AsList();
    }

    public async Task<IReadOnlyList<LiveTrackingStopItem>>
        GetTripStopsAsync(
            long tripId,
            CancellationToken cancellationToken = default)
    {
        if (tripId <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(tripId),
                tripId,
                "TripId must be greater than zero.");
        }

        await using DbConnection connection =
            _connectionFactory.CreateConnection();

        IEnumerable<LiveTrackingStopItem> records =
            await connection.QueryAsync<LiveTrackingStopItem>(
                new CommandDefinition(
                    commandText:
                        "tracking.LiveTracking_GetTripStops",
                    parameters: new
                    {
                        TripId = tripId
                    },
                    commandType:
                        CommandType.StoredProcedure,
                    cancellationToken:
                        cancellationToken));

        return records.AsList();
    }

    public async Task<
        IReadOnlyList<LiveTrackingSimulationCandidate>>
        GetSimulationCandidatesAsync(
            CancellationToken cancellationToken = default)
    {
        await using DbConnection connection =
            _connectionFactory.CreateConnection();

        IEnumerable<LiveTrackingSimulationCandidate> records =
            await connection.QueryAsync<
                LiveTrackingSimulationCandidate>(
                new CommandDefinition(
                    commandText:
                        "simulation.LiveTracking_GetCandidates",
                    commandType:
                        CommandType.StoredProcedure,
                    cancellationToken:
                        cancellationToken));

        return records.AsList();
    }

    public async Task<IReadOnlyList<LiveTrackingSimulationRun>>
        GetSimulationRunsAsync(
            bool includeCompleted = false,
            CancellationToken cancellationToken = default)
    {
        await using DbConnection connection =
            _connectionFactory.CreateConnection();

        IEnumerable<LiveTrackingSimulationRun> records =
            await connection.QueryAsync<LiveTrackingSimulationRun>(
                new CommandDefinition(
                    commandText:
                        "simulation.LiveTracking_GetRuns",
                    parameters: new
                    {
                        IncludeCompleted = includeCompleted
                    },
                    commandType:
                        CommandType.StoredProcedure,
                    cancellationToken:
                        cancellationToken));

        return records.AsList();
    }
}