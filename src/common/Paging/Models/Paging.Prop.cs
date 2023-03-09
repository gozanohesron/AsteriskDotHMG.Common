namespace AsteriskDotHMG.Paging.Models;

public class PagingProp<TModel>
{
    private static readonly string _bool = typeof(bool).FullName;
    private static readonly string _decimal = typeof(decimal).FullName;
    private static readonly string _int = typeof(int).FullName;
    private static readonly string _uint = typeof(uint).FullName;
    private static readonly string _long = typeof(long).FullName;
    private static readonly string _ulong = typeof(ulong).FullName;
    private static readonly string _char = typeof(char).FullName;
    private static readonly string _string = typeof(string).FullName;
    private static readonly string _float = typeof(float).FullName;
    private static readonly string _double = typeof(double).FullName;
    private static readonly string _date = typeof(DateOnly).FullName;
    private static readonly string _datetime = typeof(DateTime).FullName;
    private static readonly string _time = typeof(TimeOnly).FullName;
    private static readonly List<string> _validTypes = new()
    {
        _bool,
        _decimal,
        _int,
        _uint,
        _long,
        _ulong,
        _char,
        _string,
        _float,
        _double,
        _date,
        _datetime,
        _time
    };

    public PagingProp(
        int size,
        int page,
        List<string> filterColumns,
        List<object> filterValues,
        List<FilterProperty> filterProperties,
        FilterCondition filterCondition,
        string sortColumn,
        string sortDirection,
        string defaultSortColumn,
        string defaultSortDirection,
        int maxSize = 100)
    {

        ValidateSearchFilter(filterColumns, filterValues, filterProperties);

        if (maxSize <= 0)
        {
            maxSize = 1;
        }

        Page = GetPage(page);
        Size = GetPageSize(size, maxSize);
        SortDirection = GetSortDirection(sortDirection, defaultSortDirection);
        SortColumn = GetSortColumn(sortColumn, defaultSortColumn);
        SearchExpression = GetSearchExpression(filterColumns, filterValues, filterProperties, filterCondition);
    }

    public int Page { get; private set; }

    public int Size { get; private set; }

    public string SortDirection { get; private set; }

    public string SortColumn { get; private set; }

    public Expression<Func<TModel, bool>> SearchExpression { get; private set; }

    private static string GetSortDirection(string sortDirection, string defaultSortDirection)
    {
        if (!string.IsNullOrEmpty(sortDirection) && (sortDirection == "asc" || sortDirection == "desc"))
        {
            return sortDirection;
        }

        if (!string.IsNullOrEmpty(defaultSortDirection) && (defaultSortDirection == "asc" || defaultSortDirection == "desc"))
        {
            return defaultSortDirection;
        }

        return "asc";
    }

    private static int GetPage(int page)
    {
        if (page <= 0)
        {
            page = 1;
        }

        return page;
    }

    private static int GetPageSize(int pageSize, int maxSize)
    {
        if (pageSize > maxSize)
        {
            throw new ServerException($"You can only request at most {maxSize} objects in one request.");
        }
        else if (pageSize <= 0)
        {
            pageSize = maxSize;
        }

        return pageSize;
    }

    private static PropertyInfo GetPropertyInfo(string propertyName)
    {
        PropertyInfo[] properties = typeof(TModel).GetProperties();

        PropertyInfo propertyInfo = properties.FirstOrDefault(p => p.Name.ToLower() == propertyName.ToLower());

        return propertyInfo;
    }

    private static string GetProperty(string propertyName, string type, bool throwException = true)
    {
        if (!string.IsNullOrEmpty(propertyName))
        {
            PropertyInfo propertyInfo = GetPropertyInfo(propertyName);

            if (propertyInfo != null)
            {
                return propertyInfo.Name;
            }

            if (throwException)
            {
                throw new ServerException($"Invalid {type} property name '{propertyName}'");
            }
        }

        return string.Empty;
    }

    private static string GetSortColumn(string propertyName, string defaultProperty)
    {
        string property = GetProperty(propertyName, "sort", false);

        if (string.IsNullOrEmpty(property))
        {
            property = GetProperty(defaultProperty, "sort");
        }

        return property;
    }

