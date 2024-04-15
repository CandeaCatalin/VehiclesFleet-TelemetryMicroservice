using TelemetryMicroservice.Domain.Models.Vehicle;

namespace TelemetryMicroservice.Services.Contracts;

public interface IVehiclesService
{
    public Task SendErrorsForVehicle(IList<Error> errorsList, Guid vehicleId, string? token);
}