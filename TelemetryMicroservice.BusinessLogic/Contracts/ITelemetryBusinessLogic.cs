using TelemetryMicroservice.Domain.Dtos;
using TelemetryMicroservice.Domain.Models.Vehicle;

namespace TelemetryMicroservice.BusinessLogic.Contracts;

public interface ITelemetryBusinessLogic
{
    Task AddTelemetry(AddTelemetryDto dto,string? token);
    Task<IList<VehicleTelemetry>> GetTelemetryForVehicle(GetTelemetryDto dto);
}