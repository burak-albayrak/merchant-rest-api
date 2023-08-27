namespace Merchant.V1.Models.ResponseModels;

public class MerchantResponseModel
{
    public MerchantResponseModel(string name)
    {
        Name = name;
    }    
    public MerchantResponseModel()
    {
        
    }
    public string Name { get; set; }  
}