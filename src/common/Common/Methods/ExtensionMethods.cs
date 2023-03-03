namespace AsteriskDotHMG.Common.Methods;

public static partial class EnumExtensionMethods
{
    public static string GetDisplayName(this Enum enumValue)
    {
        string displayName;
        MemberInfo info = enumValue.GetType().GetMember(enumValue.ToString()).First();

        if (info != null && info.CustomAttributes.Any())
        {
            DisplayAttribute nameAttr = info.GetCustomAttribute<DisplayAttribute>();
            displayName = nameAttr != null ? nameAttr.Name : enumValue.ToString();
        }
        else
        {
            displayName = enumValue.ToString();
        }

        return displayName;
    }

    public static T ToEnum<T>(this string value) where T : struct, Enum
    {
        if (!Enum.TryParse<T>(value, true, out T result))
        {
            throw new ArgumentException($"Invalid value '{value}' for enum type {typeof(T).Name}");
        }
        return result;
    }

    public static bool IsValidEnum<T>(this string value) where T : struct, Enum
    {
        return !string.IsNullOrEmpty(value) && Enum.TryParse<T>(value, true, out T _);
    }
}

public static partial class FileExtensionMethods
{
    public static bool IsExcelFile(this IFormFile file)
    {
        return file != null && file.ContentType == Constants.EPPLUS_EXCEL_CONTENT_TYPE;
    }

    public static bool IsExcelFile(this string input)
    {
        return input.ToLower().EndsWith(".xlsx");
    }

    public static bool IsJSONFile(this IFormFile file)
    {
        return file != null && file.ContentType == Constants.JSON_CONTENT_TYPE;
    }

    public static bool IsJSONFile(this string fileName)
    {
        return fileName.ToLower().EndsWith(".json");
    }

    public static bool IsXMLFile(this IFormFile file)
    {
        return file != null && (file.ContentType == Constants.XML_CONTENT_TYPE || file.ContentType == Constants.XML_TEXT_CONTENT_TYPE);
    }

    public static bool IsXMLFile(this string fileName)
    {
        return fileName.ToLower().EndsWith(".xml");
    }
}

public static partial class StringExtensionMethods
{
    public static string ToCamelCase(this string input)
    {
        string[] words = input.Split(new[] { "_", " " }, StringSplitOptions.RemoveEmptyEntries);

        string leadWord = CamelCaseRegex().Replace(words[0], m =>
        {
            return m.Groups[1].Value.ToLower() + m.Groups[2].Value.ToLower() + m.Groups[3].Value;
        });

        string[] tailWords = words.Skip(1)
            .Select(word => char.ToUpper(word[0]) + word[1..])
            .ToArray();

        return $"{leadWord}{string.Join(string.Empty, tailWords)}";
    }

    public static int? GetIntValue(this string input, bool allowNull = false)
    {
        if (allowNull && string.IsNullOrEmpty(input))
        {
            return null;
        }

        return string.IsNullOrEmpty(input) ? 0 : Convert.ToInt32(input);
    }

    public static double? GetDoubleValue(this string input, bool allowNull = false)
    {
        if (allowNull && string.IsNullOrEmpty(input))
        {
            return null;
        }

        return string.IsNullOrEmpty(input) ? 0 : Convert.ToDouble(input);
    }

    public static bool? GetBoolValue(this string input, bool allowNull = false)
    {
        if (allowNull && string.IsNullOrEmpty(input))
        {
            return null;
        }

        return !string.IsNullOrEmpty(input) && (input.ToLower() == "true" || input == "1");
    }

    public static string GetStringValue(this string input, bool allowNull = false)
    {
        if (allowNull && string.IsNullOrEmpty(input))
        {
            return null;
        }

        return string.IsNullOrEmpty(input) ? string.Empty : input;
    }

    public static DateTime? GetDateTimeValue(this string input, bool allowNull = false)
    {
        if (allowNull && string.IsNullOrEmpty(input))
        {
            return null;
        }

        return string.IsNullOrEmpty(input) ? DateTime.Now : Convert.ToDateTime(input);
    }

