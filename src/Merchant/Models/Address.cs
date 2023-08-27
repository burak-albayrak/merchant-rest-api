using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Merchant;

public class Address
{
    [BsonElement("city")]
    [JsonPropertyName("city")]
    public string City { get; set; }
    
    [JsonPropertyName("cityCode")]
    [BsonElement("cityCode")]
    public int CityCode { get; set; }
}