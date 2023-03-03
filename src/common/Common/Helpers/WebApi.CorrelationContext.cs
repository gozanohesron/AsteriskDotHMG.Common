namespace AsteriskDotHMG.Common.Helpers;

public class WebApiCorrelationContext : ICorrelationContext
{
    private readonly IHttpContextAccessor _accessor;

    public WebApiCorrelationContext(IHttpContextAccessor accesor)
    {
        _accessor = accesor;
    }

    public string CorrelationId
    {
        get => _accessor.HttpContext?.TraceIdentifier;
        set => _accessor.HttpContext.TraceIdentifier = value;
    }
}