    private static object GetPropertyValue(string propertyName, object value, string type)
    {
        if (!string.IsNullOrEmpty(propertyName) && value != null)
        {
            PropertyInfo propertyInfo = GetPropertyInfo(propertyName);

            if (propertyInfo != null)
            {
                try
                {
                    object convertedValue = Convert.ChangeType(value, propertyInfo.PropertyType);
                    return convertedValue;
                }
                catch
                {
                    throw new ServerException($"Invalid ${type} property value '{value}' for property '{propertyName}'");
                }
            }

            throw new ServerException($"Invalid ${type} property name '{propertyName}'");
        }

        return null;
    }

    private static readonly List<PropertyTypeOperator> _propertyTypeOperators = new()
    {
        new(_bool, new() {
            FilterOperator.Equal
        }),
        new(_decimal, new() {
            FilterOperator.Equal,
            FilterOperator.NotEqual,
            FilterOperator.GreaterThan,
            FilterOperator.GreaterThanOrEqual,
            FilterOperator.LessThan,
            FilterOperator.LessThanOrEqual
        }),
        new(_int, new() {
            FilterOperator.Equal,
            FilterOperator.NotEqual,
            FilterOperator.GreaterThan,
            FilterOperator.GreaterThanOrEqual,
            FilterOperator.LessThan,
            FilterOperator.LessThanOrEqual
        }),
        new(_uint, new() {
            FilterOperator.Equal,
            FilterOperator.NotEqual,
            FilterOperator.GreaterThan,
            FilterOperator.GreaterThanOrEqual,
            FilterOperator.LessThan,
            FilterOperator.LessThanOrEqual
        }),
        new(_long, new() {
            FilterOperator.Equal,
            FilterOperator.NotEqual,
            FilterOperator.GreaterThan,
            FilterOperator.GreaterThanOrEqual,
            FilterOperator.LessThan,
            FilterOperator.LessThanOrEqual
        }),
        new(_ulong, new() {
            FilterOperator.Equal,
            FilterOperator.NotEqual,
            FilterOperator.GreaterThan,
            FilterOperator.GreaterThanOrEqual,
            FilterOperator.LessThan,
            FilterOperator.LessThanOrEqual
        }),
        new(_char, new() {
            FilterOperator.Equal,
            FilterOperator.NotEqual,
            FilterOperator.Contains,
            FilterOperator.StartsWith,
            FilterOperator.EndsWith
        }),
        new(_string, new() {
            FilterOperator.Equal,
            FilterOperator.NotEqual,
            FilterOperator.Contains,
            FilterOperator.StartsWith,
            FilterOperator.EndsWith
        }),
        new(_float, new() {
            FilterOperator.Equal,
            FilterOperator.NotEqual,
            FilterOperator.GreaterThan,
            FilterOperator.GreaterThanOrEqual,
            FilterOperator.LessThan,
            FilterOperator.LessThanOrEqual
        }),
        new(_double, new() {
            FilterOperator.Equal,
            FilterOperator.NotEqual,
            FilterOperator.GreaterThan,
            FilterOperator.GreaterThanOrEqual,
            FilterOperator.LessThan,
            FilterOperator.LessThanOrEqual
        }),
        new(_date, new() {
            FilterOperator.Equal,
            FilterOperator.NotEqual,
            FilterOperator.GreaterThan,
            FilterOperator.GreaterThanOrEqual,
            FilterOperator.LessThan,
            FilterOperator.LessThanOrEqual
        }),
        new(_datetime, new() {
            FilterOperator.Equal,
            FilterOperator.NotEqual,
            FilterOperator.GreaterThan,
            FilterOperator.GreaterThanOrEqual,
            FilterOperator.LessThan,
            FilterOperator.LessThanOrEqual
        }),
        new(_time, new() {
            FilterOperator.Equal,
            FilterOperator.NotEqual,
            FilterOperator.GreaterThan,
            FilterOperator.GreaterThanOrEqual,
            FilterOperator.LessThan,
            FilterOperator.LessThanOrEqual
        })
    };

