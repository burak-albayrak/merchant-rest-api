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
        return merchant;
    }

    public List<Merchant> GetAll()
    {
        var allMerchants = _repository.GetAll();
        return allMerchants;
    }

    public void Post(MerchantCreateRequestModel request)
    {
        _repository.Post(request);
    }

    public void Update(string id, MerchantCreateRequestModel request)
    {
        var existingMerchant = _repository.Get(id);
        if (existingMerchant == null)
        {
            throw new NotFound("Merchant not found");
        }

        existingMerchant.Name = request.Name;
        existingMerchant.Address = request.Address;

        _repository.Update(existingMerchant);
    }
    public void Delete(string id)
    {
        var existingMerchant = _repository.Get(id);
        if (existingMerchant == null)
        {
            throw new NotFound("Merchant not found");
        }

        _repository.Delete(existingMerchant);
    }
    public void UpdateName(string id, string newName)
    {
        var existingMerchant = _repository.Get(id);
        if (existingMerchant == null)
        {
            throw new NotFound("Merchant not found");
        }

        existingMerchant.Name = newName;

        _repository.Update(existingMerchant);
    }
}
