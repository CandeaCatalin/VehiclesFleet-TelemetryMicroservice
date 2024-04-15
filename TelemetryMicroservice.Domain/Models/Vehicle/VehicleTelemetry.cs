namespace TelemetryMicroservice.Domain.Models.Vehicle;

public class VehicleTelemetry
{
    public Guid VehicleId { get; set; }
    public int ActualSpeed { get; set; }
    public int KilometersSinceStart { get; set; }
    public decimal Latitude { get; set; } 
    public decimal Longitude { get; set; }
    public decimal Fuel { get; set; }
    public float TirePressure { get; set; }
    public IList<Error>? Errors { get; set; }
}