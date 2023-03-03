namespace AsteriskDotHMG.Common.Models;

public class PagingProp<TModel>
{
    public PagingProp(
        int size,
        int page,         
        string searchColumn, 
        object searchValue, 
        string sortColumn, 
        string sortDirection, 
        string defaultSortColumn, 
        string defaultSortDirection,
        int maxSize = 100)
    {

        if (maxSize <= 0)
        {
            maxSize = 1;
        }

        Page = GetPage(page);
        Size = GetPageSize(size, maxSize);
        SortDirection = GetSortDirection(sortDirection, defaultSortDirection);
        SortColumn = GetSortColumn(sortColumn, defaultSortColumn);
        SearchExpression = GetSearchExpression(searchColumn, searchValue);
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
                    return value;
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

    private static Expression<Func<TModel, bool>> GetSearchExpression(string searchColumn, object searchValue)
    {
        object value = GetPropertyValue(searchColumn, searchValue, "search");
        string column = GetProperty(searchColumn, "search");

        if (!string.IsNullOrEmpty(column) && value != null)
        {

            PropertyInfo propertyInfo = GetPropertyInfo(column);

            if (propertyInfo != null)
            {
                ParameterExpression parameter = Expression.Parameter(typeof(TModel), "x");
                MemberExpression property = Expression.Property(parameter, propertyInfo);
                ConstantExpression constant = Expression.Constant(searchValue);
                BinaryExpression binary = Expression.Equal(property, constant);
                Expression<Func<TModel, bool>> lambda = Expression.Lambda<Func<TModel, bool>>(binary, parameter);
                return lambda;
            }
        }

        return null;
    }

}
