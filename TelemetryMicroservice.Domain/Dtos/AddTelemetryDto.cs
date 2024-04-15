using FluentValidation;
using TelemetryMicroservice.Domain.Models.Vehicle;
using TelemetryMicroservice.Domain.Validators;

namespace TelemetryMicroservice.Domain.Dtos;

public class AddTelemetryDto
{
    public static AddTelemetryDtoValidator Validator => new();
    public Guid VehicleId { get; set; }
    public int ActualSpeed { get; set; }
    public int KilometersSinceStart { get; set; }
    public decimal Latitude { get; set; } 
    public decimal Longitude { get; set; }
    public decimal Fuel { get; set; }
    public float TirePressure { get; set; }
    public IList<Error> Errors { get; set; }
    
    public async Task ValidateAndThrow()
    {
        await Validator.ValidateAndThrowAsync(this);
    }
}