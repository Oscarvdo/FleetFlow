using System.Data;
using System.Data.Common;
using Dapper;
using FleetFlow.Application.Abstractions.Loads;
using FleetFlow.Application.Abstractions.Persistence;
using FleetFlow.Application.Loads;
using Microsoft.Data.SqlClient;

namespace FleetFlow.Infrastructure.Loads;

/// <summary>
/// Ejecuta las operaciones que crean o actualizan
/// cargas dentro de SQL Server.
/// </summary>
public sealed class SqlLoadCommandService
    : ILoadCommandService
{
    private readonly IDbConnectionFactory
        _connectionFactory;

    public SqlLoadCommandService(
        IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <summary>
    /// Valida y crea una carga mediante
    /// operations.Load_Create.
    /// </summary>
    public async Task<CreateLoadResult> CreateAsync(
        CreateLoadRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        ValidateCommonFields(
            request.LoadNumber,
            request.CustomerId,
            request.Description,
            request.Commodity,
            request.WeightLbs,
            request.Pieces,
            request.RevenueAmount,
            request.SpecialInstructions);

        await using DbConnection connection =
            _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: "operations.Load_Create",
            parameters: new
            {
                LoadNumber =
                    request.LoadNumber.Trim(),

                CustomerId =
                    request.CustomerId,

                Description =
                    request.Description.Trim(),

                Commodity =
                    Normalize(request.Commodity),

                WeightLbs =
                    request.WeightLbs,

                Pieces =
                    request.Pieces,

                RevenueAmount =
                    request.RevenueAmount,

                SpecialInstructions =
                    Normalize(
                        request.SpecialInstructions),

                // Una carga nueva siempre comienza
                // con el estado operacional NEW.
                LoadStatusCode =
                    "NEW",

                SourceImportBatchId =
                    (long?)null
            },
            commandType:
                CommandType.StoredProcedure,
            cancellationToken:
                cancellationToken);

        try
        {
            return await connection
                .QuerySingleAsync<CreateLoadResult>(
                    command);
        }
        catch (SqlException exception)
            when (exception.Number is 2601 or 2627)
        {
            throw new InvalidOperationException(
                $"A load with number " +
                $"'{request.LoadNumber.Trim()}' " +
                "already exists.",
                exception);
        }
    }

    /// <summary>
    /// Valida y actualiza una carga mediante
    /// operations.Load_Update.
    /// </summary>
    public async Task<UpdateLoadResult> UpdateAsync(
        UpdateLoadRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.LoadId <= 0)
        {
            throw new ArgumentException(
                "A valid load identifier is required.",
                nameof(request));
        }

        ValidateCommonFields(
            request.LoadNumber,
            request.CustomerId,
            request.Description,
            request.Commodity,
            request.WeightLbs,
            request.Pieces,
            request.RevenueAmount,
            request.SpecialInstructions);

        if (request.ExpectedRowVersion is null ||
            request.ExpectedRowVersion.Length != 8)
        {
            throw new ArgumentException(
                "A valid load RowVersion is required.",
                nameof(request));
        }

        await using DbConnection connection =
            _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: "operations.Load_Update",
            parameters: new
            {
                LoadId =
                    request.LoadId,

                LoadNumber =
                    request.LoadNumber.Trim(),

                CustomerId =
                    request.CustomerId,

                Description =
                    request.Description.Trim(),

                Commodity =
                    Normalize(request.Commodity),

                WeightLbs =
                    request.WeightLbs,

                Pieces =
                    request.Pieces,

                RevenueAmount =
                    request.RevenueAmount,

                SpecialInstructions =
                    Normalize(
                        request.SpecialInstructions),

                ExpectedRowVersion =
                    request.ExpectedRowVersion
            },
            commandType:
                CommandType.StoredProcedure,
            cancellationToken:
                cancellationToken);

        try
        {
            return await connection
                .QuerySingleAsync<UpdateLoadResult>(
                    command);
        }
        catch (SqlException exception)
            when (exception.Number is 2601 or 2627)
        {
            throw new InvalidOperationException(
                $"A load with number " +
                $"'{request.LoadNumber.Trim()}' " +
                "already exists.",
                exception);
        }
        catch (SqlException exception)
            when (exception.Number == 51030)
        {
            // RowVersion diferente o registro eliminado.
            throw new InvalidOperationException(
                "This load was changed by another user " +
                "or no longer exists. Refresh the load " +
                "and try again.",
                exception);
        }
        catch (SqlException exception)
            when (exception.Number == 51037)
        {
            throw new InvalidOperationException(
                "Only new or planned loads can be edited.",
                exception);
        }
        catch (SqlException exception)
            when (exception.Number == 51038)
        {
            throw new InvalidOperationException(
                "This load cannot be edited because its " +
                "trip has already started.",
                exception);
        }
        catch (SqlException exception)
            when (exception.Number == 51039)
        {
            throw new InvalidOperationException(
                "Load number and customer cannot be " +
                "changed after a trip is assigned.",
                exception);
        }
        catch (SqlException exception)
            when (exception.Number == 51040)
        {
            throw new InvalidOperationException(
                "The selected customer does not exist " +
                "or is inactive.",
                exception);
        }
        catch (SqlException exception)
            when (exception.Number is >= 51031 and <= 51036)
        {
            // Estas reglas también se validan en Application,
            // pero SQL Server conserva la última protección.
            throw new InvalidOperationException(
                exception.Message,
                exception);
        }
    }

    /// <summary>
    /// Aplica las reglas compartidas por creación
    /// y actualización.
    /// </summary>
    private static void ValidateCommonFields(
        string loadNumber,
        long customerId,
        string description,
        string? commodity,
        decimal weightLbs,
        int? pieces,
        decimal? revenueAmount,
        string? specialInstructions)
    {
        if (string.IsNullOrWhiteSpace(loadNumber))
        {
            throw new ArgumentException(
                "Load number is required.");
        }

        if (loadNumber.Trim().Length > 30)
        {
            throw new ArgumentException(
                "Load number cannot exceed 30 characters.");
        }

        if (customerId <= 0)
        {
            throw new ArgumentException(
                "A customer is required.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException(
                "Description is required.");
        }

        if (description.Trim().Length > 300)
        {
            throw new ArgumentException(
                "Description cannot exceed 300 characters.");
        }

        if (Normalize(commodity)?.Length > 100)
        {
            throw new ArgumentException(
                "Commodity cannot exceed 100 characters.");
        }

        if (weightLbs <= 0)
        {
            throw new ArgumentException(
                "Weight must be greater than zero.");
        }

        if (pieces.HasValue &&
            pieces.Value <= 0)
        {
            throw new ArgumentException(
                "Pieces must be greater than zero.");
        }

        if (revenueAmount.HasValue &&
            revenueAmount.Value < 0)
        {
            throw new ArgumentException(
                "Revenue cannot be negative.");
        }

        if (Normalize(specialInstructions)?.Length > 1000)
        {
            throw new ArgumentException(
                "Special instructions cannot exceed " +
                "1000 characters.");
        }
    }

    /// <summary>
    /// Convierte texto vacío en null antes
    /// de enviarlo a SQL Server.
    /// </summary>
    private static string? Normalize(
        string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}