using System.Net;
using System.Text.Json;
using cse325_team7_project.Api.Common;

namespace cse325_team7_project.Api.Middleware;

//middleware: catches all exceptions and returns the correct status code and message
// it also logs the error (no need for try catch in controller routes, we can just throw the errors on service layer)
public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    private readonly RequestDelegate _next = next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger = logger;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (HttpException httpEx)
        {
            await WriteResponse(context, httpEx.StatusCode, httpEx.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await WriteResponse(context, (int)HttpStatusCode.InternalServerError, "An unexpected error occurred");
        }
    }

    private static async Task WriteResponse(HttpContext context, int statusCode, string message)
    {
        if (context.Response.HasStarted)
        {
            return;
        }

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var payload = JsonSerializer.Serialize(new { message }, SerializerOptions);
        await context.Response.WriteAsync(payload);
    }
}
