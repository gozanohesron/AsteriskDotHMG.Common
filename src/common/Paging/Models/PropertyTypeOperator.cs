namespace AsteriskDotHMG.Paging.Models;

public class PropertyTypeOperator
{
    public PropertyTypeOperator()
    {

    }
    public PropertyTypeOperator(string propertyType, List<FilterOperator> operators)
    {
        PropertyType = propertyType;
        Operators = operators;
    }

    public string PropertyType { get; set; }

    public List<FilterOperator> Operators = new();
}
