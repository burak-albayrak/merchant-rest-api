namespace Merchant.V1.Models.RequestModels;

public class PaginationRequestModel
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}