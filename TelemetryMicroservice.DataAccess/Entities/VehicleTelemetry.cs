namespace TelemetryMicroservice.DataAccess.Entities;

public class VehicleTelemetry
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }
    public int ActualSpeed { get; set; }
    public int KilometersSinceStart { get; set; }
    public decimal Latitude { get; set; } 
    public decimal Longitude { get; set; }
    public decimal Fuel { get; set; }
    public float TirePressure { get; set; }
    public DateTime CreateAt { get; set; }
}