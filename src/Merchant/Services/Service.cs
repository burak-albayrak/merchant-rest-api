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

    public Merchant Get(string id)
    {
        var merchant = _repository.Get(id);

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
        var allMerchants = _repository.GetAll();
        _logger.LogInformation("Retrieved {MerchantCount} merchants", allMerchants.Count);

        return allMerchants;
    }

    public void Post(MerchantCreateRequestModel request)
    {
        _repository.Post(request);
        _logger.LogInformation("Merchant created successfully");
    }

    public long Update(string id, MerchantUpdateRequestModel request)
    {
        var existingMerchant = _repository.Get(id);
        if (existingMerchant == null)
        {
            throw new NotFound("Merchant not found");
        }

        existingMerchant.Name = request.Name;
        existingMerchant.Address = request.Address;

        var count = _repository.Update(existingMerchant);
        _logger.LogInformation("Merchant updated successfully");

        return count;
    }

    public long UpdateName(string id, string newName)
    {
        var existingMerchant = _repository.Get(id);
        if (existingMerchant == null)
        {
            throw new NotFound("Merchant not found");
        }

        existingMerchant.Name = newName;

        var count = _repository.Update(existingMerchant);
        _logger.LogInformation("Merchant name updated: {MerchantId}, New Name: {NewName}", id, newName);
        
        return count;
    }

    public void Delete(string id)
    {
        var existingMerchant = _repository.Get(id);
        if (existingMerchant == null)
        {
            throw new NotFound("Merchant not found");
        }

        _repository.Delete(existingMerchant);
        _logger.LogInformation("Merchant deleted successfully: {MerchantId}", id);
    }
}