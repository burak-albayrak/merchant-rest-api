using Merchant.V1.Models.RequestModels;

namespace Merchant.Services;

public interface IService
{
    Task<Merchant> Get(string id);
    Task<Merchant> Post(MerchantCreateRequestModel request);
    Task<long> Update(string id, MerchantUpdateRequestModel request);
    Task<List<Merchant>> GetAll(int page, int pageSize, FilterModel filter, SortingModel sorting);
    Task Delete(string id);
    Task<long> UpdateName(string id, string requestName);
}