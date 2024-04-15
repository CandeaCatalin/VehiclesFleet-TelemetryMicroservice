using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TelemetryMicroservice.Domain.Models.Vehicle;
using TelemetryMicroservice.Services.Contracts;
using TelemetryMicroservice.Settings;

namespace TelemetryMicroservice.Services;

public class VehiclesService: IVehiclesService
{
    private readonly IAppSettingsReader appSettingsReader;
    private readonly HttpClient httpClient;

    public VehiclesService(IAppSettingsReader appSettingsReader)
    {
        this.appSettingsReader = appSettingsReader;
        var clientHandler = new HttpClientHandler();
        httpClient = new HttpClient(clientHandler);
    }
    
    public async Task SendErrorsForVehicle(IList<Error> errorsList,Guid vehicleId, string? token)
    {
        if (token is not null)
        {
            httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);
        }
       
        
        var vehiclesErrorUrl = appSettingsReader.GetValue(AppSettingsConstants.Section.VehiclesMicroserviceSectionName, AppSettingsConstants.Keys.SendErrorsUrlKey);

        var response = await httpClient.PutAsync(vehiclesErrorUrl, GetHttpContent(errorsList,vehicleId));

        response.Dispose();
    }
    
    private HttpContent GetHttpContent(IList<Error> errorsList,Guid vehicleId)
    {
        var content = JsonSerializer.Serialize(new
        {
            ErrorsList = errorsList, VehicleId = vehicleId
        });

        return new StringContent(content, Encoding.UTF8, "application/json");
    }
}