namespace AsteriskDotHMG.Common.Helpers;

public class WebApiCorrelationGeneratorMiddleware
{
    private readonly RequestDelegate _next;

    public WebApiCorrelationGeneratorMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string correlationId = Guid.NewGuid().ToString();
        context.TraceIdentifier = correlationId;
        await _next(context);
    }
}
