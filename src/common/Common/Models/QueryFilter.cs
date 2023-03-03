namespace AsteriskDotHMG.Common.Models;

public class QueryFilter
{
    public QueryFilter()
    {
    }

    public QueryFilter(Dictionary<string, (QueryFilterOperator Operator, object Value)> filters, 
        QueryFilterCondition condition = QueryFilterCondition.And)
    {
        Filters = filters;
        Condition = condition;
    }

    public Dictionary<string, (QueryFilterOperator Operator, object Value)> Filters { get; set; }

    public QueryFilterCondition Condition { get; set; } = QueryFilterCondition.And;
}
