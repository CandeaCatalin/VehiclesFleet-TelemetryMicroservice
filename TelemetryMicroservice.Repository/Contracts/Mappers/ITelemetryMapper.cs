using TelemetryMicroservice.DataAccess.Entities;

namespace TelemetryMicroservice.Repository.Contracts.Mappers;

public interface ITelemetryMapper
{
    VehicleTelemetry DomainToDataAccess(Domain.Models.Vehicle.VehicleTelemetry telemetry);
    Domain.Models.Vehicle.VehicleTelemetry DataAccessToDomain(VehicleTelemetry telemetry);
}