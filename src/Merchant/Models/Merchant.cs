using MongoDB.Bson.Serialization.Attributes;

namespace Merchant;

public class Merchant
{
    [BsonElement("name")] 
    public string Name { get; set; }
    [BsonElement("_id")] 
    public string Id { get; set; }
    [BsonElement("address")] 
    public Address Address { get; set; }
    
    [BsonElement("reviewStar")]
    public double ReviewStar { get; set; }

    [BsonElement("reviewCount")]
    public int ReviewCount { get; set; }

    public Merchant(string name, Address address, double reviewStar, int reviewCount)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Address = address;
        ReviewStar = 0;
        ReviewCount = 0;
    }
}