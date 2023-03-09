namespace AsteriskDotHMG.Paging.Models;

public class FilterProperty
{
    public FilterProperty()
    {

    }

    public FilterProperty(string name, string type, FilterOperator firstOperator = FilterOperator.Equal, FilterOperator? secondOperator = null)
    {
        Name = name;
        Type = type;
        FirstOperator = firstOperator;
        SecondOperator = secondOperator;
    }

    public string Name { get; set; }

    public string Type { get; set; }

    public FilterOperator FirstOperator { get; set; }

    public FilterOperator? SecondOperator { get; set; }
}
