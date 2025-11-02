namespace Tekus.Core.Domain.Common;

/// <summary>
/// Represents a paginated result set
/// </summary>
/// <typeparam name="T">Type of items in the result</typeparam>
public class PagedResult<T>
{
    /// <summary>
    /// Items in the current page
    /// </summary>
    public IReadOnlyList<T> Items { get; }

    /// <summary>
    /// Total number of items across all pages
    /// </summary>
    public int TotalCount { get; }

    /// <summary>
    /// Current page number (1-based)
    /// </summary>
    public int PageNumber { get; }

    /// <summary>
    /// Number of items per page
    /// </summary>
    public int PageSize { get; }

    /// <summary>
    /// Total number of pages
    /// </summary>
    public int TotalPages { get; }

    /// <summary>
    /// Indicates if there is a previous page
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>
    /// Indicates if there is a next page
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;

    public PagedResult(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items.ToList().AsReadOnly();
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = totalCount > 0 ? (int)Math.Ceiling(totalCount / (double)pageSize) : 0;
    }

    /// <summary>
    /// Creates an empty paged result
    /// </summary>
    public static PagedResult<T> Empty(int pageNumber = 1, int pageSize = 10)
    {
        return new PagedResult<T>(Array.Empty<T>(), 0, pageNumber, pageSize);
    }
}