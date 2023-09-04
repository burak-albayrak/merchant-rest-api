using Merchant.Exceptions;
using Merchant.Repositories;
using Merchant.V1.Models.RequestModels;

namespace Merchant.Services;

// Business logiclerin koşacağı katman
public class Service : IService
{
    private readonly IRepository _repository;
    private readonly ILogger<Service> _logger;

    public Service(IRepository repository, ILogger<Service> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Merchant> Get(string id)
    {
        var merchant = await _repository.Get(id);

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

    public async Task<List<Merchant>> GetAll(int page, int pageSize, FilterModel filter, SortModel sort)
    {
        var allMerchants = await _repository.GetAll(page, pageSize, filter, sort);
        _logger.LogInformation("Retrieved {MerchantCount} merchants", allMerchants.Count);

        return allMerchants;
    }

    public async Task Post(MerchantCreateRequestModel request)
    {
        await _repository.Post(request);
        _logger.LogInformation("Merchant created successfully");
    }

    public async Task<long> Update(string id, MerchantUpdateRequestModel request)
    {
        var existingMerchant = await _repository.Get(id);
        if (existingMerchant == null)
        {
            throw new MerchantNotFound("Merchant not found");
        }

        existingMerchant.Name = request.Name;
        existingMerchant.Address = request.Address;

        var count = await _repository.Update(existingMerchant);
        _logger.LogInformation("Merchant updated successfully");

        return count;
    }

    public async Task<long> UpdateName(string id, string newName)
    {
        var existingMerchant = await _repository.Get(id);
        if (existingMerchant == null)
        {
            throw new MerchantNotFound("Merchant Not Found!");
        }

        existingMerchant.Name = newName;

        var count = await _repository.Update(existingMerchant);
        _logger.LogInformation("Merchant name updated: {MerchantId}, New Name: {NewName}", id, newName);

        return count;
    }

    public async Task Delete(string id)
    {
        var existingMerchant = await _repository.Get(id);
        if (existingMerchant == null)
        {
            throw new MerchantNotFound("Merchant Not Found!");
        }

        await _repository.Delete(existingMerchant);
        _logger.LogInformation("Merchant deleted successfully: {MerchantId}", id);
    }
}
