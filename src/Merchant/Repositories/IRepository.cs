using Merchant.V1.Models.RequestModels;

namespace Merchant.Repositories;

public interface IRepository
{
    Task<Merchant> Get(string id);
    Task<List<Merchant>> GetAll(int page, int pageSize, string? searchRequest,
        FilterRequestModel filterRequest, SortingRequestModel sortingRequest);
    Task<Merchant> Post(MerchantCreateRequestModel request);
    Task<long> Delete(Merchant existingMerchant);
    Task<long> Update(Merchant existingMerchant);
}