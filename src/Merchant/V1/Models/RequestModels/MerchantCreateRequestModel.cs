using System.Text.Json.Serialization;

namespace Merchant.V1.Models.RequestModels;

public class MerchantCreateRequestModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("address")]
    public Address Address { get; set; }
    [JsonPropertyName("reviewStar")]
    public double ReviewStar { get; set; }
    [JsonPropertyName("reviewCount")]
    public int ReviewCount { get; set; }
}