    private static void ValidateSearchFilter(List<string> filterColumns, List<object> filterValues, List<FilterProperty> filterProperties)
    {
        //if there is any filter column or values, let's make sure all are valid
        if (filterColumns.Any() || filterValues.Any())
        {
            string error = string.Empty;

            //each column must match value
            if (filterColumns.Count == filterValues.Count)
            {
                List<string> columns = filterProperties.Select(e => e.Name.ToLower()).ToList();
                List<string> invalidColumns = filterColumns.Where(e => !columns.Contains(e.ToLower())).ToList();

                //check if there is an invalid column
                if (!invalidColumns.Any())
                {
                    //process each column
                    for (int i = 0; i < filterColumns.Count; i++)
                    {
                        string column = filterColumns[i];
                        object value = filterValues[i];

                        //get the column from the filter properties
                        FilterProperty filterProperty = filterProperties.Where(e => e.Name.ToLower() == column.ToLower()).FirstOrDefault();

                        //check if the type of the column is supported
                        if (_validTypes.Contains(filterProperty.Type))
                        {
                            //validate if valid value (skip for string as everything is valid for string)
                            if (filterProperty.Type != _string)
                            {
                                //check if the column type is a valid type
                                Type propertyType = Type.GetType(filterProperty.Type);

                                if (propertyType != null)
                                {
                                    //validate if the value is valid for the type
                                    try
                                    {
                                        Convert.ChangeType(value, propertyType);
                                    }
                                    catch (Exception)
                                    {
                                        error = $"Invalid value detected for column '{column}'. Value : {value}";
                                        break;
                                    }
                                }
                                else
                                {
                                    error = $"Invalid column type detected for column '{column}'. Value : {filterProperty.Type}";
                                    break;
                                }
                            }

                            //validate if operators match the configuration
                            List<FilterOperator> operators = new()
                            {
                                filterProperty.FirstOperator
                            };

                            if (filterProperty.SecondOperator != null)
                            {
                                operators.Add(filterProperty.SecondOperator.Value);
                            }

                            //get the operators based on type
                            PropertyTypeOperator propertyTypeOperator = _propertyTypeOperators.FirstOrDefault(e => e.PropertyType == filterProperty.Type);

                            if (propertyTypeOperator != null)
                            {
                                List<FilterOperator> propertyOperators = propertyTypeOperator.Operators;

                                if (propertyOperators != null && propertyOperators.Any())
                                {
                                    //validate if there is any operator for the column that does not match the configuration
                                    List<FilterOperator> invalidOperators = operators.Where(e => !propertyOperators.Contains(e)).ToList();

                                    if (invalidOperators.Any())
                                    {
                                        error = $"Invalid operator for '{column}'. Value : {string.Join(", ", invalidOperators)}. Supported values are: {string.Join(", ", propertyOperators)}";
                                        break;
                                    }
                                }
                                else
                                {
                                    error = $"No operator support for column '{column}' type. Value : {filterProperty.Type}";
                                    break;
                                }
                            }
                            else
                            {
                                error = $"No operator support for column '{column}' type. Value : {filterProperty.Type}";
                                break;
                            }
                        }
                        else
                        {
                            error = $"Column '{column}' type not supported. Value : {filterProperty.Type}. Supported values are: {string.Join(", ", _validTypes)}";
                            break;
                        }
                    }
                }
                else
                {
                    error = $"Invalid filter column detected: {string.Join(", ", invalidColumns)}";
                }
            }
            else
            {
                error = "Filter columns and filter values must have equal number of items";
            }

            if (!string.IsNullOrEmpty(error))
            {
                throw new ArgumentException(error);
            }
        }
    }

