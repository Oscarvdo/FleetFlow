using System.Data;
using System.Data.Common;
using Dapper;
using FleetFlow.Application.Abstractions.Persistence;
using FleetFlow.Application.Abstractions.Trips;
using FleetFlow.Application.Trips;

namespace FleetFlow.Infrastructure.Trips;

public sealed class SqlTripListService : ITripListService
{
    private readonly IDbConnectionFactory _connectionFactory;

    public SqlTripListService(
        IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<TripListItem>> GetTripsAsync(
        string? statusCode = null,
        string? searchText = null,
        CancellationToken cancellationToken = default)
    {
        await using DbConnection connection =
            _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: "operations.Trip_GetList",
            parameters: new
            {
                StatusCode = Normalize(statusCode),
                SearchText = Normalize(searchText)
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        IEnumerable<TripListItem> trips =
            await connection.QueryAsync<TripListItem>(command);

        return trips.AsList();
    }

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}