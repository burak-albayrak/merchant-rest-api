namespace Merchant.V1.Models.ResponseModels;

// Represents the response model for a merchant entity
public class MerchantResponseModel
{
    public MerchantResponseModel(string name, Address address)
    {
        Name = name;
        Address = address;
    }

    // Default constructor
    public MerchantResponseModel()
    {
    }

    public string Name { get; set; }
    public Address Address { get; set; }

    public List<MerchantResponseModel> NewModel(List<Merchant> merchants)
    {
        var merchantResponseList = new List<MerchantResponseModel>();
        foreach (var m in merchants)
        {
            merchantResponseList.Add(new MerchantResponseModel()
            {
                Name = m.Name,
                Address = m.Address
            });
        }

        return merchantResponseList;
    }
}
