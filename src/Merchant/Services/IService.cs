using Merchant.V1.Models.RequestModels;

namespace Merchant.Services;

public interface IService
{
    Task<Merchant> Get(string id);
    Task Post(MerchantCreateRequestModel merchantCreateRequestModel);
    Task<long> Update(string id, MerchantUpdateRequestModel request);
    Task<List<Merchant>> GetAll(int page, int pageSize, FilterModel filter, SortModel sort);
    Task Delete(string id);
    Task<long> UpdateName(string id, string requestName);
}