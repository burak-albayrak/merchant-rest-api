namespace Merchant.V1.Models.RequestModels;

public class SortingRequestModel
{
    public string SortBy { get; set; } = "name";
    public string SortOrder { get; set; } = "asc";
}