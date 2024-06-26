namespace Merchant.V1.Models.ResponseModels;

// Represents the response model for a merchant entity
public class MerchantResponseModel
{
    public string Name { get; set; }
    public double ReviewStar { get; set; }
    public int ReviewCount { get; set; }
    public Address Address { get; set; }


    public List<MerchantResponseModel> NewModel(List<Merchant> merchants)
    {
        var merchantResponseList = new List<MerchantResponseModel>();
        foreach (var m in merchants)
        {
            merchantResponseList.Add(new MerchantResponseModel()
            {
                Name = m.Name,
                ReviewStar = m.ReviewStar,
                ReviewCount = m.ReviewCount,
                Address = m.Address
            });
        }

        return merchantResponseList;
    }
}