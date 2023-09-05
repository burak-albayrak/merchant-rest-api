namespace Merchant.V1.Models.RequestModels;

public class FilterRequestModel
{
    public string ReviewStarRange { get; set; } = "0,5";
    public string City { get; set; } = "Ankara";
}