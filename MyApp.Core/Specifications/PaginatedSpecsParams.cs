namespace MyApp.Domain.Specifications;

public class PaginatedSpecsParams
{
    /// <summary>
    /// pagination part 
    /// </summary>

    private int _pageIndex = 1;
    public int PageIndex
    {
        get { return _pageIndex; }
        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException("page index start with 1 ");
            else
                _pageIndex = value;
        }
    }
    private const int MaxPageSize = 70;
    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
    /// <summary>
    /// filter part 
    /// </summary>
    public string Search { get; set; } = string.Empty;


}
