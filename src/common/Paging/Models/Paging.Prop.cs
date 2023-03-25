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
    private static readonly string _boolNull = typeof(bool?).FullName;
    private static readonly string _decimalNull = typeof(decimal?).FullName;
    private static readonly string _intNull = typeof(int?).FullName;
    private static readonly string _uintNull = typeof(uint?).FullName;
    private static readonly string _longNull = typeof(long?).FullName;
    private static readonly string _ulongNull = typeof(ulong?).FullName;
    private static readonly string _charNull = typeof(char?).FullName;
    private static readonly string _floatNull = typeof(float?).FullName;
    private static readonly string _doubleNull = typeof(double?).FullName;
    private static readonly string _dateNull = typeof(DateOnly?).FullName;
    private static readonly string _datetimeNull = typeof(DateTime?).FullName;
    private static readonly string _timeNull = typeof(TimeOnly?).FullName;
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
        _time,
        _boolNull,
        _decimalNull,
        _intNull,
        _uintNull,
        _longNull,
        _ulongNull,
        _charNull,
        _floatNull,
        _doubleNull,
        _dateNull,
        _datetimeNull,
        _timeNull
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
            throw new PaginationException($"You can only request at most {maxSize} objects in one request.");
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

        PropertyInfo nullablePropertyInfo = properties.FirstOrDefault(p => p.Name.ToLower() == propertyName.ToLower());

        return nullablePropertyInfo;
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
                throw new PaginationException($"Invalid {type} property name '{propertyName}'");
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
                    Type underlyingType = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
                    object convertedValue = Convert.ChangeType(value, underlyingType ?? propertyInfo.PropertyType);
                    return convertedValue;
                }
                catch
                {
                    throw new PaginationException($"Invalid {type} property value '{value}' for property '{propertyName}'");
                }
            }

            throw new PaginationException($"Invalid {type} property name '{propertyName}'");
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
        }),
        new(_boolNull, new() {
            FilterOperator.Equal
        }),
        new(_decimalNull, new() {
            FilterOperator.Equal,
            FilterOperator.NotEqual,
            FilterOperator.GreaterThan,
            FilterOperator.GreaterThanOrEqual,
            FilterOperator.LessThan,
            FilterOperator.LessThanOrEqual
        }),
        new(_intNull, new() {
            FilterOperator.Equal,
            FilterOperator.NotEqual,
            FilterOperator.GreaterThan,
            FilterOperator.GreaterThanOrEqual,
            FilterOperator.LessThan,
            FilterOperator.LessThanOrEqual
        }),
        new(_uintNull, new() {
            FilterOperator.Equal,
            FilterOperator.NotEqual,
            FilterOperator.GreaterThan,
            FilterOperator.GreaterThanOrEqual,
            FilterOperator.LessThan,
            FilterOperator.LessThanOrEqual
        }),
        new(_longNull, new() {
            FilterOperator.Equal,
            FilterOperator.NotEqual,
            FilterOperator.GreaterThan,
            FilterOperator.GreaterThanOrEqual,
            FilterOperator.LessThan,
            FilterOperator.LessThanOrEqual
        }),
        new(_ulongNull, new() {
            FilterOperator.Equal,
            FilterOperator.NotEqual,
            FilterOperator.GreaterThan,
            FilterOperator.GreaterThanOrEqual,
            FilterOperator.LessThan,
            FilterOperator.LessThanOrEqual
        }),
        new(_charNull, new() {
            FilterOperator.Equal,
            FilterOperator.NotEqual,
            FilterOperator.Contains,
            FilterOperator.StartsWith,
            FilterOperator.EndsWith
        }),
        new(_floatNull, new() {
            FilterOperator.Equal,
            FilterOperator.NotEqual,
            FilterOperator.GreaterThan,
            FilterOperator.GreaterThanOrEqual,
            FilterOperator.LessThan,
            FilterOperator.LessThanOrEqual
        }),
        new(_doubleNull, new() {
            FilterOperator.Equal,
            FilterOperator.NotEqual,
            FilterOperator.GreaterThan,
            FilterOperator.GreaterThanOrEqual,
            FilterOperator.LessThan,
            FilterOperator.LessThanOrEqual
        }),
        new(_dateNull, new() {
            FilterOperator.Equal,
            FilterOperator.NotEqual,
            FilterOperator.GreaterThan,
            FilterOperator.GreaterThanOrEqual,
            FilterOperator.LessThan,
            FilterOperator.LessThanOrEqual
        }),
        new(_datetimeNull, new() {
            FilterOperator.Equal,
            FilterOperator.NotEqual,
            FilterOperator.GreaterThan,
            FilterOperator.GreaterThanOrEqual,
            FilterOperator.LessThan,
            FilterOperator.LessThanOrEqual
        }),
        new(_timeNull, new() {
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
                                        Type underlyingType = Nullable.GetUnderlyingType(propertyType);

                                        Convert.ChangeType(value, underlyingType ?? propertyType);
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
                throw new PaginationException(error);
            }
        }
    }

    private class FilterColumnValuePair
    {
        public FilterColumnValuePair(string column, object value)
        {
            Column = column;
            Value = value;
        }

        public string Column { get; set; }

        public object Value { get; set; }
    }

    private static Expression<Func<TModel, bool>> GetSearchExpression(List<string> filterColumns, List<object> filterValues, List<FilterProperty> filterProperties, FilterCondition filterCondition = FilterCondition.And)
    {
        List<FilterColumnValuePair> searchValues = new();

        for (int i = 0; i < filterColumns.Count; i++)
        {
            string column = filterColumns[i];
            object value = filterValues[i];

            searchValues.Add(new(column, value));
        }

        if (searchValues == null || searchValues.Count == 0)
        {
            return null;
        }

        ParameterExpression parameter = Expression.Parameter(typeof(TModel), "x");
        BinaryExpression condition = null;
        List<string> processedProperty = new();

        foreach (var kvp in searchValues)
        {
            string searchColumn = kvp.Column;
            object searchValue = kvp.Value;

            object value = GetPropertyValue(searchColumn, searchValue, "search");
            string column = GetProperty(searchColumn, "search");

            if (!string.IsNullOrEmpty(column) && value != null)
            {
                PropertyInfo propertyInfo = GetPropertyInfo(column);

                if ((propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateOnly)))
                {
                    value = Convert.ToDateTime(value).Date;
                }

                if (propertyInfo != null)
                {
                    MemberExpression property = Expression.Property(parameter, propertyInfo);
                    ConstantExpression constant = Expression.Constant(value);

                    Type underlyingType = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
                    Expression coalescedProperty = null;

                    if (underlyingType != null)
                    {
                        object coalescedConstant = 0.0;

                        if (underlyingType.FullName == _bool)
                        {
                            coalescedConstant = false;
                        }
                        else if (underlyingType.FullName == _decimal)
                        {
                            coalescedConstant = 0M;
                        }
                        else if (underlyingType.FullName == _int)
                        {
                            coalescedConstant = 0;
                        }
                        else if (underlyingType.FullName == _uint)
                        {
                            coalescedConstant = 0U;
                        }
                        else if (underlyingType.FullName == _long)
                        {
                            coalescedConstant = 0L;
                        }
                        else if (underlyingType.FullName == _ulong)
                        {
                            coalescedConstant = 0UL;
                        }
                        else if (underlyingType.FullName == _char)
                        {
                            coalescedConstant = '\0';
                        }
                        else if (underlyingType.FullName == _string)
                        {
                            coalescedConstant = null;
                        }
                        else if (underlyingType.FullName == _float)
                        {
                            coalescedConstant = 0.0f;
                        }
                        else if (underlyingType.FullName == _double)
                        {
                            coalescedConstant = 0.0d;
                        }
                        else if (underlyingType.FullName == _date)
                        {
                            coalescedConstant = default(DateOnly);
                        }
                        else if (underlyingType.FullName == _datetime)
                        {
                            coalescedConstant = default(DateTime);
                        }
                        else if (underlyingType.FullName == _time)
                        {
                            coalescedConstant = default(TimeOnly);
                        }

                        coalescedProperty = Expression.Coalesce(property, Expression.Constant(coalescedConstant));
                    }

                    BinaryExpression firstBinary = null;
                    BinaryExpression secondBinary = null;
                    BinaryExpression binary = null;

                    FilterOperator firstOperator = FilterOperator.Equal;
                    FilterOperator? secondOperator = null;

                    if (filterProperties != null && filterProperties.Any())
                    {
                        FilterProperty filterPropery = filterProperties.Where(e => e.Name.ToLower() == column.ToLower()).FirstOrDefault();

                        if (processedProperty.Contains(column))
                        {
                            filterPropery = filterProperties.Where(e => e.Name.ToLower() == column.ToLower()).LastOrDefault();
                        }

                        if (filterPropery != null)
                        {
                            firstOperator = filterPropery.FirstOperator;
                            secondOperator = filterPropery.SecondOperator;
                        }
                    }

                    switch (firstOperator)
                    {
                        case FilterOperator.Equal:
                            if (underlyingType != null)
                            {
                                firstBinary = Expression.Equal(coalescedProperty, constant);
                            }
                            else
                            {
                                firstBinary = Expression.Equal(property, constant);
                            }
                            break;
                        case FilterOperator.NotEqual:
                            if (underlyingType != null)
                            {
                                firstBinary = Expression.NotEqual(coalescedProperty, constant);
                            }
                            else
                            {
                                firstBinary = Expression.NotEqual(property, constant);
                            }
                            firstBinary = Expression.NotEqual(property, constant);
                            break;
                        case FilterOperator.Contains:
                            if (underlyingType != null)
                            {
                                MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                                Expression containsExpression = Expression.Call(coalescedProperty, containsMethod, constant);
                                firstBinary = Expression.Equal(containsExpression, Expression.Constant(true));
                            }
                            else
                            {
                                MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                                Expression containsExpression = Expression.Call(property, containsMethod, constant);
                                firstBinary = Expression.Equal(containsExpression, Expression.Constant(true));
                            }
                            break;
                        case FilterOperator.StartsWith:
                            if (underlyingType != null)
                            {
                                MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                                Expression startsWithExpression = Expression.Call(coalescedProperty, startsWithMethod, constant);
                                firstBinary = Expression.Equal(startsWithExpression, Expression.Constant(true));
                            }
                            else
                            {
                                MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                                Expression startsWithExpression = Expression.Call(property, startsWithMethod, constant);
                                firstBinary = Expression.Equal(startsWithExpression, Expression.Constant(true));
                            }
                            break;
                        case FilterOperator.EndsWith:
                            if (underlyingType != null)
                            {
                                MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                                Expression endsWithExpression = Expression.Call(coalescedProperty, endsWithMethod, constant);
                                firstBinary = Expression.Equal(endsWithExpression, Expression.Constant(true));
                            }
                            else
                            {
                                MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                                Expression endsWithExpression = Expression.Call(property, endsWithMethod, constant);
                                firstBinary = Expression.Equal(endsWithExpression, Expression.Constant(true));
                            }
                            break;
                        case FilterOperator.GreaterThan:
                            if (underlyingType != null)
                            {
                                firstBinary = Expression.GreaterThan(coalescedProperty, constant);
                            }
                            else
                            {
                                firstBinary = Expression.GreaterThan(property, constant);
                            }
                            break;
                        case FilterOperator.LessThan:
                            if (underlyingType != null)
                            {
                                firstBinary = Expression.LessThan(coalescedProperty, constant);
                            }
                            else
                            {
                                firstBinary = Expression.LessThan(property, constant);
                            }
                            break;
                        case FilterOperator.GreaterThanOrEqual:
                            if (underlyingType != null)
                            {
                                firstBinary = Expression.GreaterThanOrEqual(coalescedProperty, constant);
                            }
                            else
                            {
                                firstBinary = Expression.GreaterThanOrEqual(property, constant);
                            }
                            break;
                        case FilterOperator.LessThanOrEqual:
                            if (underlyingType != null)
                            {
                                firstBinary = Expression.LessThanOrEqual(coalescedProperty, constant);
                            }
                            else
                            {
                                firstBinary = Expression.LessThanOrEqual(property, constant);
                            }
                            break;
                    }


                    if (secondOperator != null)
                    {
                        switch (secondOperator)
                        {
                            case FilterOperator.Equal:
                                if (underlyingType != null)
                                {
                                    secondBinary = Expression.Equal(coalescedProperty, constant);
                                }
                                else
                                {
                                    secondBinary = Expression.Equal(property, constant);
                                }
                                break;
                            case FilterOperator.NotEqual:
                                if (underlyingType != null)
                                {
                                    secondBinary = Expression.NotEqual(coalescedProperty, constant);
                                }
                                else
                                {
                                    secondBinary = Expression.NotEqual(property, constant);
                                }
                                secondBinary = Expression.NotEqual(property, constant);
                                break;
                            case FilterOperator.Contains:
                                if (underlyingType != null)
                                {
                                    MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                                    Expression containsExpression = Expression.Call(coalescedProperty, containsMethod, constant);
                                    secondBinary = Expression.Equal(containsExpression, Expression.Constant(true));
                                }
                                else
                                {
                                    MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                                    Expression containsExpression = Expression.Call(property, containsMethod, constant);
                                    secondBinary = Expression.Equal(containsExpression, Expression.Constant(true));
                                }
                                break;
                            case FilterOperator.StartsWith:
                                if (underlyingType != null)
                                {
                                    MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                                    Expression startsWithExpression = Expression.Call(coalescedProperty, startsWithMethod, constant);
                                    secondBinary = Expression.Equal(startsWithExpression, Expression.Constant(true));
                                }
                                else
                                {
                                    MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                                    Expression startsWithExpression = Expression.Call(property, startsWithMethod, constant);
                                    secondBinary = Expression.Equal(startsWithExpression, Expression.Constant(true));
                                }
                                break;
                            case FilterOperator.EndsWith:
                                if (underlyingType != null)
                                {
                                    MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                                    Expression endsWithExpression = Expression.Call(coalescedProperty, endsWithMethod, constant);
                                    secondBinary = Expression.Equal(endsWithExpression, Expression.Constant(true));
                                }
                                else
                                {
                                    MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                                    Expression endsWithExpression = Expression.Call(property, endsWithMethod, constant);
                                    secondBinary = Expression.Equal(endsWithExpression, Expression.Constant(true));
                                }
                                break;
                            case FilterOperator.GreaterThan:
                                if (underlyingType != null)
                                {
                                    secondBinary = Expression.GreaterThan(coalescedProperty, constant);
                                }
                                else
                                {
                                    secondBinary = Expression.GreaterThan(property, constant);
                                }
                                break;
                            case FilterOperator.LessThan:
                                if (underlyingType != null)
                                {
                                    secondBinary = Expression.LessThan(coalescedProperty, constant);
                                }
                                else
                                {
                                    secondBinary = Expression.LessThan(property, constant);
                                }
                                break;
                            case FilterOperator.GreaterThanOrEqual:
                                if (underlyingType != null)
                                {
                                    secondBinary = Expression.GreaterThanOrEqual(coalescedProperty, constant);
                                }
                                else
                                {
                                    secondBinary = Expression.GreaterThanOrEqual(property, constant);
                                }
                                break;
                            case FilterOperator.LessThanOrEqual:
                                if (underlyingType != null)
                                {
                                    secondBinary = Expression.LessThanOrEqual(coalescedProperty, constant);
                                }
                                else
                                {
                                    secondBinary = Expression.LessThanOrEqual(property, constant);
                                }
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

            processedProperty.Add(column);
        }

        if (condition != null)
        {
            Expression<Func<TModel, bool>> lambda = Expression.Lambda<Func<TModel, bool>>(condition, parameter);
            return lambda;
        }

        return null;
    }
}
