namespace Merchant.V1.Models.ResponseModels;

// Represents the response model for a merchant entity
public class MerchantResponseModel
{
    // Constructor with parameters to set the 'Name' property
    public MerchantResponseModel(string name)
    {
        Name = name;
    }

    // Default constructor
    public MerchantResponseModel()
    {
    }

    //name of the merchant
    public string Name { get; set; }
}
