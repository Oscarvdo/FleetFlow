using FleetFlow.Application.Fleet;

namespace FleetFlow.Application.Abstractions.Fleet;

public interface IFleetCommandService
{
    Task<VehicleCommandResult> SaveVehicleAsync(SaveVehicleRequest request, CancellationToken cancellationToken = default);
    Task<VehicleCommandResult> SetVehicleActiveAsync(long vehicleId, bool isActive, byte[] expectedRowVersion, CancellationToken cancellationToken = default);
}
