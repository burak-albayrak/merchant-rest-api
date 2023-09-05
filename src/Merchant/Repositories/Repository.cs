using Merchant.Exceptions;
using Merchant.Services;
using Merchant.V1.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;
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
            _logger.LogError("Merchant with id {MerchantId} not found!", id);
            throw new NotFound("Merchant not found!");
        }

        _logger.LogInformation("Merchant found: {MerchantName}", merchant.Name);

        return merchant;
    }

    public async Task<List<Merchant>> GetAll(int page, int pageSize,
        FilterRequestModel filterRequest,
        SortingRequestModel sortingRequest)
    {
        var filterDefinition = Builders<Merchant>.Filter.Empty;

        if (!string.IsNullOrEmpty(filterRequest.City))
        {
            filterDefinition &= Builders<Merchant>.Filter.Eq("address.city", filterRequest.City);
        }
        
        var sortField = sortingRequest.SortBy;
        var sortDirection = sortingRequest.SortOrder.ToLower() == "desc" ? -1 : 1;

        SortDefinition<Merchant> sortDefinition;
        
        if (sortDirection == -1)
        {
            sortDefinition = Builders<Merchant>.Sort.Descending(sortField);
        }
        else
        {
            sortDefinition = Builders<Merchant>.Sort.Ascending(sortField);
        }

        var result = await _collection.Find(filterDefinition)
            .Sort(sortDefinition)
            .Limit(pageSize)
            .Skip((page - 1) * pageSize)
            .ToListAsync();

        _logger.LogInformation("Retrieved {MerchantCount} merchants", result.Count);

        return result;
    }

    public async Task<Merchant> Post(MerchantCreateRequestModel request)
    {
        var merchant = new Merchant(request.Name, request.Address, request.ReviewStar, request.ReviewCount);
        await _collection.InsertOneAsync(merchant);
        _logger.LogInformation("Merchant created successfully");
        
        return merchant;
    }

    public async Task<long> Update(Merchant existingMerchant)
    {
        var filter = Builders<Merchant>.Filter.Eq(m => m.Id, existingMerchant.Id);
        var update = Builders<Merchant>.Update
            .Set(m => m.Name, existingMerchant.Name)
            .Set(m => m.Address, existingMerchant.Address)
            .Set(m => m.ReviewStar, existingMerchant.ReviewStar)
            .Set(m => m.ReviewCount, existingMerchant.ReviewCount);

        var updateInfo = await _collection.UpdateOneAsync(filter, update);
        _logger.LogInformation("Merchant updated successfully");

        return updateInfo.MatchedCount;
    }

    public async Task<long> Delete(Merchant existingMerchant)
    {
        var filter = Builders<Merchant>.Filter.Eq(m => m.Id, existingMerchant.Id);
        var deleteInfo = await _collection.DeleteOneAsync(filter);
        if (deleteInfo.DeletedCount == 0)
        {
            _logger.LogWarning("delete");
        }
        _logger.LogInformation("Merchant deleted successfully!");

        return deleteInfo.DeletedCount;
    }
}