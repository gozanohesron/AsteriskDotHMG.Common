namespace AsteriskDotHMG.Paging.Models;

public class PagingParam
{
    public int Page { get; set; }

    public int Size { get; set; }

    public string SearchColumn { get; set; }

    public object SearchValue { get; set; }

    public string SortDirection { get; set; }

    public string SortColumn { get; set; }
}
