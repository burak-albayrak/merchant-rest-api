namespace Merchant.V1.Helpers;

public class PaginatedList<T>
{
    public List<T> Items { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalItemCount { get; }
    public int TotalPageCount { get; }

    public PaginatedList(List<T> items, int pageNumber, int pageSize, int totalItemCount)
    {
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalItemCount = totalItemCount;
        TotalPageCount = (int)Math.Ceiling((double)totalItemCount / pageSize);
    }

    public static PaginatedList<T> Create(List<T> source, int pageNumber, int pageSize)
    {
        var totalItemCount = source.Count;
        var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return new PaginatedList<T>(items, pageNumber, pageSize, totalItemCount);
    }
}