    public static Guid? GetGuidValue(this string input)
    {
        return string.IsNullOrEmpty(input) ? null : Guid.Parse(input);
    }

    public static string ReplaceNullValue(this string input)
    {
        return string.IsNullOrEmpty(input) ? string.Empty : input;
    }

    public static string ReplaceIfNullOrEmpty(this string input, string replaceValue = "")
    {
        return string.IsNullOrEmpty(input) ? replaceValue : input;
    }

    public static string ReplaceIfNullOrEmptyOrCheckValue(this string input, string valueToCheck, string replaceValue = "")
    {
        return input.ReplaceNullValue().ToLower() == valueToCheck.ToLower() ? replaceValue : input.ReplaceNullValue();
    }

    [GeneratedRegex("([A-Z])([A-Z]+|[a-z0-9]+)($|[A-Z]\\w*)")]
    private static partial Regex CamelCaseRegex();
}

public static partial class NumberExtensionMethods
{
    public static int GetIterationCount(this int count, int size)
    {
        return (int)Math.Ceiling((double)count / size);
    }
}

public static partial class WebExtensionMethods
{
    public static bool IsDevelopment(this IWebHostEnvironment env)
    {
        return env.EnvironmentName == "Development" || env.EnvironmentName == "Local";
    }
}

public static partial class ObjectExtensionMethods
{
    public static T Clone<T>(this T source)
    {
        string serialized = JsonConvert.SerializeObject(source);
        return JsonConvert.DeserializeObject<T>(serialized);
    }
}

public static partial class ExceptionExtensionMethods
{
    public static int GetStatusCode(this Exception exception) =>
        exception switch
        {
            BadRequestException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            VE => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status500InternalServerError
        };

    public static string GetTitle(this Exception exception) =>
        exception switch
        {
            AE applicationException => applicationException.Title,
            _ => "Server Error"
        };

    public static List<ValidationError> GetErrors(this Exception exception)
    {
        List<ValidationError> errors = new();

        IReadOnlyDictionary<string, string[]> dictionary = null;

        if (exception is VE validationException)
        {
            if (validationException.ErrorsDictionary != null)
            {
                dictionary = validationException.ErrorsDictionary;
            }
            else if (validationException.Errors != null)
            {
                dictionary = validationException.Errors.AsReadOnly();
            }
        }

        if (dictionary != null && dictionary.Any())
        {
            foreach (KeyValuePair<string, string[]> dic in dictionary)
            {
                //mapping of validation exception coming from the built-in Identity
                List<KeyValuePair<string, string>> fieldMappings = new()
                {
                    new KeyValuePair<string, string>("ConcurrencyFailure", "Email"),
                    new KeyValuePair<string, string>("DefaultError", "Email"),
                    new KeyValuePair<string, string>("DuplicateEmail", "Email"),
                    new KeyValuePair<string, string>("DuplicateRoleName", "RoleName"),
                    new KeyValuePair<string, string>("DuplicateUserName", "Email"),
                    new KeyValuePair<string, string>("InvalidEmail", "Email"),
                    new KeyValuePair<string, string>("InvalidRoleName", "RoleName"),
                    new KeyValuePair<string, string>("InvalidToken", "Email"),
                    new KeyValuePair<string, string>("InvalidUserName", "Email"),
                    new KeyValuePair<string, string>("LoginAlreadyAssociated", "Email"),
                    new KeyValuePair<string, string>("PasswordMismatch", "Password"),
                    new KeyValuePair<string, string>("PasswordRequiresDigit", "Password"),
                    new KeyValuePair<string, string>("PasswordRequiresLower", "Password"),
                    new KeyValuePair<string, string>("PasswordRequiresNonAlphanumeric", "Password"),
                    new KeyValuePair<string, string>("PasswordRequiresUniqueChars", "Password"),
                    new KeyValuePair<string, string>("PasswordRequiresUpper", "Password"),
                    new KeyValuePair<string, string>("PasswordTooShort", "Password"),
                    new KeyValuePair<string, string>("RecoveryCodeRedemptionFailed", "Email"),
                    new KeyValuePair<string, string>("UserAlreadyHasPassword", "Email"),
                    new KeyValuePair<string, string>("UserAlreadyInRole", "Email"),
                    new KeyValuePair<string, string>("UserLockoutNotEnabled", "Email"),
                    new KeyValuePair<string, string>("UserNotInRole", "Email")
                };

                string dicKey = dic.Key;
                List<string> dicErrors = dic.Value.Select(e => ReplaceInValidationError(e, dic.Key)).ToList();
                KeyValuePair<string, string> keyMap = fieldMappings.FirstOrDefault(e => e.Key == dicKey);

                if (!keyMap.Equals(default(KeyValuePair<string, string>)))
                {
                    dicKey = keyMap.Value;
                }

                if (dicErrors.Any())
                {
                    ValidationError error = errors.FirstOrDefault(e => e.Field == dicKey);

                    if (error == null)
                    {
                        errors.Add(new(dicKey, dicErrors));
                    }
                    else
                    {
                        error.Errors.AddRange(dicErrors);
                    }
                }
            }
        }

        return errors;
    }

