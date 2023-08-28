using Merchant.V1.Models.RequestModels;

namespace Merchant.Services;

public interface IService
{
    public Merchant Get(string id);
    public void Post(MerchantCreateRequestModel merchantCreateRequestModel);
    public void Update(string id, MerchantCreateRequestModel request);
    public List<Merchant> GetAll();
    public void Delete(string id);
    public void UpdateName(string id, string requestName);
}