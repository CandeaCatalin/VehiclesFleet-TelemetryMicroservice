using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TelemetryMicroservice.Services.Contracts;
using TelemetryMicroservice.Settings;

namespace TelemetryMicroservice.Services;

public class LoggerService : ILoggerService
{
    private readonly IAppSettingsReader appSettingsReader;
    private readonly HttpClient httpClient;

    public LoggerService(IAppSettingsReader appSettingsReader)
    {
        this.appSettingsReader = appSettingsReader;
        var clientHandler = new HttpClientHandler();
        httpClient = new HttpClient(clientHandler);
    }

    public async Task LogInfo(string message, string? token)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentNullException(nameof(message));
        }

        if (token is not null)
        {
            httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);
        }
       
        
        var logInfoUrl = appSettingsReader.GetValue(AppSettingsConstants.Section.LoggerMicroserviceSectionName, AppSettingsConstants.Keys.LogInfoUrlKey);

        var response = await httpClient.PostAsync(logInfoUrl, GetHttpContent(message));

        response.Dispose();
    }

    public async Task LogError(string message, string? token)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentNullException(nameof(message));
        }
        if (token is not null)
        {
            httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);
        }
        
        var logErrorUrl = appSettingsReader.GetValue(AppSettingsConstants.Section.LoggerMicroserviceSectionName, AppSettingsConstants.Keys.LogErrorUrlKey);

        var response = await httpClient.PostAsync(logErrorUrl, GetHttpContent(message));

        response.Dispose();
    }

    private HttpContent GetHttpContent(string message)
    {
        var applicationName = appSettingsReader.GetValue(AppSettingsConstants.Section.RunningConfigurationSectionName, AppSettingsConstants.Keys.ApplicationNameKey);
       
        var content = JsonSerializer.Serialize(new
        {
            Message = message,
            Source = applicationName
        });

        return new StringContent(content, Encoding.UTF8, "application/json");
    }
}