using TelemetryMicroservice.BusinessLogic.Contracts;
using TelemetryMicroservice.Domain.Dtos;
using TelemetryMicroservice.Domain.Models.Vehicle;
using TelemetryMicroservice.Repository.Contracts;
using TelemetryMicroservice.Services.Contracts;

namespace TelemetryMicroservice.BusinessLogic;

public class TelemetryBusinessLogic: ITelemetryBusinessLogic
{
    private readonly ITelemetryRepository telemetryRepository;
    private readonly IAnalysisService analysisService;
    private readonly IVehiclesService vehiclesService;
    private readonly ILoggerService loggerService;
    
    public TelemetryBusinessLogic(ITelemetryRepository telemetryRepository, ILoggerService loggerService,IAnalysisService analysisService,IVehiclesService vehiclesService)
    {
        this.telemetryRepository = telemetryRepository;
        this.loggerService = loggerService;
        this.analysisService = analysisService;
        this.vehiclesService = vehiclesService;
    }
    
    public async Task AddTelemetry(AddTelemetryDto dto, string? token)
    {
        await dto.ValidateAndThrow();

        var telemetry = new VehicleTelemetry
        {
            VehicleId = dto.VehicleId,
            ActualSpeed = dto.ActualSpeed,
            KilometersSinceStart = dto.KilometersSinceStart,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            Fuel = dto.Fuel,
            TirePressure = dto.TirePressure
        };

        var telemetryId =  await telemetryRepository.AddTelemetry(telemetry);
        if (dto.Errors.Count() != 0)
        {
            await vehiclesService.SendErrorsForVehicle(dto.Errors,dto.VehicleId,token);
            
        }
        if (dto.ActualSpeed == 0 && dto.KilometersSinceStart != 0)
        {
            await analysisService.SendGenerateCommandForAnalysis(dto.VehicleId,token);
        }
        await loggerService.LogInfo($"Telemetry with Id: {telemetryId} was added for vehicle {dto.VehicleId}", token);
        
    }

    public async Task<IList<VehicleTelemetry>> GetTelemetryForVehicle(GetTelemetryDto dto)
    {
        if (dto.VehicleId.Equals(new Guid()))
        {
            throw new Exception("Vehicle id cannot be null");
        }

        var telemetries = await telemetryRepository.GetTelemetryForVehicle(dto.VehicleId);
        return telemetries;
    }
}