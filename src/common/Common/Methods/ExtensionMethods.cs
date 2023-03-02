namespace AsteriskDotHMG.Common.Methods;

public static partial class ExtensionMethods
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

    public static T Clone<T>(this T source)
    {
        string serialized = JsonConvert.SerializeObject(source);
        return JsonConvert.DeserializeObject<T>(serialized);
    }

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

    public static bool? GetBoolValue(this string input, bool allowNull = false)
    {
        if (allowNull && string.IsNullOrEmpty(input))
        {
            return null;
        }

        return !string.IsNullOrEmpty(input) && (input.ToLower() == "true" || input == "1");
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

    public static bool IsDevelopment(this IWebHostEnvironment env)
    {
        return env.EnvironmentName == "Development" || env.EnvironmentName == "Local";
    }

    public static string ReplaceIfNullOrEmpty(this string input, string replaceValue = "")
    {
        return string.IsNullOrEmpty(input) ? replaceValue : input;
    }

    public static string ReplaceIfNullOrEmptyOrCheckValue(this string input, string valueToCheck, string replaceValue = "")
    {
        return input.ReplaceNullValue().ToLower() == valueToCheck.ToLower() ? replaceValue : input.ReplaceNullValue();
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

    public static int GetIterationCount(this int count, int size)
    {
        return (int)Math.Ceiling((double)count / size);
    }

    [GeneratedRegex("([A-Z])([A-Z]+|[a-z0-9]+)($|[A-Z]\\w*)")]
    private static partial Regex CamelCaseRegex();

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
