using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Extensions;
using TelemetryMicroservice.Domain.CustomExceptions;
using TelemetryMicroservice.Services.Contracts;

namespace TelemetryMicroservice.Middleware;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILoggerService loggerService;

    public ExceptionHandlerMiddleware(RequestDelegate next,ILoggerService loggerService)
    {
        this.next = next;
        this.loggerService = loggerService;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (HttpStatusCodeException ex)
        {
            await LogErrorWithRequestInput(context, ex);

            await HandleStatusCodeException(context, ex);
        }
        catch (Exception ex)
        {
            await LogErrorWithRequestInput(context, ex);

            await HandleUnknownException(context, ex);
        }
    }

    private static Task HandleStatusCodeException(HttpContext context, HttpStatusCodeException ex)
    {
        context.Response.StatusCode = (int)ex.StatusCode;
        context.Response.ContentType = "application/json";

        return context.Response.WriteAsync(JsonSerializer.Serialize(new { Message = ex.DisplayMessage }));
    }

    private static Task HandleUnknownException(HttpContext context, Exception ex)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        return context.Response.WriteAsync(JsonSerializer.Serialize(new { ex.Message }));
    }

    private async Task LogErrorWithRequestInput(HttpContext context, Exception ex)
    {
        var result = new StringBuilder();
        var token = "";
        result.AppendLine($"Message: {ex.Message}");

        result.AppendLine($"URL: {context.Request.GetEncodedUrl()}");

        result.AppendLine("Headers: ");
        foreach (var (key, value) in context.Request.GetTypedHeaders().Headers)
        {
            if (key.Equals("Authorization"))
            {
                token = value;
            }
            result.AppendLine($"{key}: {value}");
        }
        
        result.AppendLine("Body: ");
        using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
        {
            result.AppendLine(await reader.ReadToEndAsync());
        }

        result.AppendLine($"Exception: {ex}");

        await loggerService.LogError(result.ToString(),token);
    }
}