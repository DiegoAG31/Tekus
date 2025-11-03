namespace Tekus.Core.Domain.Common;

/// <summary>
/// Parameters for pagination, sorting and searching
/// </summary>
public class PaginationParams
{
    private const int MaxPageSize = 100;
    private const int DefaultPageSize = 10;
    private const int MinPageNumber = 1;
    private const int MinPageSize = 1;

    private int _pageNumber = 1;
    private int _pageSize = DefaultPageSize;

    /// <summary>
    /// Current page number (1-based, minimum 1)
    /// </summary>
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < MinPageNumber ? MinPageNumber : value;
    }

    /// <summary>
    /// Number of items per page (1-100, default 10)
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set
        {
            if (value < MinPageSize)
                _pageSize = MinPageSize;
            else if (value > MaxPageSize)
                _pageSize = MaxPageSize;
            else
                _pageSize = value;
        }
    }

    /// <summary>
    /// Search term to filter results (optional)
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Property name to sort by (optional)
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// Sort in descending order (default: false = ascending)
    /// </summary>
    public bool SortDescending { get; set; }

    /// <summary>
    /// Calculates the number of items to skip
    /// </summary>
    public int Skip => (PageNumber - 1) * PageSize;

    /// <summary>
    /// Number of items to take
    /// </summary>
    public int Take => PageSize;
}