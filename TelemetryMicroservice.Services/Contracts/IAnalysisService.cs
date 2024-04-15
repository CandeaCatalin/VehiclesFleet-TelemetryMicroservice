namespace TelemetryMicroservice.Services.Contracts;

public interface IAnalysisService
{
    public Task SendGenerateCommandForAnalysis(Guid vehicleId, string? token);
}