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
