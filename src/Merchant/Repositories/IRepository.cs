using Merchant.V1.Models.RequestModels;

namespace Merchant.Repositories;

public interface IRepository
{
    Task<Merchant> Get(string id);
    Task<List<Merchant>> GetAll(int page, int pageSize, FilterModel filter, SortModel sort);
    Task Post(MerchantCreateRequestModel request);
    Task Delete(Merchant existingMerchant);
    Task<long> Update(Merchant existingMerchant);
}