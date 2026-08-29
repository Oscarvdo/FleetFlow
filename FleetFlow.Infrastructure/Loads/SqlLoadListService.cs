using System.Data;
using System.Data.Common;
using Dapper;
using FleetFlow.Application.Abstractions.Loads;
using FleetFlow.Application.Abstractions.Persistence;
using FleetFlow.Application.Loads;

namespace FleetFlow.Infrastructure.Loads;

/// <summary>
/// Obtiene la lista de cargas almacenadas en SQL Server.
/// Implementa el contrato definido en la capa Application.
/// </summary>
public sealed class SqlLoadListService : ILoadListService
{
    private readonly IDbConnectionFactory _connectionFactory;

    /// <summary>
    /// Recibe la fábrica encargada de crear conexiones
    /// hacia la base de datos FleetFlow.
    /// </summary>
    public SqlLoadListService(
        IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <summary>
    /// Consulta las cargas y permite enviar filtros opcionales
    /// por estado y texto de búsqueda.
    /// </summary>
    public async Task<IReadOnlyList<LoadListItem>> GetLoadsAsync(
        string? statusCode = null,
        string? searchText = null,
        CancellationToken cancellationToken = default)
    {
        // La conexión se elimina automáticamente al terminar
        // la operación, incluso si ocurre una excepción.
        await using DbConnection connection =
            _connectionFactory.CreateConnection();

        // CommandDefinition permite configurar el procedimiento,
        // sus parámetros, el tipo de comando y la cancelación.
        CommandDefinition command = new(
            commandText: "operations.Load_GetList",
            parameters: new
            {
                StatusCode = Normalize(statusCode),
                SearchText = Normalize(searchText)
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        // Dapper convierte cada fila devuelta por SQL Server
        // en una instancia de LoadListItem.
        IEnumerable<LoadListItem> loads =
            await connection.QueryAsync<LoadListItem>(
                command);

        return loads.AsList();
    }

    /// <summary>
    /// Convierte valores vacíos en null para que el procedimiento
    /// interprete que no debe aplicar ese filtro.
    /// </summary>
    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}