    private static string ReplaceInValidationError(string input, string key)
    {
        return input.Replace($"'{key}'", key);
    }

    public static ValidationData GetValidationData(this Exception exception)
    {
        return new(exception.GetTitle(), exception.GetStatusCode(), exception.Message, exception.GetErrors());
    }

    public static ErrorInfo CreateError(this Exception ex, string correlationId, object data = null)
    {
        return new(ex.Message, correlationId, data);
    }

    public static Models.ValidationResult CreateValidationError(this Exception ex, string correlationId)
    {
        return new(ex.GetErrors(), ex.Message, correlationId);
    }
}

public static partial class CollectionExtensionMethods
{
    public static IQueryable<T> FilterQuery<T>(this IQueryable<T> query, Dictionary<string, (QueryFilterOperator Operator, object Value)> filterParams, QueryFilterCondition condition)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
        Expression expression = null;
        foreach (KeyValuePair<string, (QueryFilterOperator Operator, object Value)> kvp in filterParams)
        {
            Expression propertyExpression = Expression.Property(parameter, kvp.Key);
            Expression valueExpression = Expression.Constant(kvp.Value.Value);
            Expression binaryExpression = kvp.Value.Operator switch
            {
                QueryFilterOperator.Equal => Expression.Equal(propertyExpression, valueExpression),
                QueryFilterOperator.NotEqual => Expression.NotEqual(propertyExpression, valueExpression),
                QueryFilterOperator.Contains => Expression.Call(propertyExpression, typeof(string).GetMethod("Contains", new[] { typeof(string) }), valueExpression),
                QueryFilterOperator.StartsWith => Expression.Call(propertyExpression, typeof(string).GetMethod("StartsWith", new[] { typeof(string) }), valueExpression),
                QueryFilterOperator.EndsWith => Expression.Call(propertyExpression, typeof(string).GetMethod("EndsWith", new[] { typeof(string) }), valueExpression),
                _ => throw new ArgumentException("Invalid operator."),
            };

            if (expression == null)
            {
                expression = binaryExpression;
            }
            else
            {
                if (condition == QueryFilterCondition.Or)
                {
                    expression = Expression.OrElse(expression, binaryExpression);
                }
                else
                {
                    expression = Expression.AndAlso(expression, binaryExpression);
                }
            }
        }

        return query.Where(Expression.Lambda<Func<T, bool>>(expression, parameter));
    }
}

public static partial class OperationExtensionMethods
{
    public static void CreateLog(this ILogger logger, string action, string message, LogLevel logLevel = LogLevel.Information, Exception exception = null)
    {
        if (string.IsNullOrEmpty(action))
        {
            action = "[LOG]";
        }

        if (logLevel == LogLevel.Information)
        {
            logger.LogInformation("{action} {message}", action, message);
        }
        else if (logLevel == LogLevel.Warning)
        {
            logger.LogWarning("{action} {message}", action, message);
        }
        else if (logLevel == LogLevel.Error)
        {
            logger.LogError(exception, "{action} {message}", action, message);
        }
    }

