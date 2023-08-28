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
    
    public Merchant(string name, Address address)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Address = address;
    }
}