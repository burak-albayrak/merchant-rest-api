using Merchant.Services;
using Merchant.V1.Models.RequestModels;
using MongoDB.Driver;

namespace Merchant.Repositories;

public class Repository : IRepository //Database(data access layer) (database ile haberleşecek olan katman)
{
    private IMongoCollection<Merchant> _collection;
    private readonly ILogger<Repository> _logger;

    public Repository(IMongoCollection<Merchant> collection, ILogger<Repository> logger)
    {
        _collection = collection;
        _logger = logger;
    }

    public Merchant Get(string id)
    {
        var merchant = _collection.Find(i => i.Id == id).FirstOrDefault();

        if (merchant == null)
        {
            _logger.LogWarning("Merchant with id {MerchantId} not found!", id);
        }
        else
        {
            _logger.LogInformation("Merchant found: {MerchantName}", merchant.Name);
        }

        return merchant;
    }

    public List<Merchant> GetAll()
    {
        var allMerchants = _collection.Find(Builders<Merchant>.Filter.Empty).ToList();
        _logger.LogInformation("Retrieved {MerchantCount} merchants", allMerchants.Count);

        return allMerchants;
    }

    public void Post(MerchantCreateRequestModel request)
    {
        var merchant = new Merchant(request.Name, request.Address);
        _collection.InsertOne(merchant);
        _logger.LogInformation("Merchant created successfully");
    }

    public long Update(Merchant existingMerchant)
    {
        var filter = Builders<Merchant>.Filter.Eq(m => m.Id, existingMerchant.Id);
        var update = Builders<Merchant>.Update
            .Set(m => m.Name, existingMerchant.Name)
            .Set(m => m.Address, existingMerchant.Address);

        var updateInfo = _collection.UpdateOne(filter, update);
        _logger.LogInformation("Merchant updated successfully");

        return updateInfo.MatchedCount;
    }

    public void Delete(Merchant existingMerchant)
    {
        var filter = Builders<Merchant>.Filter.Eq(m => m.Id, existingMerchant.Id);
        _collection.DeleteOne(filter);
        _logger.LogInformation("Merchant deleted successfully!");
    }
}