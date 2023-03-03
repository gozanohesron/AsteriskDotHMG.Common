namespace AsteriskDotHMG.Common.Helpers;

public class FunctionCorrelationContext: ICorrelationContext
{
    private readonly IFunctionContextAccessor _context;

    public FunctionCorrelationContext(IFunctionContextAccessor httpContextAccessor)
    {
        _context = httpContextAccessor;
    }

    public string CorrelationId
    {
        get => _context.FunctionContext.Items["CorrelationId"].ToString();
        set => _context.FunctionContext.Items["CorrelationId"] = value;
    }
}
