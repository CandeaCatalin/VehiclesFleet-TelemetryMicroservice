using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TelemetryMicroservice.Services.Contracts;
using TelemetryMicroservice.Settings;

namespace TelemetryMicroservice.Services;

public class AnalysisService : IAnalysisService
{private readonly IAppSettingsReader appSettingsReader;
    private readonly HttpClient httpClient;

    public AnalysisService(IAppSettingsReader appSettingsReader)
    {
        this.appSettingsReader = appSettingsReader;
        var clientHandler = new HttpClientHandler();
        httpClient = new HttpClient(clientHandler);
    }
    
    
    private HttpContent GetHttpContent(Guid vehicleId)
    {
        var content = JsonSerializer.Serialize(new
        {
           VehicleId = vehicleId
        });

        return new StringContent(content, Encoding.UTF8, "application/json");
    }
    
    public async Task SendGenerateCommandForAnalysis(Guid vehicleId, string? token)
    {
        if (token is not null)
        {
            httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);
        }
       
        
        var addAnalysisUrl = appSettingsReader.GetValue(AppSettingsConstants.Section.AnalysisMicroserviceSectionName, AppSettingsConstants.Keys.GenerateAnalysisUrlKey);

        var response = await httpClient.PostAsync(addAnalysisUrl, GetHttpContent(vehicleId));

        response.Dispose();
    }
}