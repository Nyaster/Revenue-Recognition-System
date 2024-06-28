using Revenue_Recognition_System.Exceptions;

namespace Revenue_Recognition_System.Middlewares;

public class ErrorHandlingMiddleWare
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleWare> _logger;

    public ErrorHandlingMiddleWare(RequestDelegate next, ILogger<ErrorHandlingMiddleWare> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Unauthorized e)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsJsonAsync("You cannot have access to this enpoint\\function");
        }
        catch (NotFoundException e)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync("Not found exception");
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync("Unhandled error occured");
        }
    }
}