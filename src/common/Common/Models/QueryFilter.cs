namespace AsteriskDotHMG.Common.Models;

public class QueryFilter
{
    public Dictionary<string, (QueryFilterOperator Operator, object Value)> Filters { get; set; }
    public QueryFilterCondition Condition { get; set; } = QueryFilterCondition.And;
}
