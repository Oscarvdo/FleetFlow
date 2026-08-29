using System.Data;
using System.Data.Common;
using Dapper;
using FleetFlow.Application.Abstractions.Dispatch;
using FleetFlow.Application.Abstractions.Persistence;
using FleetFlow.Application.Dispatch;

namespace FleetFlow.Infrastructure.Dispatch;

public sealed class SqlDispatchBoardService
    : IDispatchBoardService
{
    private readonly IDbConnectionFactory _connectionFactory;

    public SqlDispatchBoardService(
        IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<DispatchBoardItem>>
        GetActiveTripsAsync(
            CancellationToken cancellationToken = default)
    {
        await using DbConnection connection =
            _connectionFactory.CreateConnection();

        IEnumerable<DispatchBoardItem> records =
            await connection.QueryAsync<DispatchBoardItem>(
                new CommandDefinition(
                    commandText:
                        "dispatch.DispatchBoard_GetActive",
                    commandType:
                        CommandType.StoredProcedure,
                    cancellationToken:
                        cancellationToken));

        return records.AsList();
    }
}