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

                    List<string> excludedProperties = new()
                    {
                        // Common passwords
                        "password", "newpassword", "confirmpassword", "oldpassword", "pwd",

                        // Personal identifiers
                        "lastname", "firstname", "middlename", "fullname", "nickname", "username", "user", "name",
    
                        // Email
                        "email", "emailaddress", "emailaddr", "emaiadd", "e_mail",

                        // Phone/Mobile
                        "mobile", "mobilenum", "mobilenumber", "phone", "phonenum", "phonenumber", "contact", "contactnumber",

                        // User identity
                        "requestor", "identifier", "idnumber", "id", "userid", "user_id",

                        // Location-based
                        "address", "homeaddress", "workaddress", "residentialaddress", "location", "coordinates",

                        // Auth and tokens
                        "token", "accesstoken", "refreshtoken", "keytoken", "authtoken", "apikey", "authkey", "secret", "privatekey",

                        // Banking/payment
                        "creditcard", "cardnumber", "cvv", "cvc", "bankaccount", "iban", "swift", "routingnumber",

                        // Government-issued
                        "ssn", "socialsecuritynumber", "passport", "driverlicense", "nationalid",

                        // One-time PIN / Verification
                        "otp", "one_time_pin", "onetimepin", "oneTimePin", "pin", "verificationcode", "verification_code", "authcode", "auth_code", "securitycode", "security_code", "accesscode",

                    };


                    if (excludedProperties.Any(p => property.Name.ToLower().Contains(p)))
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