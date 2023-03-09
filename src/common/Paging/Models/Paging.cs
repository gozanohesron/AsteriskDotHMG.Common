namespace AsteriskDotHMG.Paging.Models;

public class PagingParam
{
    public int Page { get; set; }

    public int Size { get; set; }

    public List<string> FilterColumns { get; set; } = new();

    public List<object> FilterValues { get; set; } = new();

    public string SortDirection { get; set; }

    public string SortColumn { get; set; }
}
