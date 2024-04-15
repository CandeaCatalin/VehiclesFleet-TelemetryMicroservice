using TelemetryMicroservice.DataAccess.Entities;
using TelemetryMicroservice.Repository.Contracts.Mappers;

namespace TelemetryMicroservice.Repository.Mappers;

public class TelemetryMapper:ITelemetryMapper
{
    public VehicleTelemetry DomainToDataAccess(Domain.Models.Vehicle.VehicleTelemetry telemetry)
    {
        return new VehicleTelemetry
        {
            Id = Guid.NewGuid(),
            VehicleId = telemetry.VehicleId,
            ActualSpeed = telemetry.ActualSpeed,
            KilometersSinceStart = telemetry.KilometersSinceStart,
            Latitude = telemetry.Latitude,
            Longitude = telemetry.Longitude,
            Fuel = telemetry.Fuel,
            TirePressure = telemetry.TirePressure,
            CreateAt = DateTime.Now
        };
    }

    public Domain.Models.Vehicle.VehicleTelemetry DataAccessToDomain(VehicleTelemetry telemetry)
    {
        return new Domain.Models.Vehicle.VehicleTelemetry
        {
            VehicleId = telemetry.VehicleId,
            ActualSpeed = telemetry.ActualSpeed,
            KilometersSinceStart = telemetry.KilometersSinceStart,
            Latitude = telemetry.Latitude,
            Longitude = telemetry.Longitude,
            Fuel = telemetry.Fuel,
            TirePressure = telemetry.TirePressure,
        };
    }
}