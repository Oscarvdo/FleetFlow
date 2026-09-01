using System.Data;
using System.Data.Common;
using Dapper;
using FleetFlow.Application.Abstractions.Customers;
using FleetFlow.Application.Abstractions.Persistence;
using FleetFlow.Application.Customers;

namespace FleetFlow.Infrastructure.Customers;

/// <summary>
/// Obtiene clientes desde SQL Server para utilizarlos
/// en controles de selección.
/// </summary>
public sealed class SqlCustomerLookupService
    : ICustomerLookupService
{
    private readonly IDbConnectionFactory
        _connectionFactory;

    public SqlCustomerLookupService(
        IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <summary>
    /// Ejecuta catalog.Customer_Search.
    /// Puede devolver únicamente clientes activos
    /// o incluir también los inactivos.
    /// </summary>
    public async Task<IReadOnlyList<CustomerLookupItem>>
        SearchAsync(
            string? searchText = null,
            bool includeInactive = false,
            CancellationToken cancellationToken = default)
    {
        await using DbConnection connection =
            _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: "catalog.Customer_Search",
            parameters: new
            {
                SearchText =
                    Normalize(searchText),

                IncludeInactive =
                    includeInactive
            },
            commandType:
                CommandType.StoredProcedure,
            cancellationToken:
                cancellationToken);

        IEnumerable<CustomerLookupItem> customers =
            await connection
                .QueryAsync<CustomerLookupItem>(
                    command);

        return customers.ToList();
    }

    /// <summary>
    /// Convierte una búsqueda vacía en null para que
    /// SQL Server devuelva todos los clientes permitidos.
    /// </summary>
    private static string? Normalize(
        string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}