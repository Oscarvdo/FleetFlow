using System.Data;
using System.Data.Common;
using Dapper;
using FleetFlow.Application.Abstractions.Loads;
using FleetFlow.Application.Abstractions.Persistence;
using FleetFlow.Application.Loads;

namespace FleetFlow.Infrastructure.Loads;

/// <summary>
/// Consulta los detalles de una carga desde SQL Server.
/// </summary>
public sealed class SqlLoadDetailsService
    : ILoadDetailsService
{
    private readonly IDbConnectionFactory
        _connectionFactory;

    /// <summary>
    /// Recibe la fábrica utilizada para crear conexiones
    /// hacia la base de datos FleetFlow.
    /// </summary>
    public SqlLoadDetailsService(
        IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <summary>
    /// Ejecuta operations.Load_GetDetails y convierte
    /// la fila resultante en un objeto LoadDetails.
    /// </summary>
    public async Task<LoadDetails?> GetByIdAsync(
        long loadId,
        CancellationToken cancellationToken = default)
    {
        if (loadId <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(loadId),
                "A valid LoadId is required.");
        }

        await using DbConnection connection =
            _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: "operations.Load_GetDetails",
            parameters: new
            {
                LoadId = loadId
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        // SingleOrDefault permite recibir una carga
        // o null cuando el identificador no existe.
        return await connection
            .QuerySingleOrDefaultAsync<LoadDetails>(
                command);
    }
}