namespace AsteriskDotHMG.Common.Models;

public class QueryFilter
{
    public QueryFilter()
    {
    }

    public QueryFilter(Dictionary<string, (FilterOperator Operator, object Value)> filters, 
        FilterCondition condition = FilterCondition.And)
    {
        Filters = filters;
        Condition = condition;
    }

    public Dictionary<string, (FilterOperator Operator, object Value)> Filters { get; set; }

    public FilterCondition Condition { get; set; } = FilterCondition.And;
}
