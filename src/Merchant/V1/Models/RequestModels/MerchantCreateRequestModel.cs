using System.Text.Json.Serialization;

namespace Merchant.V1.Models.RequestModels;

// Represents the request model for creating a merchant
public class MerchantCreateRequestModel
{
    //name of the merchant
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    //address of the merchant
    [JsonPropertyName("address")]
    public Address Address { get; set; }
}