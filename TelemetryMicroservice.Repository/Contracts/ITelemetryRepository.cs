using TelemetryMicroservice.Domain.Models.Vehicle;

namespace TelemetryMicroservice.Repository.Contracts;

public interface ITelemetryRepository
{
    public Task<Guid> AddTelemetry(VehicleTelemetry telemetry);
    public Task<IList<VehicleTelemetry>> GetTelemetryForVehicle(Guid vehicleId);
}