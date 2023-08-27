using System.Reflection.Metadata;
using Merchant.Exceptions;

namespace Merchant.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (ErrorDetail e)
        {
            await HandleError(httpContext, e);
        }
        catch (Exception)
        {
            Console.WriteLine("Error Yo");
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