    public static void StartTrigger(this ILogger logger, string functionName, string correlationId, params object[] args)
    {
        string message = CreateMessage($"{functionName} - trigger started;", args);
        CreateImportLog(logger, Constants.LOGGING_ACTION_TRIGGER_START, message, correlationId);
    }

    public static void SetTriggerError(this ILogger logger, string functionName, string message, Exception ex, string correlationId, params object[] args)
    {
        message = CreateMessage($"{functionName} - {message}; Error: {ex.Message}", args);

        CreateImportLog(logger, Constants.LOGGING_ACTION_TRIGGER_ERROR, message, correlationId, LogLevel.Error, ex);
    }

    public static void EndTrigger(this ILogger logger, string functionName, string correlationId, params object[] args)
    {
        string message = CreateMessage($"{functionName} - trigger completed;", args);
        CreateImportLog(logger, Constants.LOGGING_ACTION_TRIGGER_END, message, correlationId);
    }

    public static void StartApi(this ILogger logger, string functionName, string apiAction, string correlationId, params object[] args)
    {
        string message = CreateMessage($"{functionName} - {apiAction} request started;", args);
        CreateImportLog(logger, Constants.LOGGING_ACTION_TRIGGER_API_START, message, correlationId);
    }

    public static void EndApi(this ILogger logger, string functionName, string apiAction, string correlationId, params object[] args)
    {
        string message = CreateMessage($"{functionName} - {apiAction} request completed;", args);
        CreateImportLog(logger, Constants.LOGGING_ACTION_TRIGGER_API_END, message, correlationId);
    }

    public static void SetTriggerSuccessResult(this ILogger logger, string functionName, string message, string correlationId, params object[] args)
    {
        message = CreateMessage($"{functionName} - {message};", args);
        CreateImportLog(logger, Constants.LOGGING_ACTION_TRIGGER_RESULT, message, correlationId);
    }

    public static void SetTriggerErrorResult(this ILogger logger, string functionName, string message, Exception ex, string correlationId, params object[] args)
    {
        message = CreateMessage($"{functionName} - {message}; Error: {ex.Message}", args);
        CreateImportLog(logger, Constants.LOGGING_ACTION_TRIGGER_RESULT, message, correlationId, LogLevel.Error, ex);
    }

    public static void SetTriggerWarningResult(this ILogger logger, string functionName, string message, string warningLabel, Exception ex, string correlationId, params object[] args)
    {
        message = CreateMessage($"{functionName} - {message}; ${warningLabel}: {ex.Message}", args);
        CreateImportLog(logger, Constants.LOGGING_ACTION_TRIGGER_RESULT, message, correlationId, LogLevel.Warning);
    }

    public static void SetTriggerProcess(this ILogger logger, string functionName, string message, string correlationId, params object[] args)
    {
        message = CreateMessage($"{functionName} - {message};", args);
        CreateImportLog(logger, Constants.LOGGING_ACTION_TRIGGER_PROCESS, message, correlationId);
    }

    private static void CreateImportLog(ILogger logger, string action, string message, string correlationId, LogLevel loglevel = LogLevel.Information, Exception exception = null)
    {

        string separator = message.EndsWith(";") ? " " : ", ";
        string logMessage = $"{message}{separator}Correlation Id: {correlationId}";

        logger.CreateLog(action, logMessage, loglevel, exception);
    }

    private static string CreateMessage(string message, params object[] args)
    {
        if (args != null && args.Any() && !message.EndsWith(";") && !message.Contains(','))
        {
            message += "; ";
        }

        for (int i = 0; i < args.Length; i++)
        {
            object obj = args[i];

            message += $"{obj}";

            if (i != args.Length - 1)
            {
                message += ", ";
            }
        }

        return message;
    }
}