    private static Expression<Func<TModel, bool>> GetSearchExpression(List<string> filterColumns, List<object> filterValues, List<FilterProperty> filterProperties, FilterCondition filterCondition = FilterCondition.And)
    {
        Dictionary<string, object> searchValues = new();

        for (int i = 0; i < filterColumns.Count; i++)
        {
            string column = filterColumns[i];
            object value = filterValues[i];

            searchValues.Add(column, value);
        }

        if (searchValues == null || searchValues.Count == 0)
        {
            return null;
        }

        ParameterExpression parameter = Expression.Parameter(typeof(TModel), "x");
        BinaryExpression condition = null;

        foreach (var kvp in searchValues)
        {
            string searchColumn = kvp.Key;
            object searchValue = kvp.Value;

            object value = GetPropertyValue(searchColumn, searchValue, "search");
            string column = GetProperty(searchColumn, "search");

            if (!string.IsNullOrEmpty(column) && value != null)
            {
                PropertyInfo propertyInfo = GetPropertyInfo(column);

                if (propertyInfo != null)
                {
                    MemberExpression property = Expression.Property(parameter, propertyInfo);
                    ConstantExpression constant = Expression.Constant(value);
                    BinaryExpression firstBinary = null;
                    BinaryExpression secondBinary = null;
                    BinaryExpression binary = null;

                    FilterOperator firstOperator = FilterOperator.Equal;
                    FilterOperator? secondOperator = null;


                    if (filterProperties != null && filterProperties.Any())
                    {
                        FilterProperty filterPropery = filterProperties.Where(e => e.Name.ToLower() == column.ToLower()).FirstOrDefault();

                        if (filterPropery != null)
                        {
                            firstOperator = filterPropery.FirstOperator;
                            secondOperator = filterPropery.SecondOperator;
                        }
                    }

                    switch (firstOperator)
                    {
                        case FilterOperator.Equal:
                            firstBinary = Expression.Equal(property, constant);
                            break;
                        case FilterOperator.NotEqual:
                            firstBinary = Expression.NotEqual(property, constant);
                            break;
                        case FilterOperator.Contains:
                            MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                            Expression containsExpression = Expression.Call(property, containsMethod, constant);
                            firstBinary = Expression.Equal(containsExpression, Expression.Constant(true));
                            break;
                        case FilterOperator.StartsWith:
                            MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                            Expression startsWithExpression = Expression.Call(property, startsWithMethod, constant);
                            firstBinary = Expression.Equal(startsWithExpression, Expression.Constant(true));
                            break;
                        case FilterOperator.EndsWith:
                            MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                            Expression endsWithExpression = Expression.Call(property, endsWithMethod, constant);
                            firstBinary = Expression.Equal(endsWithExpression, Expression.Constant(true));
                            break;
                        case FilterOperator.GreaterThan:
                            firstBinary = Expression.GreaterThan(property, constant);
                            break;
                        case FilterOperator.LessThan:
                            firstBinary = Expression.LessThan(property, constant);
                            break;
                        case FilterOperator.GreaterThanOrEqual:
                            firstBinary = Expression.GreaterThanOrEqual(property, constant);
                            break;
                        case FilterOperator.LessThanOrEqual:
                            firstBinary = Expression.LessThanOrEqual(property, constant);
                            break;
                    }


                    if (secondOperator != null)
                    {
                        switch (secondOperator)
                        {
                            case FilterOperator.Equal:
                                secondBinary = Expression.Equal(property, constant);
                                break;
                            case FilterOperator.NotEqual:
                                secondBinary = Expression.NotEqual(property, constant);
                                break;
                            case FilterOperator.Contains:
                                MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                                Expression containsExpression = Expression.Call(property, containsMethod, constant);
                                secondBinary = Expression.Equal(containsExpression, Expression.Constant(true));
                                break;
                            case FilterOperator.StartsWith:
                                MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                                Expression startsWithExpression = Expression.Call(property, startsWithMethod, constant);
                                secondBinary = Expression.Equal(startsWithExpression, Expression.Constant(true));
                                break;
                            case FilterOperator.EndsWith:
                                MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                                Expression endsWithExpression = Expression.Call(property, endsWithMethod, constant);
                                secondBinary = Expression.Equal(endsWithExpression, Expression.Constant(true));
                                break;
                            case FilterOperator.GreaterThan:
                                secondBinary = Expression.GreaterThan(property, constant);
                                break;
                            case FilterOperator.LessThan:
                                secondBinary = Expression.LessThan(property, constant);
                                break;
                            case FilterOperator.GreaterThanOrEqual:
                                secondBinary = Expression.GreaterThanOrEqual(property, constant);
                                break;
                            case FilterOperator.LessThanOrEqual:
                                secondBinary = Expression.LessThanOrEqual(property, constant);
                                break;
                        }
                    }

                    binary = firstBinary;

                    if (secondBinary != null)
                    {
                        binary = Expression.AndAlso(firstBinary, secondBinary);
                    }

                    if (condition == null)
                    {
                        condition = binary;
                    }
                    else
                    {
                        if (filterCondition == FilterCondition.Or)
                        {
                            condition = Expression.OrElse(condition, binary);
                        }
                        else
                        {
                            condition = Expression.AndAlso(condition, binary);
                        }
                    }
                }
            }
        }

        if (condition != null)
        {
            Expression<Func<TModel, bool>> lambda = Expression.Lambda<Func<TModel, bool>>(condition, parameter);
            return lambda;
        }

        return null;
    }
}
