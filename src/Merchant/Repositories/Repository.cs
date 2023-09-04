using Merchant.Services;
using Merchant.V1.Models.RequestModels;
using MongoDB.Driver;

namespace Merchant.Repositories;

public class Repository : IRepository //Database(data access layer) (database ile haberleşecek olan katman)
{
    private readonly IMongoCollection<Merchant> _collection;
    private readonly ILogger<Repository> _logger;

    public Repository(IMongoCollection<Merchant> collection, ILogger<Repository> logger)
    {
        _collection = collection;
        _logger = logger;
    }

    public async Task<Merchant> Get(string id)
    {
        var merchant = await _collection.Find(i => i.Id == id).FirstOrDefaultAsync();

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

    public async Task<List<Merchant>> GetAll(int page, int pageSize,
        FilterModel filter,
        SortModel sort)
    {
        var filterDefinition = Builders<Merchant>.Filter.Empty;

        if (!string.IsNullOrEmpty(filter.City))
        {
            filterDefinition &= Builders<Merchant>.Filter.Eq("address.city", filter.City);
        }

        var sortDirection = sort.SortOrder.ToLower() == "desc" ? -1 : 1;
        var sortField = sort.SortBy;

        var sortDefinition = Builders<Merchant>.Sort.Ascending(sortField);

        if (sortDirection == -1)
        {
            sortDefinition = Builders<Merchant>.Sort.Descending(sortField);
        }

        var result = await _collection.Find(filterDefinition)
            .Sort(sortDefinition)
            .Limit(pageSize)
            .Skip((page - 1) * pageSize)
            .ToListAsync();

        _logger.LogInformation("Retrieved {MerchantCount} merchants", result.Count);

        return result;
    }

    public async Task Post(MerchantCreateRequestModel request)
    {
        var merchant = new Merchant(request.Name, request.Address);
        await _collection.InsertOneAsync(merchant);
        _logger.LogInformation("Merchant created successfully");
    }

    public async Task<long> Update(Merchant existingMerchant)
    {
        var filter = Builders<Merchant>.Filter.Eq(m => m.Id, existingMerchant.Id);
        var update = Builders<Merchant>.Update
            .Set(m => m.Name, existingMerchant.Name)
            .Set(m => m.Address, existingMerchant.Address);

        var updateInfo = await _collection.UpdateOneAsync(filter, update);
        _logger.LogInformation("Merchant updated successfully");

        return updateInfo.MatchedCount;
    }

    public async Task Delete(Merchant existingMerchant)
    {
        var filter = Builders<Merchant>.Filter.Eq(m => m.Id, existingMerchant.Id);
        await _collection.DeleteOneAsync(filter);
        _logger.LogInformation("Merchant deleted successfully!");
    }
}