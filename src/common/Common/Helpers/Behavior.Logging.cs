namespace AsteriskDotHMG.Common.Helpers;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    private readonly ICorrelationContext _correlationContext;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger, ICorrelationContext correlationContext)
    {
        _logger = logger;
        _correlationContext = correlationContext;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;
        string correlationId = _correlationContext.CorrelationId;

        _logger.CreateLog(Constants.LOGGING_ACTION_MEDIATR_START, $"Handling {requestName}; CorrelationId: {correlationId}");

        Stopwatch stopwatch = Stopwatch.StartNew();
        TResponse response;

        try
        {
            try
            {

                if (requestName == "AuthCommand")
                {
                    _logger.CreateLog(Constants.LOGGING_ACTION_DATA, $"Handling User Login; Email: {request.GetType().GetProperties().Where(e => e.Name == "Email").FirstOrDefault().GetValue(request)}, CorrelationId: {correlationId}");
                }
                else
                {
                    Type requestType = request.GetType();
                    PropertyInfo[] properties = requestType.GetProperties();

                    if (properties.Any(e => e.PropertyType.Name.StartsWith("List")))
                    {
                        Dictionary<string, object> values = new();

                        foreach (PropertyInfo property in properties)
                        {
                            if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                            {
                                if (property.PropertyType.Name.StartsWith("List"))
                                {
                                    IEnumerable collection = property.GetValue(request) as IEnumerable;

                                    if (collection != null)
                                    {
                                        int count = collection.Cast<object>().Count();
                                        values.Add(property.Name, $"List contains {count} record(s)");
                                    }
                                    else
                                    {
                                        values.Add(property.Name, "Null");
                                    }
                                }
                                else
                                {
                                    values.Add(property.Name, property.GetValue(request));
                                }
                            }
                            else
                            {
                                values.Add(property.Name, property.GetValue(request));
                            }
                        }

                        string requestData = SJ.JsonSerializer.Serialize(values);

                        _logger.CreateLog(Constants.LOGGING_ACTION_DATA, $"Processing data;  Request data: {requestData}, CorrelationId: {correlationId}");
                    }
                    else
                    {
                        string requestData = SJ.JsonSerializer.Serialize(request);

                        _logger.CreateLog(Constants.LOGGING_ACTION_DATA, $"Processing data; Request data: {requestData}, CorrelationId: {correlationId}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.CreateLog(Constants.LOGGING_ACTION_GENERIC_ERROR, $"Pipeline Logging Behavior; Error: {ex.Message}, CorrelationId: {correlationId}", LogLevel.Error, ex);
            }
            response = await next();
        }
        finally
        {
            stopwatch.Stop();
            _logger.CreateLog(Constants.LOGGING_ACTION_MEDIATR_END, $"Handled {requestName} with execution time {stopwatch.ElapsedMilliseconds}ms; CorrelationId: {correlationId}");
        }
        return response;
    }
}