namespace TelemetryMicroservice.Services.Contracts;

public interface ILoggerService
{
    Task LogInfo(string message, string? token);
    Task LogError(string message, string? token);
}