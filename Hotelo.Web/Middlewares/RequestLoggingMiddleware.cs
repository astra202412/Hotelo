namespace Hotelo.Web.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation("REQ {Method} {Path} [{User}]",
            context.Request.Method,
            context.Request.Path,
            context.User?.Identity?.Name ?? "Anonyme");
        await _next(context);
    }
}