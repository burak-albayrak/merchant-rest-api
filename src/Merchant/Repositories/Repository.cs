using Merchant.Services;
using Merchant.V1.Models.RequestModels;
using MongoDB.Driver;

namespace Merchant.Repositories;

public class Repository : IRepository //Database(data access layer) (database ile haberleşecek olan katman)
{
    private IMongoCollection<Merchant> _collection;

    public Repository(IMongoCollection<Merchant> collection)
    {
        _collection = collection;
    }

    public Merchant Get(string id)
    {
        var merchant = _collection.Find(i => i.Id == id).FirstOrDefault();
        return merchant;
    }

    public List<Merchant> GetAll()
    {
        var allMerchants = _collection.Find(Builders<Merchant>.Filter.Empty).ToList();
        return allMerchants;
    }

    public void Post(MerchantCreateRequestModel request)
    {
        var merchant = new Merchant(request.Name, request.Address);
        _collection.InsertOne(merchant);
    }

    public void Update(Merchant existingMerchant)
    {
        var filter = Builders<Merchant>.Filter.Eq(m => m.Id, existingMerchant.Id);
        var update = Builders<Merchant>.Update
            .Set(m => m.Name, existingMerchant.Name)
            .Set(m => m.Address, existingMerchant.Address);

        _collection.UpdateOne(filter, update);
    }
    public void Delete(Merchant existingMerchant)
    {
        var filter = Builders<Merchant>.Filter.Eq(m => m.Id, existingMerchant.Id);
        _collection.DeleteOne(filter);
    }
}