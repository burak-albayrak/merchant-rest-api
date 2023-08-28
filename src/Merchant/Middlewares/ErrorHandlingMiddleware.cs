using System.Reflection.Metadata;
using Merchant.Exceptions;

namespace Merchant.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (ErrorDetail e)
        {
            _logger.LogError(e, "[{StatusCode}] - {ErrorMessage}", e.StatusCode, e.Message);
            await HandleError(httpContext, e);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[500] - {ErrorMessage}", e.Message);
        }
    }

    public async Task HandleError(HttpContext httpContext, ErrorDetail errorDetail)
    {
        httpContext.Response.StatusCode = errorDetail.StatusCode;
        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsync(new ErrorDetail()
        {
            StatusCode = errorDetail.StatusCode,
            Message = errorDetail.Message
        }.ToString());
    }
}
