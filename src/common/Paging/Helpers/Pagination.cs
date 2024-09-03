namespace AsteriskDotHMG.Paging.Helpers;

public class Pagination<T>
{
    public Pagination()
    {
        Results = new List<T>();
        CurrentPage = 1;
    }

    public Pagination(Pagination<T> pagination)
    {
        TotalItems = pagination.TotalPages;
        CurrentPage = pagination.CurrentPage;
        NextPage = pagination.NextPage;
        PreviousPage = pagination.PreviousPage;
        TotalPages = pagination.TotalPages;
        Results = pagination.Results;
    }

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

    public static Pagination<TDestination> GetPagination<TSource, TDestination>(IEnumerable<TSource> results, long totalItems, Func<TSource, TDestination> convertTSourceToTDestinationMethod, int page = 1, int limit = 10, bool applyPageAndLimitToResults = false)
    {
        var destinationResults = results?.Select(x => convertTSourceToTDestinationMethod(x)) ?? new List<TDestination>();
        return new Pagination<TDestination>(destinationResults, totalItems, page, limit, applyPageAndLimitToResults);
    }

    public static async Task<Pagination<TDestination>> GetPaginationAsync<TSource, TDestination>(IEnumerable<TSource> results, long totalItems, Func<TSource, Task<TDestination>> convertTSourceToTDestinationMethod, int page = 1, int limit = 10, bool applyPageAndLimitToResults = false)
    {
        if (results == null)
        {
            return new Pagination<TDestination>(new List<TDestination>(), totalItems, page, limit, applyPageAndLimitToResults);
        }
        var destinationResults = await results.Select(async ev => await convertTSourceToTDestinationMethod(ev)).WhenAll().ConfigureAwait(false);
        return new Pagination<TDestination>(destinationResults, totalItems, page, limit, applyPageAndLimitToResults);
    }

    public long TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int? NextPage { get; set; }
    public int? PreviousPage { get; set; }
    public int TotalPages { get; set; }
    public IEnumerable<T> Results { get; set; }

}