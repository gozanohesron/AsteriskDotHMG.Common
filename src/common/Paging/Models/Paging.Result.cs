namespace AsteriskDotHMG.Common.Models;

public class PagingResult<TModel>
{
    [SwaggerSchema("Total number of records")]
    public long TotalItems { get; set; }

    [SwaggerSchema("Total number of pages")]
    public int TotalPages { get; set; }

    [SwaggerSchema("Current page records")]
    public List<TModel> Records { get; set; } = new List<TModel>();
}

public class PagingResult<TModel, TData>: PagingResult<TModel>
{

    [SwaggerSchema("Extra data for configuration")]
    public TData Data { get; set; }
}