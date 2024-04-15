using Microsoft.EntityFrameworkCore;
using TelemetryMicroservice.DataAccess;
using TelemetryMicroservice.DataAccess.Entities;
using TelemetryMicroservice.Repository.Contracts;
using TelemetryMicroservice.Repository.Contracts.Mappers;
using VehicleTelemetry = TelemetryMicroservice.Domain.Models.Vehicle.VehicleTelemetry;

namespace TelemetryMicroservice.Repository;

public class TelemetryRepository: ITelemetryRepository
{
    private readonly DataContext dataContext;
    private readonly ITelemetryMapper telemetryMapper;

    public TelemetryRepository(DataContext dataContext, ITelemetryMapper telemetryMapper)
    {
        this.dataContext = dataContext;
        this.telemetryMapper = telemetryMapper;
        
    }
    public async Task<Guid> AddTelemetry(VehicleTelemetry telemetry)
    {
        var telemetryToAdd = telemetryMapper.DomainToDataAccess(telemetry);
        var vehicle = await dataContext.Vehicles.FirstOrDefaultAsync(v => v.Id == telemetry.VehicleId);
        if (vehicle is null)
        {
            await dataContext.Vehicles.AddAsync(new Vehicle { Id = telemetry.VehicleId });
        }
        await dataContext.VehicleTelemetries.AddAsync(telemetryToAdd);
        await dataContext.SaveChangesAsync();
        return telemetryToAdd.Id;
    }

    public async Task<IList<VehicleTelemetry>> GetTelemetryForVehicle(Guid vehicleId)
    {
        var telemetriesForVehicle = await dataContext.VehicleTelemetries.Where(vt => vt.VehicleId == vehicleId)
            .OrderBy(vt => vt.CreateAt).ToListAsync();
        IList<VehicleTelemetry> telemetries = new List<VehicleTelemetry>();
        foreach (var tfv in telemetriesForVehicle)
        {
            telemetries.Add(telemetryMapper.DataAccessToDomain(tfv));
        }

        return telemetries;
    }
}