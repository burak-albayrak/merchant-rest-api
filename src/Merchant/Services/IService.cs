using Merchant.V1.Models.RequestModels;

namespace Merchant.Services;

public interface IService
{
    public Merchant Get(string id);
    public void Post(MerchantCreateRequestModel merchantCreateRequestModel);
    public long Update(string id, MerchantUpdateRequestModel request);
    public List<Merchant> GetAll(int page, int pageSize, MerchantFilterModel filter, MerchantSortModel sort);
    public void Delete(string id);
    public long UpdateName(string id, string requestName);
}