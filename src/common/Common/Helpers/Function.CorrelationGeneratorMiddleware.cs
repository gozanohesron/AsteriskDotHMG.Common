namespace AsteriskDotHMG.Common.Helpers;

public class FunctionCorrelationGeneratorMiddleware : IFunctionsWorkerMiddleware
{
    private IFunctionContextAccessor FunctionContextAccessor { get; }

    public FunctionCorrelationGeneratorMiddleware(IFunctionContextAccessor accessor)
    {
        FunctionContextAccessor = accessor;
    }

    public Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        string correlationId = Guid.NewGuid().ToString();
        context.Items["CorrelationId"] = correlationId;

        FunctionContextAccessor.FunctionContext = context;
        return next(context);
    }
}
