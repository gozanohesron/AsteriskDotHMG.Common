namespace AsteriskDotHMG.Paging.Helpers;

public class Pagination<T>
{
    public Pagination(IEnumerable<T> results, long totalItems, int page = 1, int limit = 10, bool applyPageAndLimitToResults = false)
    {
        if (limit <= 0)
        {
            throw new PaginationException("Limit must be greater than 0");
        }
        if (page <= 0)
        {
            throw new PaginationException("Page must be greater than 0");
        }
        //if (totalItems <= 0)
        //{
        //    throw new PaginationException("TotalItems must be greater than 0");
        //}

        var startIndex = (page - 1) * limit;
        var endIndex = page * limit;

        TotalItems = totalItems;
        CurrentPage = page;

        if (totalItems > 0)
        {
            Results = results ?? Enumerable.Empty<T>();
            if (applyPageAndLimitToResults)
            {
                Results = Results.Skip(startIndex).Take(limit);
            }
            if (startIndex > 0)
            {
                PreviousPage = page - 1;
            }
            if (endIndex < totalItems)
            {
                NextPage = page + 1;
            }

            TotalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)limit);
        }
        else
        {
            Results = Enumerable.Empty<T>();
            PreviousPage = 0;
            NextPage = 0;
            TotalPages = 0;
        }       
    }

    public long TotalItems { get; private set; }
    public int CurrentPage { get; private set; }
    public int? NextPage { get; private set; }
    public int? PreviousPage { get; private set; }
    public int TotalPages { get; private set; }
    public IEnumerable<T> Results { get; private set; }
}