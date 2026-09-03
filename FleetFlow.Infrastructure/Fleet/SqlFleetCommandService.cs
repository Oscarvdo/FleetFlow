using System.Data;
using System.Data.Common;
using Dapper;
using FleetFlow.Application.Abstractions.Fleet;
using FleetFlow.Application.Abstractions.Persistence;
using FleetFlow.Application.Fleet;

namespace FleetFlow.Infrastructure.Fleet;

public sealed class SqlFleetCommandService : IFleetCommandService
{
    private readonly IDbConnectionFactory _connectionFactory;
    public SqlFleetCommandService(IDbConnectionFactory connectionFactory) => _connectionFactory = connectionFactory;

    public async Task<VehicleCommandResult> SaveVehicleAsync(SaveVehicleRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (string.IsNullOrWhiteSpace(request.UnitNumber) || string.IsNullOrWhiteSpace(request.Vin) ||
            string.IsNullOrWhiteSpace(request.Make) || string.IsNullOrWhiteSpace(request.Model) ||
            string.IsNullOrWhiteSpace(request.LicensePlate) || string.IsNullOrWhiteSpace(request.LicenseState))
            throw new ArgumentException("Complete all required vehicle fields.", nameof(request));
        if (request.VehicleId.HasValue && request.ExpectedRowVersion is not { Length: 8 })
            throw new ArgumentException("A valid RowVersion is required.", nameof(request));

        await using DbConnection connection = _connectionFactory.CreateConnection();
        object parameters = request.VehicleId.HasValue ? new
        {
            VehicleId = request.VehicleId.Value, request.UnitNumber, request.Vin, request.ModelYear,
            request.Make, request.Model, request.LicensePlate, request.LicenseState,
            request.MaxPayloadLbs, request.CurrentOdometerMiles, FleetAssetStatusCode = request.StatusCode,
            ExpectedRowVersion = request.ExpectedRowVersion
        } : new
        {
            request.UnitNumber, request.Vin, request.ModelYear, request.Make, request.Model,
            request.LicensePlate, request.LicenseState, request.MaxPayloadLbs, request.CurrentOdometerMiles,
            FleetAssetStatusCode = request.StatusCode
        };
        var command = new CommandDefinition(request.VehicleId.HasValue ? "catalog.Vehicle_Update" : "catalog.Vehicle_Create", parameters, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.QuerySingleAsync<VehicleCommandResult>(command);
    }

    public async Task<VehicleCommandResult> SetVehicleActiveAsync(long vehicleId, bool isActive, byte[] expectedRowVersion, CancellationToken cancellationToken = default)
    {
        await using DbConnection connection = _connectionFactory.CreateConnection();
        var command = new CommandDefinition("catalog.Vehicle_SetActive", new { VehicleId = vehicleId, IsActive = isActive, ExpectedRowVersion = expectedRowVersion }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.QuerySingleAsync<VehicleCommandResult>(command);
    }

    public async Task<TrailerCommandResult> SaveTrailerAsync(SaveTrailerRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (string.IsNullOrWhiteSpace(request.UnitNumber) || request.Vin.Trim().Length != 17 ||
            string.IsNullOrWhiteSpace(request.TrailerType) || string.IsNullOrWhiteSpace(request.LicensePlate) ||
            request.LicenseState.Trim().Length != 2 || request.MaxPayloadLbs <= 0)
            throw new ArgumentException("Complete all required trailer fields with valid values.", nameof(request));
        if (request.TrailerId.HasValue && request.ExpectedRowVersion is not { Length: 8 })
            throw new ArgumentException("A valid RowVersion is required.", nameof(request));

        await using DbConnection connection = _connectionFactory.CreateConnection();
        object parameters = request.TrailerId.HasValue ? new
        {
            TrailerId = request.TrailerId.Value,
            UnitNumber = request.UnitNumber.Trim(), Vin = request.Vin.Trim(), request.TrailerType,
            LicensePlate = request.LicensePlate.Trim(), LicenseState = request.LicenseState.Trim(),
            request.MaxPayloadLbs, FleetAssetStatusCode = request.StatusCode,
            ExpectedRowVersion = request.ExpectedRowVersion
        } : new
        {
            UnitNumber = request.UnitNumber.Trim(), Vin = request.Vin.Trim(), request.TrailerType,
            LicensePlate = request.LicensePlate.Trim(), LicenseState = request.LicenseState.Trim(),
            request.MaxPayloadLbs, FleetAssetStatusCode = request.StatusCode
        };
        var command = new CommandDefinition(request.TrailerId.HasValue ? "catalog.Trailer_Update" : "catalog.Trailer_Create", parameters, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.QuerySingleAsync<TrailerCommandResult>(command);
    }

    public async Task<TrailerCommandResult> SetTrailerActiveAsync(long trailerId, bool isActive, byte[] expectedRowVersion, CancellationToken cancellationToken = default)
    {
        if (trailerId <= 0) throw new ArgumentOutOfRangeException(nameof(trailerId));
        if (expectedRowVersion is not { Length: 8 }) throw new ArgumentException("A valid RowVersion is required.", nameof(expectedRowVersion));
        await using DbConnection connection = _connectionFactory.CreateConnection();
        var command = new CommandDefinition("catalog.Trailer_SetActive", new { TrailerId = trailerId, IsActive = isActive, ExpectedRowVersion = expectedRowVersion }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.QuerySingleAsync<TrailerCommandResult>(command);
    }
}
