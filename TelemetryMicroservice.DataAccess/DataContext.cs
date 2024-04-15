using Microsoft.EntityFrameworkCore;
using TelemetryMicroservice.DataAccess.Entities;

namespace TelemetryMicroservice.DataAccess;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {

        builder.Entity<VehicleTelemetry>().Property(p => p.Latitude).HasColumnType("decimal(10,7)");
        builder.Entity<VehicleTelemetry>().Property(p => p.Longitude).HasColumnType("decimal(10,7)");
        builder.Entity<VehicleTelemetry>().Property(p => p.Fuel).HasColumnType("decimal(5,2)");
    }
    
    public DbSet<VehicleTelemetry> VehicleTelemetries { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
}