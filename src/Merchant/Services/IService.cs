using Merchant.V1.Models.RequestModels;
using Merchant.V1.Helpers;

namespace Merchant.Services;

public interface IService
{
    public Merchant Get(string id);
    public void Post(MerchantCreateRequestModel merchantCreateRequestModel);
    public long Update(string id, MerchantUpdateRequestModel request);
    public List<Merchant> GetAll();
    public void Delete(string id);
    public long UpdateName(string id, string requestName);
    PaginatedList<Merchant> GetPaginated(int page, int pageSize);
    List<Merchant> GetFilteredByName(MerchantNameFilterModel filters);
}