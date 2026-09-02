using System.Data;
using System.Data.Common;
using Dapper;
using FleetFlow.Application.Abstractions.Fleet;
using FleetFlow.Application.Abstractions.Persistence;
using FleetFlow.Application.Fleet;

namespace FleetFlow.Infrastructure.Fleet;

public sealed class SqlFleetOverviewService : IFleetOverviewService
{
    private readonly IDbConnectionFactory _connectionFactory;

    public SqlFleetOverviewService(IDbConnectionFactory connectionFactory) =>
        _connectionFactory = connectionFactory;

    public async Task<FleetOverviewResult> GetOverviewAsync(
        bool includeInactive = false,
        CancellationToken cancellationToken = default)
    {
        await using DbConnection connection = _connectionFactory.CreateConnection();
        CommandDefinition command = new(
            "catalog.Fleet_GetOverview",
            new { IncludeInactive = includeInactive },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        using SqlMapper.GridReader reader = await connection.QueryMultipleAsync(command);
        return new FleetOverviewResult
        {
            Summary = await reader.ReadSingleAsync<FleetOverviewSummary>(),
            Vehicles = (await reader.ReadAsync<FleetOverviewVehicleItem>()).AsList(),
            Trailers = (await reader.ReadAsync<FleetOverviewTrailerItem>()).AsList(),
            Drivers = (await reader.ReadAsync<FleetOverviewDriverItem>()).AsList()
        };
    }
}
