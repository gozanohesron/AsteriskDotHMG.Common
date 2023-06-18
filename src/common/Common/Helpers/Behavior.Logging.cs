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
                Type requestType = request.GetType();
                PropertyInfo[] properties = requestType.GetProperties();
                Dictionary<string, object> values = new();
                string requestData = string.Empty;

                foreach (PropertyInfo property in properties)
                {
                    if (!string.IsNullOrEmpty(requestData))
                    {
                        requestData += ", ";
                    }

                    object value = property.GetValue(request);
                    bool willHideValue = false;

                    List<string> excludedProperties = new() { "password", "newpassword", "confirmpassword", "oldpassword" };

                    if (excludedProperties.Contains(property.Name.ToLower()))
                    {
                        value = "<Sensitive information hidden>";

                    }
                    else
                    {
                        HideValueOnLoggerAttribute attribute = property.GetCustomAttribute<HideValueOnLoggerAttribute>();
                        willHideValue = property.IsDefined(typeof(HideValueOnLoggerAttribute), false);

                        if (willHideValue && !string.IsNullOrEmpty(attribute.Message))
                        {
                            value = $"<{attribute.Message}>";
                        }
                    }

                    if (!willHideValue && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                    {
                        if (property.PropertyType.Name.StartsWith("List"))
                        {
                            if (property.GetValue(request) is IEnumerable collection)
                            {
                                int count = collection.Cast<object>().Count();
                                value = $"<List containing {count} record{(count > 1 ? "s" : string.Empty)}>";
                            }
                            else
                            {
                                value = "null";
                            }
                        }
                    }

                    requestData += $"{property.Name}: {(value ??"null")}";
                }

                if (!string.IsNullOrEmpty(requestData))
                {
                    _logger.CreateLog(Constants.LOGGING_ACTION_DATA, $"Processing data; Request data: {requestData}, CorrelationId: {correlationId}");
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