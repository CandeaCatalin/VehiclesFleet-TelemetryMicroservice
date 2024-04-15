using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using TelemetryMicroservice.BusinessLogic.Contracts;
using TelemetryMicroservice.Domain.Dtos;

namespace TelemetryMicroservice.Controllers;

[ApiController]
[Route("telemetry")]
[Authorize]
public class TelemetryController: ControllerBase
{
    private readonly ITelemetryBusinessLogic telemetryBusinessLogic;

    public TelemetryController(ITelemetryBusinessLogic telemetryBusinessLogic)
    {
        this.telemetryBusinessLogic = telemetryBusinessLogic;
    }
    
    [HttpPost("add")]
    public async Task<IActionResult> AddTelemetry(AddTelemetryDto dto)
    {
        var token = GetToken();
        await telemetryBusinessLogic.AddTelemetry(dto,token);
        return Ok();
    }

    [HttpPost("getTelemetries")]
    public async Task<IActionResult> GetTelemetryForVehicle(GetTelemetryDto dto)
    {
        var telemetries = await telemetryBusinessLogic.GetTelemetryForVehicle(dto);
        return Ok(telemetries);
    }
    
    private string? GetToken()
    {
        if (Request.Headers.TryGetValue("Authorization", out StringValues authHeaderValue))
        {
            var token = authHeaderValue.ToString().Replace("Bearer ", "");
              
            return token;
        }

        return null;
    }
}