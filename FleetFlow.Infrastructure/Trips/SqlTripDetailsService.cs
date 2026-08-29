using System.Data;
using System.Data.Common;
using Dapper;
using FleetFlow.Application.Abstractions.Persistence;
using FleetFlow.Application.Abstractions.Trips;
using FleetFlow.Application.Trips;

namespace FleetFlow.Infrastructure.Trips;

public sealed class SqlTripDetailsService
    : ITripDetailsService
{
    private readonly IDbConnectionFactory _connectionFactory;

    public SqlTripDetailsService(
        IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<TripDetailsResult?> GetByIdAsync(
        long tripId,
        CancellationToken cancellationToken = default)
    {
        await using DbConnection connection =
            _connectionFactory.CreateConnection();

        using SqlMapper.GridReader results =
            await connection.QueryMultipleAsync(
                new CommandDefinition(
                    commandText:
                        "operations.Trip_GetDetails",
                    parameters: new
                    {
                        TripId = tripId
                    },
                    commandType:
                        CommandType.StoredProcedure,
                    cancellationToken:
                        cancellationToken));

        TripDetails? trip =
            await results.ReadSingleOrDefaultAsync<TripDetails>();

        IReadOnlyList<TripStopItem> stops =
            (
                await results.ReadAsync<TripStopItem>()
            ).AsList();

        IReadOnlyList<TripStatusTimelineItem> timeline =
            (
                await results.ReadAsync<TripStatusTimelineItem>()
            ).AsList();

        if (trip is null)
        {
            return null;
        }

        return new TripDetailsResult
        {
            Trip = trip,
            Stops = stops,
            StatusTimeline = timeline
        };
    }
}