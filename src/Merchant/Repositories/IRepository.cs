using Merchant.V1.Models.RequestModels;

namespace Merchant.Repositories;

public interface IRepository
{
    public Merchant Get(string id);
    List<Merchant> GetAll();
    public void Post(MerchantCreateRequestModel request);
    void Update(Merchant existingMerchant);
    void Delete(Merchant existingMerchant); // Yeni Delete metodu
}